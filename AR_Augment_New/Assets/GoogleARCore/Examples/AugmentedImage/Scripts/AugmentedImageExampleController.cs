//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google LLC">
//
// Copyright 2018 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System;
    using System.Linq;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for AugmentedImage example.
    /// </summary>
    /// <remarks>
    /// In this sample, we assume all images are static or moving slowly with
    /// a large occupation of the screen. If the target is actively moving,
    /// we recommend to check <see cref="AugmentedImage.TrackingMethod"/> and
    /// render only when the tracking method equals to
    /// <see cref="AugmentedImageTrackingMethod"/>.<c>FullTracking</c>.
    /// See details in <a href="https://developers.google.com/ar/develop/c/augmented-images/">
    /// Recognize and Augment Images</a>
    /// </remarks>
    public class AugmentedImageExampleController : MonoBehaviour
    {
        //経路コンストラクタ
        public Keiro Keiro = new Keiro();

        //矢印
        public Yazirushi _yazirushi=new Yazirushi();

        //ナビ用のボタン設定
        public Button Btn_menu1;
        public Button Btn_menu2;
        public Button Btn_menu3;
        public Button Btn_menu4;

        //方角表示オブジェクト
        public GameObject obj_arrow;

        //デバッグ用テキスト
        public Text DebugText;

        //現在地を取得しナビが開始できるかのフラグ
        public bool NaviFlag = false;
        //目的地に着いたか
        public bool GoalFlag = false;

        //目的地のノード番号
        public int Distination = 0;

        //ダイクストラの結果（チェックポイントの順番と総コスト）
        Dijkstra.Result result;

        //オブジェクトをスポーンさせるための方角
        int Yanomuki = -1;

        //public Camera Cam;

        private AugmentedImageVisualizer PrevVisualizer = null;
        private AugmentedImageVisualizer CurrentVisualizer = null;
        AugmentedImageVisualizer visualizer2 = null;

        private AugmentedImage LateImage = null;

        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        private Dictionary<int, AugmentedImageVisualizer> _visualizers
            = new Dictionary<int, AugmentedImageVisualizer>();

        private List<AugmentedImage> _tempAugmentedImages = new List<AugmentedImage>();

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
            Btn_menu1.onClick.AddListener(() => Navigate(1));
            Btn_menu2.onClick.AddListener(() => Navigate(2));
            Btn_menu3.onClick.AddListener(() => Navigate(3));
            Btn_menu4.onClick.AddListener(() => Navigate(8));

        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            

            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(
                _tempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do
            // not previously have a visualizer. Remove visualizers for stopped images.
            foreach (var image in _tempAugmentedImages)
            {
                AugmentedImageVisualizer visualizer = null;
                _visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = (AugmentedImageVisualizer)Instantiate(
                        AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    LateImage = image;
                    _visualizers.Add(image.DatabaseIndex, visualizer);
                    //最後に読み取ったvisualizerを保存

                    CurrentVisualizer = visualizer;
                    
                }
                else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
                {
                    _visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
                }
            }
            if(Distination == 0 && NaviFlag == false && GoalFlag == false && CurrentVisualizer != null){
                DebugText.text = "目的地を\n選択してください";
            }
            if(_visualizers.Count >= 2){
                //最初にスポーンさせたオブジェクトを非表示にする
                _visualizers.First().Value.gameObject.SetActive(false);

                //そのオブジェクトの情報を持つ要素を削除
                _visualizers.Remove(_visualizers.First().Key);

                //最後のオブジェクトを現在のオブジェクトにする
                CurrentVisualizer = _visualizers.First().Value;

                //最新の現在地からルートを再計算
                if(NaviFlag == true && int.Parse(CurrentVisualizer.Image.Name.Substring(1,CurrentVisualizer.Image.Name.Length - 1)) != Distination){
                    Navigate(Distination);
                }
            }

            if (NaviFlag == true){
                DebugText.text = "方角表示に従って\n移動してください";
                //DebugText.text = "現在地:"+CurrentVisualizer.Image.Name.Substring(1,CurrentVisualizer.Image.Name.Length - 1);
                //DebugText.text += "@"+Distination.ToString();
                foreach (var rt in result.cost)
                {
                    //DebugText.text += "[" + rt.ToString()+"]";
                    
                }
                if (int.Parse(CurrentVisualizer.Image.Name.Substring(1,CurrentVisualizer.Image.Name.Length - 1)) == Distination)
                {
                    DebugText.text = "目的地に到着しました";
                    _visualizers.First().Value.gameObject.transform.position = CurrentVisualizer.Image.CenterPose.position;
                    //目的地についたので初期化
                    Distination = 0;
                    NaviFlag = false;
                    GoalFlag = true;
                    //次のチェックポイントにマーカーを設置する（しなおす）
                    _visualizers.TryGetValue(LateImage.DatabaseIndex, out visualizer2);
                    visualizer2.gameObject.transform.position = CurrentVisualizer.DistinationMarker.transform.localPosition;
                    return;
                }
                //経路結果から設置すべき座標をオブジェクトの座標に更新する
                else if (result.cost.Count >= 2)
                {
                    //まだ目的地に着いていないので次のチェックポイントへ
                    ConvCoodinate(result.cost[1], Yanomuki, CurrentVisualizer.DistinationMarker.transform);
                }
                
                //次のチェックポイントに方角表示オブジェクトを向ける
                arrowController();
                
            }
            

            // Show the fit-to-scan overlay if there are no images that are Tracking.
            foreach (var visualizer in _visualizers.Values)
            {
                if (visualizer.Image.TrackingState == TrackingState.Tracking)
                {
                    FitToScanOverlay.SetActive(false);
                    return;
                }
            } 

            FitToScanOverlay.SetActive(true);
        }

        public void Navigate(int Distinatuon)
        {
            Distination = Distinatuon;

            if(CurrentVisualizer != null)
            {
                int ImageNum = int.Parse(CurrentVisualizer.Image.Name.Substring(1,CurrentVisualizer.Image.Name.Length - 1));
                Yanomuki = -1;
                result = Keiro.GetMindistance(ImageNum,Distinatuon);
                NaviFlag = true;

                if(result.route.Count>0)
                {
                    Yanomuki = _yazirushi.Getyazirushi(result);
                }
                
                /* foreach (var rt in result.cost)
                {
                    DebugText.text += "[" + rt.ToString()+"]";
                    
                } */
                //DebugText.text += string.Format("矢{0}",Yanomuki);
            }
            else{
                DebugText.text = "マーカーが見つかりません";
            }
        }

        //方角表示オブジェクトのコントロール
        public void arrowController(){
            if(CurrentVisualizer != null)
            {
                obj_arrow.transform.LookAt(CurrentVisualizer.DistinationMarker.transform.localPosition);
                
            }
            
        }

        //距離と方角から座標に変換
        public void ConvCoodinate(int distance, int direction, Transform trans){
            float x = (float)distance * (float)Math.Cos(DireNum2Rad(direction)) + trans.localPosition.x;
            float z = (float)distance * (float)Math.Sin(DireNum2Rad(direction)) + trans.localPosition.z;
            trans.localPosition = new Vector3(x,trans.localPosition.y,z);
        }
        //方角番号からラジアンに変換する関数
        public double DireNum2Rad(int n){
            int baseDire = (int)CurrentVisualizer.Image.CenterPose.rotation.eulerAngles.y;
            switch (n)
            {
                case 0:
                    return (90-baseDire) * (Math.PI / 180);
                case 1:
                    return (45-baseDire) * (Math.PI / 180);
                case 2:
                    return (0-baseDire) * (Math.PI / 180);
                case 3:
                    return (315-baseDire) * (Math.PI / 180);
                case 4:
                    return (270-baseDire) * (Math.PI / 180);
                case 5:
                    return (225-baseDire) * (Math.PI / 180);
                case 6:
                    return (180-baseDire) * (Math.PI / 180);
                case 7:
                    return (135-baseDire) * (Math.PI / 180);
                
                default:
                    return 90 * (Math.PI / 180);
            }
        }

    }
}

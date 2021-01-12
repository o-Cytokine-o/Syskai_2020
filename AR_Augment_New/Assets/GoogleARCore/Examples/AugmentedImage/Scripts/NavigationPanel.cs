using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : MonoBehaviour
{
    public RectTransform rect;
    //public Camera cam;
    
    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 prevPoint;
    float panelDiff;
    

    //デバッグ用テキスト
    public Text DebugText;

    void Start()
    {
        
    }

    // ナビゲーションパネルのタッチ操作
    void Update()
    {
        // タッチされているかチェック
        if (Input.touchCount > 0) 
        {
            // タッチ情報の取得
            Touch touch = Input.GetTouch(0);

            //タッチされたときの処理
            if (touch.phase == TouchPhase.Began) {
                startPoint = touch.position;
                panelDiff = touch.position.y - rect.position.y;
                prevPoint = startPoint;
            }

            //スワイプ操作処理
            if (touch.phase == TouchPhase.Moved) {
                Vector3 currentstartPoint = touch.position;
    　　　　　　　　//指をずらしたときのベクトルを取得
                Vector3 PointDiff = (currentstartPoint - prevPoint);
                prevPoint = currentstartPoint;

                rect.position = new Vector3(rect.position.x,rect.position.y + (PointDiff.y * 1f),rect.position.z);
            }

            //指が画面から離れたとき
            if (touch.phase == TouchPhase.Ended){
                endPoint = touch.position;

                //自動でナビゲーションバーの開閉をする
                if(endPoint.y > startPoint.y){
                    panelOpen();
                }
                if(endPoint.y < startPoint.y){
                    panelClose();
                }
            }
        }
        
        rect.position = new Vector3(rect.position.x,Mathf.Clamp(rect.position.y, 0f, 1400f),rect.position.z);
        //DebugText.text = rect.position.ToString();
    }

    //自動で開く
    async void panelOpen(){
        int time = 10;
        float speed = 200f;
        while (rect.position.y <= 1300){
            rect.position = new Vector3(rect.position.x, rect.position.y + speed,rect.position.z);
            await Task.Delay(time);
            //非同期処理のためタッチされたら中断する
            if(Input.touchCount > 0){
                break;
            }
        }
    }

    //自動で閉じる
    async void panelClose(){
        int time = 10;
        float speed = 200f;
        while (rect.position.y >= 100){
            rect.position = new Vector3(rect.position.x, rect.position.y - speed,rect.position.z);
            await Task.Delay(time);
            if(Input.touchCount > 0){
                break;
            }
        }
    }
}

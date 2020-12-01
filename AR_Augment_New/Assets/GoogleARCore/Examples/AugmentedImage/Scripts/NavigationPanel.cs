using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : MonoBehaviour
{
    public RectTransform rect;

    Vector3 tapPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // タッチされているかチェック
        if (Input.touchCount > 0) {
            // タッチ情報の取得
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                tapPoint = touch.position;
            }

            if (touch.phase == TouchPhase.Moved) {
                Vector3 currentTapPoint = touch.position;
    　　　　　　　　//指をずらしたときのベクトルを取得
                Vector3 PointDiff = (currentTapPoint - tapPoint);

                rect.position = new Vector3(rect.position.x,PointDiff.y * 500f,rect.position.z);
            }
        }
    }
}

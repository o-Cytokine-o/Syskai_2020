# 開発ドキュメント

## 「目標の場所にマーカーを設置」について

基本的にAugmentedImageVisualizerをいじって動かす。  
DistinationMarker.transform.localPositionにVector3の値を入れるとマーカーの中心から動かせる。  

Vector3（0,0,0）にすると画像の中心に来る（それぞれX,Y,Z）  
画像に対して左右に動くのがX,前後がZ,奥行きがY  

## ナビゲーションのロジックについて
マーカー同士でお互いの座標の情報を持っておき、その情報を使って経路を計算する。  
情報は隣のマーカーのみで、隣接するマーカーのIDをキーにしてそのマーカーの座標を保存する。  

```
マーカーAが持っているデータ  
    [マーカーB：（2,0,7）]  
    [マーカーC：（4,0,8）]  
   
マーカーBが持っているデータ  
    [マーカーD:(2,0,14)]  
```
 ![1](https://user-images.githubusercontent.com/40162639/98762476-0d40ce80-241b-11eb-8376-c5cc6b8ef862.png)  
 
 ## マーカー画像の作成について
 自作でマーカーを作ろうとしたがKeypointが足らず使えなかった
 ![image](https://user-images.githubusercontent.com/40162639/100169152-1bf0b080-2f06-11eb-8362-4b8849582869.png)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Keiro : MonoBehaviour
{
    private var graph;

    public Keiro()
    {

        //n:頂点の個数
        int n = 1;
        graph = new Dikstra(n);

        //Add(枝元,枝先,重み)
        //頂点n個分の情報を手動で追加
        graph.Add(0, 1, 1);
    }

    private int GetMindistance(int nowVertex, int toVertex)
    {
        var mindistance = graph.GetMinCost(nowVertex);
        int distance = mindistance[toVertex];

        return distance;
    }

    private var GetKeiro(int nowVertex){
        
    }


}

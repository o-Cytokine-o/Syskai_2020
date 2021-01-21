using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Keiro 
{
    private Dijkstra graph;

    public Keiro()
    {

        //n:頂点の個数
        int n = 11;
        graph = new Dijkstra(n);

        //Add(枝元,枝先,重み)
        //頂点n個分の情報を手動で追加
        // 辺の情報を追加する(無向グラフなので両方の向きに)
        graph.Add(1, 2, 6);
        graph.Add(2, 1, 6);
        graph.Add(2, 3, 6);
        graph.Add(3, 2, 6);
        graph.Add(3, 4, 3);
        graph.Add(3, 5, 3);
        graph.Add(4, 3, 3);
        graph.Add(4, 6, 3);
        graph.Add(4, 7, 3);
        graph.Add(4, 8, 6);
        graph.Add(5, 3, 3);
        graph.Add(6, 4, 3);
        graph.Add(7, 4, 3);
        graph.Add(8, 4, 6);
        graph.Add(8, 9, 6);
        graph.Add(9, 8, 6);
        graph.Add(9, 10, 3);
        graph.Add(10, 9, 3);　
    }

    public Dijkstra.Result GetMindistance(int nowVertex, int toVertex)
    {
        Dijkstra.Result result = graph.GetMinCost(nowVertex, toVertex);
        return result;
    }



}

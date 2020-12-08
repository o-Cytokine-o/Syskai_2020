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
        int n = 6;
        graph = new Dijkstra(n);

        //Add(枝元,枝先,重み)
        //頂点n個分の情報を手動で追加
        // 辺の情報を追加する(無向グラフなので両方の向きに)
        graph.Add(0, 1, 7);
        graph.Add(0, 2, 14);
        graph.Add(0, 3, 9);
        graph.Add(1, 0, 7);
        graph.Add(1, 3, 10);
        graph.Add(1, 4, 15);
        graph.Add(2, 0, 14);
        graph.Add(2, 3, 2);
        graph.Add(2, 5, 9);
        graph.Add(3, 0, 9);
        graph.Add(3, 2, 2);
        graph.Add(3, 4, 11);
        graph.Add(4, 1, 15);
        graph.Add(4, 3, 11);
        graph.Add(4, 5, 6);
        graph.Add(5, 2, 9);
        graph.Add(5, 4, 6);　
    }

    public Dijkstra.Result GetMindistance(int nowVertex, int toVertex)
    {
        Dijkstra.Result result = graph.GetMinCost(nowVertex, toVertex);
        return result;
    }



}

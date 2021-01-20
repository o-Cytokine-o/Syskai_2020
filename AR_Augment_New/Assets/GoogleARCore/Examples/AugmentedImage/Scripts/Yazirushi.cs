using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Yazirushi
{
    private const int N = 11;
    private Dijkstra graph;
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="n">頂点数</param>
    public Yazirushi()
    {
        int n = 11;
        graph = new Dijkstra(n);

        //Add(枝元,枝先,方角)
        //頂点n個分の情報を手動で追加
        // 辺の情報を追加する(無向グラフなので両方の向きに)
        graph.Add(1, 2, 4);
        graph.Add(2, 1, 0);
        graph.Add(2, 3, 4);
        graph.Add(3, 2, 0);
        graph.Add(3, 4, 2);
        graph.Add(3, 5, 3);
        graph.Add(4, 3, 6);
        graph.Add(4, 6, 3);
        graph.Add(4, 7, 0);
        graph.Add(4, 8, 2);
        graph.Add(5, 3, 3);
        graph.Add(6, 4, 7);
        graph.Add(7, 4, 4);
        graph.Add(8, 4, 6);
        graph.Add(8, 9, 4);
        graph.Add(9, 8, 6);
        graph.Add(9, 10, 2);
        graph.Add(10, 9, 6);
    }

    public int Getyazirushi(Dijkstra.Result result)
    {
        int yazirushiNumber = -1;
        Dijkstra.Result yazirushiResult = graph.GetMinCost(result.route[0], result.route[1]);
        for (int i = 0; i < yazirushiResult.route.Count; i++)
        {
            yazirushiNumber = yazirushiResult.cost[i];
        }
        return yazirushiNumber;
    }
}
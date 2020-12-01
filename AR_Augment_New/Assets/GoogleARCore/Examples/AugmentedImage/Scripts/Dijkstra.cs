using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dijkstra : MonoBehaviour
{

    public int N { get; }               // 頂点の数
    private List<Edge>[] _graph;        // グラフの辺のデータ
    int[] last_update_node_ids= (new int[6]).Select(v => 99999).ToArray();   // 各頂点距離の最後に変更した頂点ID保存用 未計測状態として初期化
 
    public Dijkstra(int n)
    {
        N = n;
        _graph = new List<Edge>[n];
        for (int i = 0; i < n; i++) _graph[i] = new List<Edge>();
    }

    
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="n">頂点数</param>


    /// <summary>
    /// 辺を追加
    /// </summary>
    /// <param name="a">接続元の頂点</param>
    /// <param name="b">接続先の頂点</param>
    /// <param name="cost">コスト</param>
    public void Add(int a, int b, long cost = 1)
            => _graph[a].Add(new Edge(b, cost));

    /// <summary>
    /// 最短経路のコストを取得
    /// </summary>
    /// <param name="start">開始頂点</param>
    public Result GetMinCost(int start)
    {
        var keiroList = new List<int>(); 

        // コストをスタート頂点以外を無限大に
        var cost = new int[N];
        for (int i = 0; i < N; i++) cost[i] = 1000000000;
        cost[start] = 0;

        int[] last_update_node_ids = (new int[6]).Select(v => 99999).ToArray();   // 各頂点距離の最後に変更した頂点ID保存用 未計測状態として初期化
        last_update_node_ids[start] = -1;
        Result result = new Result(true);

        // 未確定の頂点を格納する優先度付きキュー(コストが小さいほど優先度が高い)
        var q = new PriorityQueue<Vertex>(N * 10, Comparer<Vertex>.Create((a, b) => b.CompareTo(a)));
        q.Push(new Vertex(start, 0));

        while (q.Count > 0)
        {
            var v = q.Pop();

            // 記録されているコストと異なる(コストがより大きい)場合は無視
            if (v.cost != cost[v.index]) continue;

            // 今回確定した頂点からつながる頂点に対して更新を行う
            foreach (var e in _graph[v.index])
            {
                if (cost[e.to] > v.cost + e.cost)
                {
                    // 既に記録されているコストより小さければコストを更新
                    cost[e.to] = v.cost + e.cost;
                    q.Push(new Vertex(e.to, cost[e.to]));
                    
                    // 更新した経路の始点側の頂点IDを記録する
                    last_update_node_ids[e.to] = v.index;
                }
            }
        }

         // 最短ルートを取得
        int current_route_id = goal;    // 今チェック中の経路ID
        result.route.Insert(0, goal);

        while (true)
        {
            // 頂点配列から次の頂点IDを取得する
            int next_id = last_update_node_ids[current_route_id];

            // 始点じゃなかったら追加
            if (current_route_id != start)
            {
                result.route.Insert(0, next_id);
                current_route_id = next_id;
            }
            else
            {
                break;
            }
        }

            for(int i=0; i< result.route.Count(); i++)
            {
                result.cost.Add(cost[result.route[i]]);
            }

            // 確定したコストを返す
            //return cost;
            //return shortest_route;
            return result;
    }

    public struct Edge
    {
        public int to;                      // 接続先の頂点
        public int cost;                   // 辺のコスト

        public Edge(int to, int cost)
        {
            this.to = to;
            this.cost = cost;
        }
    }

    public struct Vertex : IComparable<Vertex>
    {
        public int index;                   // 頂点の番号
        public int cost;                   // 記録したコスト

        public Vertex(int index, int cost)
        {
            this.index = index;
            this.cost = cost;
        }

        public int CompareTo(Vertex other)
            => cost.CompareTo(other.cost);
    }

    public struct Result
    {
        public List<int> route;
        public List<int> cost;

        public Result(bool flag)
        {
            route = new List<int>();
            cost = new List<int>();
        }
    }
}



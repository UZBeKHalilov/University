using System;
using System.Collections.Generic;
using System.Linq;

public class Edge : IComparable<Edge>
{
    public int Source, Destination, Weight;
    public int CompareTo(Edge other) => this.Weight.CompareTo(other.Weight);
}

public class KruskalMST
{
    public static int Find(int[] parent, int i)
    {
        if (parent[i] == i) return i;
        return parent[i] = Find(parent, parent[i]);
    }

    public static void Union(int[] parent, int x, int y)
    {
        int xroot = Find(parent, x);
        int yroot = Find(parent, y);
        parent[xroot] = yroot;
    }

    public static void Main()
    {
        int V = 5; // Tugunlar soni (1 dan 5 gacha)
        List<Edge> edges = new List<Edge>
        {
            new Edge {Source = 1, Destination = 2, Weight = 1},
            new Edge {Source = 1, Destination = 3, Weight = 2},
            new Edge {Source = 1, Destination = 4, Weight = 3},
            new Edge {Source = 1, Destination = 5, Weight = 4},
            new Edge {Source = 2, Destination = 3, Weight = 5},
            new Edge {Source = 2, Destination = 5, Weight = 7},
            new Edge {Source = 3, Destination = 4, Weight = 6}
        };

        edges.Sort(); // O(E log E) yoki O(E log V)

        int[] parent = new int[V + 1];
        for (int i = 1; i <= V; i++) parent[i] = i;

        int mstWeight = 0;
        int edgesCount = 0;

        foreach (var edge in edges)
        {
            int x = Find(parent, edge.Source);
            int y = Find(parent, edge.Destination);

            if (x != y) // Agar sikl hosil qilmasa
            {
                mstWeight += edge.Weight;
                Union(parent, x, y);
                edgesCount++;
            }

            if (edgesCount == V - 1) break;
        }

        Console.WriteLine($"Minimal qamrovchi daraxt og'irligi: {mstWeight}");
    }
}
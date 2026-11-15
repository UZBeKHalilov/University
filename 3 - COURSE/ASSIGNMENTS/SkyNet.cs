using System.Text;

// ═══════════════════════════════════════════════════════════════════════════════
//  SkyNet – Global Aviation Logistics & Management System
//  Data Structures & Algorithms – C# Implementation
//  Covers: Graph + Dijkstra, Max-Heap, FIFO Queue, LIFO Stack,
//          Hash Table (chaining), BST, QuickSort, MergeSort, KMP, Backtracking
// ═══════════════════════════════════════════════════════════════════════════════



public interface IPricingStrategy
{
    decimal CalculateTotal(List<IMenuItem> items);
}
public class StandardPricing : IPricingStrategy
{
    public decimal CalculateTotal(List<IMenuItem> items)
        => items.Sum(i => i.GetPrice());
}
public class HappyHourPricing : IPricingStrategy
{
    public decimal CalculateTotal(List<IMenuItem> items)
        => items.Sum(i => i.GetPrice()) * 0.80m; // 20% off
}
public class LoyaltyCardPricing : IPricingStrategy
{
    public decimal CalculateTotal(List<IMenuItem> items)
    {
        decimal total = items.Sum(i => i.GetPrice()) * 0.90m; // 10% off
        // free drink: deduct cheapest Beverage
        var cheapDrink = items.OfType<Beverage>().OrderBy(b => b.GetPrice()).FirstOrDefault();
        return cheapDrink != null ? total - cheapDrink.GetPrice() : total;
    }
}

public sealed class OrderHistoryLog
{
    private static readonly Lazy<OrderHistoryLog> _instance
        = new(() => new OrderHistoryLog());
    public static OrderHistoryLog Instance => _instance.Value;
    private OrderHistoryLog() { }   // private constructor

    private readonly List<OrderRecord> _records = new();

    public void Append(OrderRecord record)
    {
        if (record == null) throw new ArgumentNullException(nameof(record));
        _records.Add(record);
    }
    public IEnumerable<OrderRecord> ByDateRange(DateTime start, DateTime end)
        => _records.Where(r => r.Timestamp >= start && r.Timestamp <= end);
    public IEnumerable<OrderRecord> ByTable(int tableId)
        => _records.Where(r => r.TableId == tableId);
    public string TopOrderedItem()
        => _records.SelectMany(r => r.Items)
                   .GroupBy(i => i).OrderByDescending(g => g.Count())
                   .FirstOrDefault()?.Key ?? "None";
}

public interface IKitchenCommand
{
    void Execute();
    void Undo();
}
public class PrepareOrderCommand : IKitchenCommand
{
    private readonly Order _order;
    private readonly Chef _chef;
    public PrepareOrderCommand(Order order, Chef chef)
    {
        _order = order ?? throw new ArgumentNullException(nameof(order));
        _chef = chef ?? throw new ArgumentNullException(nameof(chef));
    }
    public void Execute() { _order.SetStatus("Preparing"); _chef.StartPrep(_order); }
    public void Undo() { _order.SetStatus("Cancelled"); _chef.CancelPrep(_order); }
}
public class KitchenQueue
{
    private readonly Queue<IKitchenCommand> _pending = new();
    private readonly Stack<IKitchenCommand> _history = new();
    public void Submit(IKitchenCommand cmd)
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));
        _pending.Enqueue(cmd);
    }
    public void ProcessNext()
    {
        if (_pending.Count == 0) return;
        var cmd = _pending.Dequeue();
        cmd.Execute();
        _history.Push(cmd);
    }
    public void Undo()
    {
        if (_history.Count == 0) throw new InvalidOperationException("Nothing to undo.");
        _history.Pop().Undo();
    }
}

public class BillingService
{
    private IPricingStrategy _strategy;
    public BillingService(IPricingStrategy strategy) => _strategy = strategy;
    public void SetStrategy(IPricingStrategy s) => _strategy = s; // swap at runtime

    public decimal ProcessAndLog(Order order, int tableId, int staffId)
    {
        decimal total = _strategy.CalculateTotal(order.Items);
        // Command already queued and executed by KitchenQueue before billing
        // Write to Singleton log
        OrderHistoryLog.Instance.Append(new OrderRecord
        {
            OrderId = order.Id,
            TableId = tableId,
            StaffId = staffId,
            Items = order.Items.Select(i => i.GetDescription()).ToList(),
            Total = total,
            Timestamp = DateTime.UtcNow
        });
        return total;
    }
}













public abstract class MenuItem
{
    public abstract string GetDescription();
    public abstract decimal GetPrice();
}
public class Starter : MenuItem
{
    public override string GetDescription() => $"[Starter] {Name}";
    public override decimal GetPrice() => _basePrice;
}


namespace SkyNet
{
    // ─────────────────────────────────────────────────────────────────────────
    // PHASE 1 – GRAPH + DIJKSTRA'S ALGORITHM
    // ─────────────────────────────────────────────────────────────────────────



    public class Bill
    {
        private decimal _subtotal;
        private decimal _taxRate = 0.20m; // 20% VAT
        public decimal GetTotal() => _subtotal * (1 + _taxRate);
        public void AddLineItem(BillLineItem item) => _subtotal += item.Price;
    }



    /// <summary>Weighted undirected flight network (adjacency list).</summary>
    public class FlightGraph
    {
        private readonly Dictionary<string, List<(string Dest, double Weight)>> _adj
            = new Dictionary<string, List<(string, double)>>();

        public void AddAirport(string id)
        {
            if (!_adj.ContainsKey(id))
                _adj[id] = new List<(string, double)>();
        }

        public void AddRoute(string src, string dst, double weight)
        {
            if (weight < 0) throw new ArgumentException("Route weight must be non-negative.");
            AddAirport(src);
            AddAirport(dst);
            _adj[src].Add((dst, weight));
            _adj[dst].Add((src, weight));
        }

        public void RemoveRoute(string src, string dst)
        {
            if (_adj.ContainsKey(src)) _adj[src].RemoveAll(e => e.Dest == dst);
            if (_adj.ContainsKey(dst)) _adj[dst].RemoveAll(e => e.Dest == src);
        }

        public IEnumerable<(string Dest, double Weight)> GetNeighbours(string id)
            => _adj.ContainsKey(id) ? _adj[id] : new List<(string, double)>();

        /// <summary>Dijkstra's shortest path. Returns (cost, path) or (∞, empty) if unreachable.</summary>
        public (double Cost, List<string> Path) Dijkstra(string start, string end)
        {
            if (!_adj.ContainsKey(start) || !_adj.ContainsKey(end))
                return (double.PositiveInfinity, new List<string>());

            // Min-heap: (cost, node)
            var heap = new SortedSet<(double Cost, string Node)>(
                Comparer<(double, string)>.Create((a, b) =>
                    a.Item1 != b.Item1 ? a.Item1.CompareTo(b.Item1) : string.Compare(a.Item2, b.Item2)));

            var dist    = new Dictionary<string, double>();
            var prev    = new Dictionary<string, string>();
            var visited = new HashSet<string>();

            foreach (var node in _adj.Keys) dist[node] = double.PositiveInfinity;
            dist[start] = 0;
            heap.Add((0, start));

            while (heap.Count > 0)
            {
                var (cost, node) = heap.Min;
                heap.Remove(heap.Min);

                if (visited.Contains(node)) continue;
                visited.Add(node);

                if (node == end)
                    return (cost, BuildPath(prev, start, end));

                foreach (var (nbr, w) in _adj[node])
                {
                    if (visited.Contains(nbr)) continue;
                    double newCost = cost + w;
                    if (newCost < dist[nbr])
                    {
                        dist[nbr] = newCost;
                        prev[nbr] = node;
                        heap.Add((newCost, nbr));
                    }
                }
            }
            return (double.PositiveInfinity, new List<string>());
        }

        private static List<string> BuildPath(Dictionary<string, string> prev, string start, string end)
        {
            var path = new List<string>();
            string cur = end;
            while (cur != start) { path.Insert(0, cur); cur = prev[cur]; }
            path.Insert(0, start);
            return path;
        }

        /// <summary>Kruskal's MST for backup communication network.</summary>
        public List<(string Src, string Dst, double Weight)> KruskalMST()
        {
            // Collect all edges (undirected – add each once)
            var edges = new List<(double W, string U, string V)>();
            var seen  = new HashSet<string>();
            foreach (var (u, neighbours) in _adj)
                foreach (var (v, w) in neighbours)
                {
                    string key = string.Compare(u, v) < 0 ? $"{u}-{v}" : $"{v}-{u}";
                    if (seen.Add(key)) edges.Add((w, u, v));
                }
            edges.Sort((a, b) => a.W.CompareTo(b.W));

            var uf  = new UnionFind(_adj.Keys);
            var mst = new List<(string, string, double)>();
            foreach (var (w, u, v) in edges)
                if (uf.Union(u, v))
                    mst.Add((u, v, w));
            return mst;
        }
    }

    /// <summary>Union-Find (Disjoint Set) for Kruskal's MST.</summary>
    internal class UnionFind
    {
        private readonly Dictionary<string, string> _parent = new Dictionary<string, string>();
        private readonly Dictionary<string, int>    _rank   = new Dictionary<string, int>();

        public UnionFind(IEnumerable<string> nodes)
        {
            foreach (var n in nodes) { _parent[n] = n; _rank[n] = 0; }
        }

        public string Find(string x)
        {
            if (_parent[x] != x) _parent[x] = Find(_parent[x]);
            return _parent[x];
        }

        public bool Union(string x, string y)
        {
            string rx = Find(x), ry = Find(y);
            if (rx == ry) return false;
            if (_rank[rx] < _rank[ry]) (rx, ry) = (ry, rx);
            _parent[ry] = rx;
            if (_rank[rx] == _rank[ry]) _rank[rx]++;
            return true;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PHASE 2 – PRIORITY QUEUE (MAX-HEAP), FIFO QUEUE, LIFO STACK
    // ─────────────────────────────────────────────────────────────────────────

    public class Passenger
    {
        public string Name     { get; }
        public string PNR      { get; }
        public int    Priority { get; } // 3=Platinum, 2=Gold, 1=Economy

        public Passenger(string name, string pnr, int priority)
        {
            if (priority < 1 || priority > 3)
                throw new ArgumentOutOfRangeException(nameof(priority), "Priority must be 1-3.");
            Name = name; PNR = pnr; Priority = priority;
        }
        public override string ToString() => $"{Name} [{PNR}] P{Priority}";
    }

    /// <summary>Max-Heap priority queue for check-in ordering.</summary>
    public class CheckInQueue
    {
        private readonly List<Passenger> _heap = new List<Passenger>();

        public int  Count    => _heap.Count;
        public bool IsEmpty  => _heap.Count == 0;

        public void Insert(Passenger p)
        {
            _heap.Add(p);
            SiftUp(_heap.Count - 1);
        }

        public Passenger ExtractMax()
        {
            if (IsEmpty) throw new InvalidOperationException("Check-in queue is empty.");
            Passenger top = _heap[0];
            int last = _heap.Count - 1;
            _heap[0] = _heap[last];
            _heap.RemoveAt(last);
            if (_heap.Count > 0) SiftDown(0);
            return top;
        }

        public Passenger Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Check-in queue is empty.");
            return _heap[0];
        }

        private void SiftUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (_heap[parent].Priority >= _heap[i].Priority) break;
                (_heap[parent], _heap[i]) = (_heap[i], _heap[parent]);
                i = parent;
            }
        }

        private void SiftDown(int i)
        {
            int n = _heap.Count;
            while (true)
            {
                int largest = i, l = 2 * i + 1, r = 2 * i + 2;
                if (l < n && _heap[l].Priority > _heap[largest].Priority) largest = l;
                if (r < n && _heap[r].Priority > _heap[largest].Priority) largest = r;
                if (largest == i) break;
                (_heap[largest], _heap[i]) = (_heap[i], _heap[largest]);
                i = largest;
            }
        }
    }

    /// <summary>FIFO boarding gate queue (circular array).</summary>
    public class BoardingQueue
    {
        private readonly Passenger[] _buffer;
        private int _front, _rear, _count;

        public BoardingQueue(int capacity = 200)
        {
            _buffer = new Passenger[capacity];
        }

        public bool IsEmpty => _count == 0;
        public bool IsFull  => _count == _buffer.Length;
        public int  Count   => _count;

        public void Enqueue(Passenger p)
        {
            if (IsFull) throw new InvalidOperationException("Boarding gate is at full capacity.");
            _buffer[_rear] = p;
            _rear = (_rear + 1) % _buffer.Length;
            _count++;
        }

        public Passenger Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Boarding gate queue is empty.");
            Passenger p = _buffer[_front];
            _front = (_front + 1) % _buffer.Length;
            _count--;
            return p;
        }

        public Passenger Front()
        {
            if (IsEmpty) throw new InvalidOperationException("Boarding gate queue is empty.");
            return _buffer[_front];
        }
    }

    /// <summary>LIFO cargo hold stack.</summary>
    public class CargoStack
    {
        private readonly Stack<string> _items = new Stack<string>();

        public int  Count   => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        public void Push(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new ArgumentException("Cargo item cannot be empty.");
            _items.Push(item);
        }

        public string Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("Cargo hold is empty.");
            return _items.Pop();
        }

        public string Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Cargo hold is empty.");
            return _items.Peek();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PHASE 3 – HASH TABLE (PNR LOOKUP) + BST (PRICE RANGE QUERIES)
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Hash table with separate chaining for O(1) avg PNR lookup.</summary>
    public class PNRHashTable
    {
        private readonly List<(string Key, Passenger Value)>[] _buckets;
        private readonly int _size;
        private int _count;

        public PNRHashTable(int size = 10007)   // prime reduces clustering
        {
            _size    = size;
            _buckets = new List<(string, Passenger)>[size];
            for (int i = 0; i < size; i++)
                _buckets[i] = new List<(string, Passenger)>();
        }

        public int Count    => _count;
        public double LoadFactor => (double)_count / _size;

        private int Hash(string key)
            => Math.Abs(key.GetHashCode()) % _size;

        public void Insert(string pnr, Passenger profile)
        {
            if (string.IsNullOrWhiteSpace(pnr))
                throw new ArgumentException("PNR cannot be empty.");
            var bucket = _buckets[Hash(pnr)];
            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key == pnr) { bucket[i] = (pnr, profile); return; } // update
            }
            bucket.Add((pnr, profile));
            _count++;
        }

        public Passenger Search(string pnr)
        {
            if (string.IsNullOrWhiteSpace(pnr)) return null;
            foreach (var (k, v) in _buckets[Hash(pnr)])
                if (k == pnr) return v;
            return null;
        }

        public bool Delete(string pnr)
        {
            var bucket = _buckets[Hash(pnr)];
            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key == pnr) { bucket.RemoveAt(i); _count--; return true; }
            }
            return false;
        }
    }

    /// <summary>Binary Search Tree for flight price range queries (O(log n) search).</summary>
    public class PriceBST
    {
        private class Node { public double Price; public Node Left, Right; }
        private Node _root;

        public void Insert(double price)
            => _root = Insert(_root, price);

        private Node Insert(Node node, double price)
        {
            if (node == null) return new Node { Price = price };
            if (price < node.Price)      node.Left  = Insert(node.Left,  price);
            else if (price > node.Price) node.Right = Insert(node.Right, price);
            return node;    // duplicates ignored
        }

        /// <summary>Returns all prices in [low, high] in O(log n + k).</summary>
        public List<double> RangeSearch(double low, double high)
        {
            var results = new List<double>();
            RangeSearch(_root, low, high, results);
            return results;
        }

        private void RangeSearch(Node node, double low, double high, List<double> results)
        {
            if (node == null) return;
            if (node.Price > low)  RangeSearch(node.Left,  low, high, results);
            if (node.Price >= low && node.Price <= high) results.Add(node.Price);
            if (node.Price < high) RangeSearch(node.Right, low, high, results);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PHASE 4 – QUICKSORT, MERGESORT, KMP STRING MATCHING
    // ─────────────────────────────────────────────────────────────────────────

    public static class Sorter
    {
        // ── QuickSort (in-place, median-of-three pivot) ──────────────────────
        public static void QuickSort(double[] arr, int lo = -1, int hi = -1)
        {
            if (lo == -1) lo = 0;
            if (hi == -1) hi = arr.Length - 1;
            if (lo >= hi) return;
            int pivot = Partition(arr, lo, hi);
            QuickSort(arr, lo, pivot - 1);
            QuickSort(arr, pivot + 1, hi);
        }

        private static int Partition(double[] arr, int lo, int hi)
        {
            // Median-of-three pivot to mitigate O(n²) worst case
            int mid = lo + (hi - lo) / 2;
            if (arr[mid] < arr[lo]) (arr[mid], arr[lo]) = (arr[lo], arr[mid]);
            if (arr[hi]  < arr[lo]) (arr[hi],  arr[lo]) = (arr[lo], arr[hi]);
            if (arr[mid] < arr[hi]) (arr[mid], arr[hi]) = (arr[hi], arr[mid]);
            double pivot = arr[hi];
            int i = lo - 1;
            for (int j = lo; j < hi; j++)
                if (arr[j] <= pivot) { i++; (arr[i], arr[j]) = (arr[j], arr[i]); }
            (arr[i + 1], arr[hi]) = (arr[hi], arr[i + 1]);
            return i + 1;
        }

        // ── MergeSort (stable, O(n log n) guaranteed) ───────────────────────
        public static double[] MergeSort(double[] arr)
        {
            if (arr.Length <= 1) return arr;
            int mid = arr.Length / 2;
            double[] left  = MergeSort(arr[..mid]);
            double[] right = MergeSort(arr[mid..]);
            return Merge(left, right);
        }

        private static double[] Merge(double[] left, double[] right)
        {
            double[] result = new double[left.Length + right.Length];
            int i = 0, j = 0, k = 0;
            while (i < left.Length && j < right.Length)
                result[k++] = left[i] <= right[j] ? left[i++] : right[j++];
            while (i < left.Length)  result[k++] = left[i++];
            while (j < right.Length) result[k++] = right[j++];
            return result;
        }
    }

    /// <summary>Knuth-Morris-Pratt string matching for passenger name search in manifests.</summary>
    public static class KMP
    {
        /// <summary>Returns all start indices where pattern occurs in text.</summary>
        public static List<int> Search(string text, string pattern)
        {
            var matches = new List<int>();
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text)) return matches;

            int[] lps = BuildLPS(pattern);
            int i = 0, j = 0;
            while (i < text.Length)
            {
                if (text[i] == pattern[j]) { i++; j++; }
                if (j == pattern.Length) { matches.Add(i - j); j = lps[j - 1]; }
                else if (i < text.Length && text[i] != pattern[j])
                {
                    if (j != 0) j = lps[j - 1];
                    else        i++;
                }
            }
            return matches;
        }

        private static int[] BuildLPS(string pattern)
        {
            int[] lps = new int[pattern.Length];
            int len = 0, i = 1;
            while (i < pattern.Length)
            {
                if (pattern[i] == pattern[len]) { lps[i++] = ++len; }
                else if (len > 0)               { len = lps[len - 1]; }
                else                            { lps[i++] = 0; }
            }
            return lps;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PHASE 5 – BACKTRACKING REROUTING
    // ─────────────────────────────────────────────────────────────────────────

    public class RerouteEngine
    {
        private readonly FlightGraph _graph;
        private readonly int _maxHops;

        public RerouteEngine(FlightGraph graph, int maxHops = 3)
        {
            _graph   = graph;
            _maxHops = maxHops;
        }

        /// <summary>Finds all alternative paths (max _maxHops) excluding blockedAirport.</summary>
        public List<List<string>> FindAlternatives(string origin, string destination, string blockedAirport)
        {
            var results = new List<List<string>>();
            var current = new List<string> { origin };
            var visited = new HashSet<string> { origin, blockedAirport };
            Backtrack(origin, destination, visited, current, results);
            return results;
        }

        private void Backtrack(string current, string destination,
                               HashSet<string> visited, List<string> path,
                               List<List<string>> results)
        {
            if (current == destination) { results.Add(new List<string>(path)); return; }
            if (path.Count - 1 >= _maxHops) return;   // prune: too many hops

            foreach (var (next, _) in _graph.GetNeighbours(current))
            {
                if (visited.Contains(next)) continue;
                visited.Add(next);
                path.Add(next);
                Backtrack(next, destination, visited, path, results);
                path.RemoveAt(path.Count - 1);         // undo choice
                visited.Remove(next);
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // TEST RUNNER
    // ─────────────────────────────────────────────────────────────────────────

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("  SkyNet – DSA Test Suite");
            Console.WriteLine("═══════════════════════════════════════════════\n");

            TestGraph();
            TestHeapAndQueues();
            TestHashTable();
            TestBST();
            TestSorting();
            TestKMP();
            TestBacktracking();

            Console.WriteLine("\n═══════════════════════════════════════════════");
            Console.WriteLine("  All tests completed.");
            Console.WriteLine("═══════════════════════════════════════════════");
        }

        // ── Phase 1: Graph ────────────────────────────────────────────────────
        static void TestGraph()
        {
            Section("PHASE 1 – Graph + Dijkstra's + Kruskal's MST");

            var g = new FlightGraph();
            g.AddRoute("LHR", "DXB", 300);
            g.AddRoute("LHR", "CDG", 500);
            g.AddRoute("DXB", "SYD", 550);
            g.AddRoute("CDG", "SYD", 600);
            g.AddRoute("LHR", "SYD", 1100);

            var (cost, path) = g.Dijkstra("LHR", "SYD");
            Pass("Dijkstra LHR→SYD optimal path",
                 cost == 850 && path.Count == 3,
                 $"Cost={cost}, Path={string.Join("→", path)}");

            var (inf, _) = g.Dijkstra("LHR", "XYZ");
            Pass("Dijkstra unreachable airport returns ∞",
                 double.IsPositiveInfinity(inf), $"Cost={inf}");

            var (same, samePath) = g.Dijkstra("LHR", "LHR");
            Pass("Dijkstra same src/dst returns cost 0",
                 same == 0 && samePath.Count == 1, $"Cost={same}");

            try { g.AddRoute("A", "B", -10); Pass("Negative weight rejected", false, "No exception"); }
            catch (ArgumentException ex) { Pass("Negative weight rejected", true, ex.Message); }

            var mst = g.KruskalMST();
            Pass("Kruskal MST has V-1 edges (4 airports → 3 edges)",
                 mst.Count == 3, $"{mst.Count} edges in MST");
        }

        // ── Phase 2: Heap, Queue, Stack ───────────────────────────────────────
        static void TestHeapAndQueues()
        {
            Section("PHASE 2 – Max-Heap, FIFO Queue, LIFO Stack");

            var checkin = new CheckInQueue();
            checkin.Insert(new Passenger("Alice",   "PNR001", 1)); // Economy
            checkin.Insert(new Passenger("Bob",     "PNR002", 3)); // Platinum
            checkin.Insert(new Passenger("Charlie", "PNR003", 2)); // Gold

            var first = checkin.ExtractMax();
            Pass("Max-Heap: Platinum served first", first.Priority == 3,
                 $"First served: {first}");

            try { new CheckInQueue().ExtractMax(); Pass("ExtractMax on empty throws", false, "No exception"); }
            catch (InvalidOperationException ex) { Pass("ExtractMax on empty throws", true, ex.Message); }

            var boarding = new BoardingQueue(3);
            boarding.Enqueue(new Passenger("Dave",  "PNR010", 1));
            boarding.Enqueue(new Passenger("Eve",   "PNR011", 1));
            boarding.Enqueue(new Passenger("Frank", "PNR012", 1));
            var boarded = boarding.Dequeue();
            Pass("FIFO Queue: Dave boards first (FIFO order)", boarded.Name == "Dave",
                 $"Boarded: {boarded.Name}");

            try { boarding.Enqueue(new Passenger("Extra", "PNR013", 1));
                  Pass("Full boarding queue throws", false, "No exception"); }
            catch (InvalidOperationException ex) { Pass("Full boarding queue throws", true, ex.Message); }

            var cargo = new CargoStack();
            cargo.Push("BAG-001");
            cargo.Push("BAG-002");
            cargo.Push("BAG-003");
            string last = cargo.Pop();
            Pass("Stack: last loaded bag unloaded first (LIFO)", last == "BAG-003",
                 $"Unloaded: {last}");

            try { new CargoStack().Pop(); Pass("Pop on empty stack throws", false, "No exception"); }
            catch (InvalidOperationException ex) { Pass("Pop on empty stack throws", true, ex.Message); }
        }

        // ── Phase 3a: Hash Table ──────────────────────────────────────────────
        static void TestHashTable()
        {
            Section("PHASE 3 – PNR Hash Table");

            var table = new PNRHashTable();
            var p1 = new Passenger("Grace", "ABC123", 2);
            var p2 = new Passenger("Henry", "XYZ789", 1);
            table.Insert("ABC123", p1);
            table.Insert("XYZ789", p2);

            Pass("Search existing PNR returns correct passenger",
                 table.Search("ABC123")?.Name == "Grace", $"Found: {table.Search("ABC123")?.Name}");

            Pass("Search non-existent PNR returns null",
                 table.Search("INVALID") == null, "null returned");

            var updated = new Passenger("Grace Updated", "ABC123", 3);
            table.Insert("ABC123", updated);
            Pass("Duplicate PNR insert updates existing entry",
                 table.Search("ABC123")?.Name == "Grace Updated",
                 $"Name after update: {table.Search("ABC123")?.Name}");

            table.Delete("XYZ789");
            Pass("Delete removes PNR entry",
                 table.Search("XYZ789") == null, "Deleted successfully");

            // Collision test: insert many entries
            for (int i = 0; i < 100; i++)
                table.Insert($"PNR{i:D4}", new Passenger($"P{i}", $"PNR{i:D4}", 1));
            Pass("100 insertions all retrievable",
                 table.Search("PNR0042")?.Name == "P42", $"PNR0042 → {table.Search("PNR0042")?.Name}");

            Console.WriteLine($"   Load factor: {table.LoadFactor:F3}");
        }

        // ── Phase 3b: BST ─────────────────────────────────────────────────────
        static void TestBST()
        {
            Section("PHASE 3 – BST Price Range Query");

            var bst = new PriceBST();
            double[] prices = { 450, 200, 800, 150, 350, 620, 999 };
            foreach (var p in prices) bst.Insert(p);

            var range = bst.RangeSearch(200, 500);
            Pass("BST range [200, 500] returns correct prices",
                 range.Count == 4 && range.Contains(200) && range.Contains(350) && range.Contains(450),
                 $"Found: [{string.Join(", ", range)}]");

            var empty = bst.RangeSearch(1000, 2000);
            Pass("BST range with no matches returns empty list",
                 empty.Count == 0, "Empty list returned");
        }

        // ── Phase 4a: Sorting ─────────────────────────────────────────────────
        static void TestSorting()
        {
            Section("PHASE 4 – QuickSort vs MergeSort");

            double[] departures = { 14.5, 7.0, 22.3, 1.2, 18.0, 9.45, 6.0 };
            double[] qs = (double[])departures.Clone();
            double[] ms = (double[])departures.Clone();

            Sorter.QuickSort(qs);
            ms = Sorter.MergeSort(ms);

            bool qsSorted = IsSorted(qs);
            bool msSorted = IsSorted(ms);
            bool identical = ArraysEqual(qs, ms);

            Pass("QuickSort produces sorted output", qsSorted,
                 $"[{string.Join(", ", qs)}]");
            Pass("MergeSort produces sorted output", msSorted,
                 $"[{string.Join(", ", ms)}]");
            Pass("Both algorithms produce identical results", identical,
                 identical ? "Outputs match" : "MISMATCH");

            // Edge case: already sorted
            double[] sorted = { 1, 2, 3, 4, 5 };
            Sorter.QuickSort(sorted);
            Pass("QuickSort on already-sorted input (median-of-three avoids O(n²))",
                 IsSorted(sorted), $"[{string.Join(", ", sorted)}]");
        }

        static bool IsSorted(double[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
                if (arr[i] < arr[i - 1]) return false;
            return true;
        }

        static bool ArraysEqual(double[] a, double[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;
            return true;
        }

        // ── Phase 4b: KMP ─────────────────────────────────────────────────────
        static void TestKMP()
        {
            Section("PHASE 4 – KMP String Matching");

            string manifest = "John Smith, Jane Doe, John Carter, Mary Johnson";

            var hits = KMP.Search(manifest, "John");
            Pass("KMP finds all 'John' occurrences (3 matches)", hits.Count == 3,
                 $"Positions: [{string.Join(", ", hits)}]");

            var none = KMP.Search(manifest, "XYZ");
            Pass("KMP returns empty list for missing pattern",
                 none.Count == 0, "No matches");

            var empty = KMP.Search("", "John");
            Pass("KMP handles empty text gracefully",
                 empty.Count == 0, "No matches");
        }

        // ── Phase 5: Backtracking ─────────────────────────────────────────────
        static void TestBacktracking()
        {
            Section("PHASE 5 – Backtracking Rerouting");

            var g = new FlightGraph();
            g.AddRoute("LHR", "AMS", 100);
            g.AddRoute("LHR", "FRA", 120);
            g.AddRoute("AMS", "SYD", 700);
            g.AddRoute("FRA", "SYD", 680);
            g.AddRoute("FRA", "AMS", 80);

            var engine = new RerouteEngine(g, maxHops: 3);
            // Block primary hub AMS
            var alternatives = engine.FindAlternatives("LHR", "SYD", blockedAirport: "AMS");

            Pass("Backtracking finds alternative routes when hub is blocked",
                 alternatives.Count > 0,
                 $"{alternatives.Count} alternative(s) found");

            foreach (var path in alternatives)
                Console.WriteLine($"   Alternative: {string.Join(" → ", path)}");

            var noPath = engine.FindAlternatives("LHR", "SYD", blockedAirport: "FRA");
            Pass("Backtracking returns empty when all paths blocked",
                 noPath.Count == 0, $"{noPath.Count} path(s)");
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        static void Section(string title)
        {
            Console.WriteLine($"\n── {title} ──");
        }

        static void Pass(string name, bool condition, string detail)
        {
            string icon = condition ? "✓ PASS" : "✗ FAIL";
            Console.WriteLine($"  [{icon}] {name}");
            Console.WriteLine($"         {detail}");
        }
    }
}

namespace GraphWithList
{
    public class Graph
    {
        private Dictionary<char, List<(char vertex,int weight)>> _dictVertex { get; set; } = [];
        private List<char> _Vertices = [];
        private int _VerticesCount { get; set; }
        public enum enGraphType { enDirected = 1, enUndirected = 2 };
        public enGraphType GraphType { get; set; }
        public Graph(List<char> vertices, enGraphType graphtype = enGraphType.enUndirected)
        {
            GraphType = graphtype;
            _Vertices = vertices;
            _VerticesCount = vertices.Count;
            for (int i = 0; i < vertices.Count; i++)
            {
                _dictVertex[vertices[i]] = new List<(char vertex, int weight)>();
            }
        }


        public void AddEdge(char from, char to, int weight = 1)
        {
            if (!_dictVertex.ContainsKey(from) || !_dictVertex.ContainsKey(to))
            {
                Console.WriteLine($"the vertices {from} & {to} couldnt found in the graph!!");
                return;
            }
            _dictVertex[from].Add((to, weight));
            if (GraphType == enGraphType.enUndirected)
            {
                _dictVertex[to].Add((from, weight));
            }
        }
        public void RemoveEdge(char from, char to)
        {

            if (!_dictVertex.ContainsKey(from) || !_dictVertex.ContainsKey(to))
            {
                Console.WriteLine($"the vertices {from} & {to} did not found in the graph!!");
                return;
            }
            _dictVertex[from].RemoveAll((v) => v.vertex == to);
            if (GraphType == enGraphType.enUndirected)
            {
                _dictVertex[to].RemoveAll((v) => v.vertex == from);
            }
        }
        public int InDegree(char vertex)
        {
            if (!_dictVertex.ContainsKey(vertex)) return -1;
            int degree = 0;
            foreach(char v in _Vertices)
            {
                degree += _dictVertex[v].Count(t => t.vertex == vertex);
            }
            return degree;
        }

        public int OutDegree(char vertex)
        {
            if (!_dictVertex.ContainsKey(vertex)) return -1;
            return _dictVertex[vertex].Count;
        }
        public bool IsThereEdge(char from, char to)
        {
            if (!_dictVertex.ContainsKey(from) || !_dictVertex.ContainsKey(to))
            {
                Console.WriteLine($"the vertices {from} & {to} couldnt found in the graph!!");
                return false;
            }
            return (_dictVertex[from].Exists(t => t.vertex == to));
        }
        public void DisplayMatrix()
        {
            Console.WriteLine();
            foreach(char vertex in _Vertices)
            {
                Console.Write($"{vertex} --> ");
                foreach((char to , int weight) in _dictVertex[vertex])
                    Console.Write($"({to},{weight}), ");
                Console.WriteLine();
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== TEST 1: UNDIRECTED GRAPH ==========");
            List<char> vertices1 = ['A', 'B', 'C', 'D', 'E'];
            // By default, it's Undirected
            Graph undirectedGraph = new Graph(vertices1);

            undirectedGraph.AddEdge('A', 'B');
            undirectedGraph.AddEdge('B', 'D');
            undirectedGraph.AddEdge('E', 'C');
            undirectedGraph.AddEdge('B', 'C');
            undirectedGraph.AddEdge('A', 'D');
            undirectedGraph.AddEdge('A', 'E');

            Console.WriteLine("Initial Undirected Matrix:");
            undirectedGraph.DisplayMatrix();

            Console.WriteLine("\n--- Testing IsThereEdge ---");
            Console.WriteLine($"Edge between A and B? {undirectedGraph.IsThereEdge('A', 'B')}"); // Expected: True
            Console.WriteLine($"Edge between C and A? {undirectedGraph.IsThereEdge('C', 'A')}"); // Expected: False

            Console.WriteLine("\n--- Testing Degrees ---");
            // In an undirected graph, InDegree and OutDegree are always the same!
            Console.WriteLine($"Vertex 'A' -> InDegree: {undirectedGraph.InDegree('A')} | OutDegree: {undirectedGraph.OutDegree('A')}");

            Console.WriteLine("\n--- Testing RemoveEdge ---");
            Console.WriteLine("Removing edge between 'A' and 'B'...");
            undirectedGraph.RemoveEdge('A', 'B');
            Console.WriteLine($"Edge between A and B now? {undirectedGraph.IsThereEdge('A', 'B')}"); // Expected: False
            undirectedGraph.DisplayMatrix();


            Console.WriteLine("\n\n========== TEST 2: DIRECTED GRAPH ==========");
            List<char> vertices2 = ['X', 'Y', 'Z'];
            // Explicitly making it Directed
            Graph directedGraph = new Graph(vertices2, Graph.enGraphType.enDirected);

            directedGraph.AddEdge('X', 'Y'); // X goes to Y
            directedGraph.AddEdge('Y', 'Z'); // Y goes to Z
            directedGraph.AddEdge('X', 'Z'); // X goes to Z

            Console.WriteLine("Directed Matrix (Notice it is NOT symmetrical):");
            directedGraph.DisplayMatrix();

            Console.WriteLine("\n--- Testing Degrees (Directed) ---");
            // X goes to Y and Z (Out = 2). Nobody goes to X (In = 0).
            Console.WriteLine($"Vertex 'X' -> InDegree: {directedGraph.InDegree('X')} | OutDegree: {directedGraph.OutDegree('X')}");

            // Y goes to Z (Out = 1). X goes to Y (In = 1).
            Console.WriteLine($"Vertex 'Y' -> InDegree: {directedGraph.InDegree('Y')} | OutDegree: {directedGraph.OutDegree('Y')}");

            // Z goes nowhere (Out = 0). X and Y go to Z (In = 2).
            Console.WriteLine($"Vertex 'Z' -> InDegree: {directedGraph.InDegree('Z')} | OutDegree: {directedGraph.OutDegree('Z')}");


            Console.WriteLine("\n\n========== TEST 3: ERROR HANDLING ==========");
            Console.WriteLine("Trying to add an edge to a non-existent vertex 'W':");
            undirectedGraph.AddEdge('A', 'W'); // Should trigger your error message

            Console.WriteLine($"\nTrying to check InDegree of a non-existent vertex 'W': {undirectedGraph.InDegree('W')}");

            Console.WriteLine("\n\n========== TEST 3: Weighted Graph ==========");
            undirectedGraph = new Graph(vertices1, Graph.enGraphType.enUndirected);
            undirectedGraph.AddEdge('A', 'B', 20);
            undirectedGraph.AddEdge('B', 'D', 10);
            undirectedGraph.AddEdge('E', 'C', 4);
            undirectedGraph.AddEdge('B', 'C', 12);
            undirectedGraph.AddEdge('A', 'D', 17);
            undirectedGraph.AddEdge('A', 'E', 28);

            Console.WriteLine("Initial Undirected Matrix:");
            undirectedGraph.DisplayMatrix();

            Console.WriteLine("\n--- Testing IsThereEdge ---");
            Console.WriteLine($"Edge between A and B? {undirectedGraph.IsThereEdge('A', 'B')}"); // Expected: True
            Console.WriteLine($"Edge between C and A? {undirectedGraph.IsThereEdge('C', 'A')}"); // Expected: False

            Console.WriteLine("\n--- Testing Degrees ---");
            // In an undirected graph, InDegree and OutDegree are always the same!
            Console.WriteLine($"Vertex 'A' -> InDegree: {undirectedGraph.InDegree('A')} | OutDegree: {undirectedGraph.OutDegree('A')}");

            Console.WriteLine("\n--- Testing RemoveEdge ---");
            Console.WriteLine("Removing edge between 'A' and 'B'...");
            undirectedGraph.RemoveEdge('A', 'B');
            Console.WriteLine($"Edge between A and B now? {undirectedGraph.IsThereEdge('A', 'B')}"); // Expected: False
            undirectedGraph.DisplayMatrix();
        }
    }
}

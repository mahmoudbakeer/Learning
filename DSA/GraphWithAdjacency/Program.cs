namespace GraphWithAdjacency
{
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

            Console.WriteLine("\nTrying to check InDegree of a non-existent vertex 'W':");
            int degree = undirectedGraph.InDegree('W'); // Should trigger your error message and return -1

            Console.WriteLine("\n\n========== TEST 3: Weighted Graph ==========");
            undirectedGraph.AddEdge('A', 'B',20);
            undirectedGraph.AddEdge('B', 'D',10);
            undirectedGraph.AddEdge('E', 'C',4);
            undirectedGraph.AddEdge('B', 'C',12);
            undirectedGraph.AddEdge('A', 'D',17);
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

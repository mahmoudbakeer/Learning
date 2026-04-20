namespace GraphWithAdjacency
{
    public class Graph
    {
        private Dictionary<char, int> _dictVertex { get; set; } = [];
        private List<char> _Vertices = [];
        private int[,] AdjMatrix { get; set; }
        private  int _VerticesCount { get; set; }
        public enum enGraphType { enDirected = 1 , enUndirected = 2};
        public enGraphType GraphType { get; set; }
        public Graph(List<char> vertices , enGraphType graphtype = enGraphType.enUndirected)
        {
            GraphType = graphtype;
            _Vertices = vertices;
            _VerticesCount = vertices.Count;
            AdjMatrix = new int[vertices.Count, vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                _dictVertex[vertices[i]] = i;
            }
        }


        public void AddEdge(char from, char to, int weight = 1)
        {
            if (!_dictVertex.ContainsKey(from) || !_dictVertex.ContainsKey(to))
            {
                Console.WriteLine($"the vertices {from} & {to} couldnt found in the graph!!");
                return;
            }
            int row = _dictVertex[from];
            int column = _dictVertex[to];
            AdjMatrix[row, column] = weight;
            if (GraphType == enGraphType.enUndirected)
            {
                AdjMatrix[column,row] = weight;
            }
        }
        public void RemoveEdge(char from , char to)
        {

            if (!_dictVertex.ContainsKey(from) || !_dictVertex.ContainsKey(to))
            {
                Console.WriteLine($"the vertices {from} & {to} did not found in the graph!!");
                return;
            }
            int row = _dictVertex[from];
            int column = _dictVertex[to];
            AdjMatrix[row, column] = 0;
            if (GraphType == enGraphType.enUndirected)
            {
                AdjMatrix[column, row] = 0;
            }
        }
        public int InDegree(char vertex)
        {
            if (!_dictVertex.ContainsKey(vertex)) return -1;

            int col = _dictVertex[vertex];

            // LINQ: Generate numbers from 0 to Count, and count how many have a value > 0
            return Enumerable.Range(0, _VerticesCount).Count(row => AdjMatrix[row, col] > 0);
        }

        public int OutDegree(char vertex)
        {
            if (!_dictVertex.ContainsKey(vertex)) return -1;

            int row = _dictVertex[vertex];

            // LINQ: Check the row and iterate through all the columns
            return Enumerable.Range(0, _VerticesCount).Count(col => AdjMatrix[row, col] > 0);
        }
        public bool IsThereEdge(char from , char to)
        {
            if (!_dictVertex.ContainsKey(from) || !_dictVertex.ContainsKey(to))
            {
                Console.WriteLine($"the vertices {from} & {to} couldnt found in the graph!!");
                return false;
            }
            int row = _dictVertex[from];
            int column = _dictVertex[to];
            if (AdjMatrix[row,column] > 0) return true;
            else return false;
        }
        public void DisplayMatrix()
        {
            Console.WriteLine();
            Console.WriteLine($"  {string.Join(" ", _Vertices)}"); // Top header

            foreach (char vertex in _Vertices)
            {
                int row = _dictVertex[vertex];

                // LINQ: Select all values in this specific row, then join them with a space
                var rowValues = Enumerable.Range(0, _VerticesCount)
                                          .Select(col => AdjMatrix[row, col]);

                Console.WriteLine($"{vertex} {string.Join(" ", rowValues)}");
            }
        }
    }
}

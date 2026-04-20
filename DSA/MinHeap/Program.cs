
namespace MinHeap
{
    public class MinHeap
    {
        public List<int> heap = new List<int>();

        public void insert(int number)
        {
            heap.Add(number);

            // heapifyUp
            HeapifyUp(heap.Count - 1);
        }
        public int Peek()
        {
            if (heap.Count == 0)
                throw new ArgumentNullException();
            return heap[0];
        }
        public int ExtractMin()
        {
            if (heap.Count == 0) throw new InvalidOperationException("The heap is empty!");

            int min = heap[0];

            heap[0] = heap[heap.Count - 1];

            heap.RemoveAt(heap.Count - 1);

            if (heap.Count > 0)
            {
                HeapifyDown(0);
            }

            return min;
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;

            while (true)
            {
                int leftChild = (2 * index) + 1;
                int rightChild = (2 * index) + 2;

                int smallest = index;

                if (leftChild <= lastIndex && heap[leftChild] < heap[smallest])
                {
                    smallest = leftChild;
                }

                if (rightChild <= lastIndex && heap[rightChild] < heap[smallest])
                {
                    smallest = rightChild;
                }

                if (smallest == index) break;

                (heap[index], heap[smallest]) = (heap[smallest], heap[index]);

                index = smallest;
            }
        }

        private void HeapifyUp(int index)
        {
            while(index > 0)
            {
                int parentind = (index - 1) / 2;

                if (heap[parentind] <= heap[index]) break;

                (heap[parentind], heap[index]) = (heap[index], heap[parentind]);

                index = parentind;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            MinHeap mh = new MinHeap();

            mh.insert(6);
            mh.insert(2);
            mh.insert(1);
            mh.insert(3);
            // print the heap
            Console.WriteLine($"The heap is now : {string.Join(", ",mh.heap)}");
        }
    }
}

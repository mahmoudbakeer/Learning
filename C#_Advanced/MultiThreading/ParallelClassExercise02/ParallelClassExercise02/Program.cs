namespace ParallelClassExercise02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int numberofiterations = 10;
            Parallel.For(0, numberofiterations, printThreadResults);
        }
        public static void printThreadResults(int iteration)
        {
            Console.WriteLine($"The task iteration is {iteration} and task id is {Task.CurrentId}");
        }
    }
}

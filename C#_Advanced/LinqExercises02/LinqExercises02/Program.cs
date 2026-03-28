using System.Diagnostics.Metrics;

namespace LinqExercises02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // the where clause in c# is the same as sql where 

            List<string> Fruits = new List<string> { "apple", "passionfruit", "banana", "mango",
                    "orange", "blueberry", "grape", "strawberry" };


            IEnumerable<string> resFruits = Fruits.Where(F => F.Length < 6);

            Console.WriteLine("The results of string has less than 6 characters of where query is : ");
            foreach (var item in resFruits)
            {
                Console.WriteLine(item);
            }


            // if you want to filter on indexes and values also there is where overloaded method 
            int[] numbers = { 0, 30, 20, 15, 90, 85, 40, 75 };

            IEnumerable<int> query =
                numbers.Where((number, index) => number <= index * 10);
            // use it when you want to filter on indexes
            foreach (int number in query)
            {
                Console.WriteLine(number);
            }



            // get the first element in the dataSource
            var first = numbers.First();
            Console.WriteLine($"The first element of numbers is {first}");


            var firstwithcondition = numbers.First(x => x > 80);
            Console.WriteLine($"The first element satisfies the condition of numbers is {firstwithcondition}");

            /*
             This code produces the following output:
             0
             20
             15
             40
            */

            var emptyList = new List<int>();

            // =========================================================
            // 1) FirstOrDefault()
            // Returns the first element, or default(T) if the collection is empty
            // =========================================================
            var result1 = numbers.FirstOrDefault();
            Console.WriteLine(result1); // 10

            var result1_empty = emptyList.FirstOrDefault();
            Console.WriteLine(result1_empty); // 0 (default for int)


            // =========================================================
            // 2) FirstOrDefault(predicate)
            // Returns the first element that matches the condition,
            // or default(T) if no match is found
            // =========================================================
            var result2 = numbers.FirstOrDefault(x => x > 25);
            Console.WriteLine(result2); // 30

            var result2_notFound = numbers.FirstOrDefault(x => x > 100);
            Console.WriteLine(result2_notFound); // 0


            // =========================================================
            // 3) FirstOrDefault(defaultValue)
            // Returns the first element, or a custom default value
            // if the collection is empty (C# 9 / .NET 6+)
            // =========================================================
            var result3 = emptyList.FirstOrDefault(-1);
            Console.WriteLine(result3); // -1


            // =========================================================
            // 4) FirstOrDefault(predicate, defaultValue)
            // Returns the first element that matches the condition,
            // or a custom default value if no match is found
            // =========================================================
            var result4 = numbers.FirstOrDefault(x => x > 100, -1);
            Console.WriteLine(result4); // -1


            // =========================================================
            // Example with objects
            // =========================================================
            //    var students = new List<Student>
            //{
            //    new Student { Name = "Ali", Grade = 80 },
            //    new Student { Name = "Omar", Grade = 60 }
            //};

            //    // Returns first student with Grade > 90, or null if none exists
            //    var studentResult = students.FirstOrDefault(s => s.Grade > 90);

            //    if (studentResult == null)
            //        Console.WriteLine("No student found");
        }
    }
}

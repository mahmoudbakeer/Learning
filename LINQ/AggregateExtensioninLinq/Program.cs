namespace AggregateExtensioninLinq
{
    internal class Program
    {
        static void Main(string[] args)
        {





            // we use .Aggregate extension to implement a customized aggregations for or resultint[int[
            //int[] numbers = [1, 2, 3, 4, 5];



            // implement an aggregate function for summation

            //int sum = numbers.Aggregate((acc, next) =>
            //{

            //    Console.WriteLine($"accumolator : {acc}  , next : {next}");
            //    return acc + next;
            //}
            //);
            //Console.WriteLine($"The sum of numbers is : {sum}");


            // even though this function demonstrate the work of the .Aggregate ext b
            // we might want to implement our own aggregation like numbers of even in array


            // Aggregate(initialValue , lambda expression , the final result if you want to change it when the function return it)
            // if you did not specify a initial value (seed) the first element will be the initial value and that is wrong in our example the initial value must be zero
            //int Evens = numbers.Aggregate(0 , (acc , next) =>
            //{
            //    Console.WriteLine($"accumolator : {acc}  , next : {next}");
            //    return next % 2 == 1 ? acc : ++acc;
            //}, Value => Value *2 // this is how the final result will look like 
            //);
            //Console.WriteLine($"The number of even numbers is : {Evens}");



            List<int> numbers = [35, 44,-1, 84, 3987, -4, -199, 329, 446, 208];


            // group by returns the values grouped for key 
            // query contains IEnumerable<IGrouping<key ,group>> 

            // LINQ GroupBy differs from SQL GROUP BY:
            // In SQL, selected columns must be grouped or aggregated,
            // while in LINQ, GroupBy returns groups (IGrouping<TKey, TElement>)
            // and allows access to all elements without requiring aggregation.


            // you should assigne variable name for the keys is they are more than one
            //var query = numbers
            //    .GroupBy(number =>   new {reminder =  number % 2,ispositive =  number > 0 });

            //Console.WriteLine($"The final data : ");
            //foreach (var g in query)
            //{
            //    Console.WriteLine("{0}", g.Key.reminder == 0 ? "Even Numbers" : "Odd Numbers");
            //    Console.WriteLine("{0}", !g.Key.ispositive ? "Negative Numbers" : "Positive Numbers");
            //    foreach (var item in g)
            //    {
            //        Console.WriteLine(item);
            //    }
            //}

        }
    }
}

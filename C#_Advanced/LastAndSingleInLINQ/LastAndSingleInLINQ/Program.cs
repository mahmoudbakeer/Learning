namespace LastAndSingleInLINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // Last → returns the last element (or last match), throws InvalidOperationException if sequence is empty or no match found

            // LastOrDefault → same as Last, but returns default(T) if no element found
            // NOTE: default(string) is null, not string.Empty
            // In .NET 6+, you can specify a custom default value

            // Single → expects exactly one element
            // throws if:
            // - no element found
            // - more than one element found

            // SingleOrDefault → returns default(T) if no match
            // BUT still throws if more than one element exists
            List<int> numbers = new List<int>{1 , 2, 3, 4, 5};
            //numbers = []
            // Last extension will return the last element in the given sequence or exception if the sequence is empty or no elements matches the creteria
            //var t = numbers.Last(x => x > 4);
            // it will return the Last element same as Last extension but if no element found it will return the default value accordingly , e.g 0 for int , string.Empty for string etc... 
            // we can specifiy the required default value LastOrDefault should return in case no matching elements found here -1
            //var t = numbers.LastOrDefault(x => x>6 , -1);

            // Single extension will return a single element or exception if (No element found , The sequence contain more than one element)
            //var t = numbers.Single(x => x == 5);
            // same as Single ext but will return a default value if No element matches or exception if the sequence contain more than one element
            //var t = numbers.SingleOrDefault(x => x == 6 , -1);
            //var t = numbers.SingleOrDefault(-1);
            //Console.WriteLine($"The result is : {t}");
        }
    }
}

namespace QueryLanguage
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // the LINQ stands for Language integrated query 
            // LINQ used for doing the query syntax

            /*
             * Language-Integrated Query (LINQ) is the name for a set of technologies
             * based on the integration of query capabilities directly into the C# language.
             * Traditionally, queries against data are expressed as simple strings without type checking at compile time
             * or IntelliSense support. Furthermore,
             * you have to learn a different query language for each type of data source:
             * SQL databases, XML documents, various Web services, and so on.
             * With LINQ, a query is a first-class language construct, 
             * just like classes, methods, and events.
             * When you write queries, the most visible "language-integrated" part of LINQ is the query expression.
             * You write query expressions in a declarative query syntax.
             * By using query syntax, you perform filtering, ordering,
             * and grouping operations on data sources with a minimum of code.
             * You use the same query expression patterns to query and transform data from any type of data source.

             * The following example shows a complete query operation. The complete operation includes creating a data source, defining the query expression, and executing the query in a foreach statement.
             */
            // Define the query expression.
            // from first then where then selection from source
            /*All LINQ query operations consist of three distinct actions:
               1.Obtain the data source.
               2.Create the query.
               3.Execute the query.
            */
            // Specify the data source.
            int[] scores = [97, 92, 81, 60];
            IEnumerable<int> scoreQuery =
                                                from score in scores
                                                where score > 80
                                                select score;
                                
            // Execute the query.
            foreach (var i in scoreQuery)
            {
                Console.Write(i + " ");
            }

            // Output: 97 92 81


            IEnumerable<int> highScoresQuery =
                                                from score in scores
                                                where score > 80
                                                orderby score descending
                                                select score;



            IEnumerable<string> highScoresQuery2 =
                                                from score in scores
                                                where score > 80
                                                orderby score descending
                                                select $"The score is {score}"; // it will yeild a string each filteration

            Console.WriteLine("\nThe selection as a string : ");
            foreach (var i in highScoresQuery2)
            {
                Console.WriteLine(i);
            }

            /*
             * var highScoreCount = (
             * from score in scores
             * where score > 80
             * select score).Count();
             */
            var scoreCount = scoreQuery.Count();
            Console.WriteLine($"The total scores are : {scoreCount}");

        }
    }
}

namespace GroupByExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
                
            var Cars = Car.GetCars();
            // this will return the IEnumerable<IGrouping<string, Car>>  
            //var res = Cars.GroupBy(c => c.Brand);

            //foreach (var group in res)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine();
            //    Console.WriteLine($"the Brand is : {group.Key}");
            //    Car.Print(group);
            //}

            // the difference between the ToLookUp and the GroupBy is 
            // GrouBy is deffered excution extension , ToLookUp is imediate
            // the second thing that groupby applies the gouping on database and return the values grouped
            // but the ToLookUp return all the values then group them
            // that only when we are retrieving the data from the database
            //var res2 = Cars.ToLookup(c=> c.Brand);
            //foreach (var group in res2)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine();
            //    Console.WriteLine($"the Brand is : {group.Key}");
            //    Car.Print(group);
            //}


            // Now with Chunk() extension 
            //var res3 = Cars.Chunk(size: 20); // this will partition the cars into groups of 20 sizes each
            //foreach (var group in res3)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine();
            //    Car.Print(group);
            //}
            // now with the Take , TakeLast , TakeWhile
            //var FirstFewCars = Cars.Take(count: 20); // the difference is that this will take the first 20 cars in the sequence
            //var LastFewCars = Cars.TakeLast(count: 20); // this will take last 20 cars in the sequence 
            //var TakeUntilNot = Cars.TakeWhile(car => car.Price  < 60000); // this will keep taking from the sequence until the predicate returns false
            //Console.WriteLine();
            //Console.WriteLine();
            //Car.Print(FirstFewCars);
            //Console.WriteLine();
            //Console.WriteLine();
            //Car.Print(LastFewCars);

            //Car.Print(TakeUntilNot);
            // now with the Skip and SkipLast and SkipWhile 
            // Skip it will bypass the elements until its predicates return true
            // means it will return the elements while the predicates condition false
            // skip last same thing but from the last
            // SkipWhile it will return the values while its condition predicate return false
            // on the first true hits it will stop

            //var res = Cars.Skip(count : 20);
            //Console.WriteLine();
            //Console.WriteLine();
            // Car.Print(res);
            // usually the take and skip works togather to give required results
            // like one examples is when we want to print some pages on each page twenty element
            int PageSize = 10;
            int size = (int)Math.Ceiling(Cars.Count() / (double)PageSize);
            for (int i = 0; i < size; i++)
            {
                var res = Cars
                    .Skip(i * PageSize)
                    .Take(PageSize);
                Car.Print(res);
            }    
        }
    }
}

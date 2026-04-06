namespace LINQQueries
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // OrderBy : Sorts the elements of a sequence in ascending order according to a key
            // OrderByDescending : Sorts the elements of a sequence in ascending order by using a specified comparer.

            List<Car> cars = Car.GetCars();
            // var res = cars.OrderBy(c => c.Year);
            //var res = cars.OrderByDescending(c => c.Price);


            //var res = cars
            //    .OrderBy(c => c.Brand)
            //    .ThenBy(c => c.Price)
            //    .ThenByDescending(c => c.Year);


            // .Order() sorts a collection whose elements are directly comparable (e.g., int, string).
            // It works only when the type implements IComparable or a default comparer exists.
            // For complex objects (like Car), use OrderBy() and specify a key to sort by.

            // .OrderDescending() does the exact opposite of .Order()




            // .Any() returns true if there is at least one element in the sequence 
            // .Any(Predicat) return true if found at least one element meets the condition of the predicate


            // .All(Predicate) only one overload and returns true if all the elements meets the Predicate condition
            // means it will loop on the data source and return false on the first element that does not satisfy the Predicate condition
            // if all the elements meets the Predicate condition then it will return true



            // .Append(element) append { add at the end } the element at the end of the collection
            // .Prepend(element) add the element at the begaining of the collection 





            // .Count() returns the number of elements / record in the collection
            // .Count(Predicate) returns the number of elements / record in the collection that satisfy the Predicate condition
            // it will return exception if the collection is empty or it is number of records larger than int32 which is 2,14 billion
            // .LongCount() and .LongCount(Predicate) is same as .Count() and .Count(Predicate) but the number of possible records is long , i think it is 10^9
            // var res = cars.CountBy( c => c.Make); // will return keyValuePair<key , value> count using grouping




            // Sum(x => x.RequiredSumedColumn) Computes the sum of a sequence of any sumable type values.


            //long[] numbers = new long[] { 1, 2, 3, 4, 5, 20000000000 };
            //// 
            //var res = numbers.Sum();
            //Console.WriteLine($"The sum of numbers is : {res}");



            //var res = cars.Sum(car => car.Price);
            //Console.WriteLine($"The sum of Pricese is : {res}");



            // use int.Parse("string") when you want to convert string into other type 
            // use convert.ToType() when you want to convert anyType to other type 

            // var res = cars.Average(c => c.Year);
            //Console.WriteLine($"The Avg of Years is : {Math.Floor(res)}");


            // .Max() extension returns the greatest value in the given sequence 
            // .Max(x => x.ColumnName) returns the greates value int the column



            // .MaxBy() return the TSource where for a given selector has the maximun value
            // 
            // var res = cars.MaxBy(x => x.Price);


            // samething but the opposite for .Min() and .MinBy()

            //string str = "the dog under the water";
            //List<string> sList = str.Split(' ').ToList(); // split return type [] not list
            //string res = sList.Aggregate((acc, next) =>

            //{
            //    return next + acc;
            //}
            //);


            //Console.WriteLine(res); 
            //Car.Print([res]);


            // using select we can transform any type of objects into another type 

            //var res = cars
            //    .Select(c => 
            //    new CarDto(c.Id,c.Brand,c.Year,c.Price)
            //    );
            //foreach(var c in res)
            //{
            //    Console.WriteLine($"{c.Id} - {c.Brand} - {c.Year}  - {c.Price}");
            //}
            // we can also take the index from the second overload of the .Select() extenstion in this way 
            // the index start from zero same as array and assign value for each record retrieved and increamented by one

            //var res = cars
            //    .Select((c , i)=>
            //    new CarDto(c.Id, c.Brand, c.Year, c.Price , i)
            //    );
            //foreach (var c in res)
            //{
            //    Console.WriteLine($"{c.Id} - {c.Brand} - {c.Year}  - {c.Price} - {c.index}");
            //}

            PetOwner[] petOwners =
        { 
          new PetOwner { Name="Higa",
              Pets = new List<string>{ "Scruffy", "Sam" } },
          new PetOwner { Name="Ashkenazi",
              Pets = new List<string>{ "Walker", "Sugar" } },
          new PetOwner { Name="Price",
              Pets = new List<string>{ "Scratches", "Diesel" } },
          new PetOwner { Name="Hines",
              Pets = new List<string>{ "Dusty" } } };

            // Project the pet owner's name and the pet's name.
            var query =
                petOwners
                .SelectMany(petOwner => petOwner.Pets, (petOwner, petName) => new { petOwner, petName });

           var nquery = query
                .Select(ownerAndPet =>
                        new
                        {
                            Owner = ownerAndPet.petOwner.Name,
                            Pet = ownerAndPet.petName
                        }
                );

            // Print the results.
            foreach (var obj in query)
            {
                Console.WriteLine(obj);
            }
        }
        class PetOwner
        {
            public string Name { get; set; }
            public List<string> Pets { get; set; }
        }


    }
}

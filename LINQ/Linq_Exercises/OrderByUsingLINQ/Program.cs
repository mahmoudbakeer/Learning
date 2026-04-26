namespace LINQQueries
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // OrderBy : Sorts the elements of a sequence in ascending order according to a key
            // OrderByDescending : Sorts the elements of a sequence in descending order according to a key

            List<Car> cars = Car.GetCars();

            // var res = cars.OrderBy(c => c.Year);
            // var res = cars.OrderByDescending(c => c.Price);

            // var res = cars
            //     .OrderBy(c => c.Brand)
            //     .ThenBy(c => c.Price)
            //     .ThenByDescending(c => c.Year);


            // .Order() sorts elements directly if the type implements IComparable (e.g., int, string)
            // For complex objects (like Car), use OrderBy() with a key selector
            // .OrderDescending() is the reverse of Order()


            // .Any() returns true if there is at least one element
            // .Any(predicate) returns true if at least one element satisfies the condition

            // .All(predicate) returns true if ALL elements satisfy the condition
            // It stops and returns false at the first element that does NOT satisfy the condition


            // .Append(element) adds an element at the end (returns new sequence, does NOT modify original)
            // .Prepend(element) adds an element at the beginning (returns new sequence)


            // .Count() returns the number of elements (returns 0 if empty)
            // .Count(predicate) returns number of elements that satisfy the condition
            // .LongCount() same as Count but supports very large numbers (long)


            // Example:
            // long[] numbers = new long[] { 1, 2, 3, 4, 5, 20000000000 };
            // var res = numbers.Sum();
            // Console.WriteLine($"The sum of numbers is : {res}");

            // var res = cars.Sum(car => car.Price);
            // Console.WriteLine($"The sum of Prices is : {res}");


            // var res = cars.Average(c => c.Year);
            // Console.WriteLine($"The Avg of Years is : {Math.Floor(res)}");


            // .Max() returns the greatest value in the sequence
            // .Max(x => x.Property) returns the greatest value of that property

            // .MaxBy() returns the object that has the maximum value
            // var res = cars.MaxBy(x => x.Price);

            // Same idea for .Min() and .MinBy()


            // string str = "the dog under the water";
            // List<string> sList = str.Split(' ').ToList();

            // string res = sList.Aggregate((acc, next) =>
            // {
            //     return next + acc;
            // });

            // Console.WriteLine(res);


            // Select: transforms each element into another form

            // var res = cars
            //     .Select(c => new CarDto(c.Id, c.Brand, c.Year, c.Price));

            // foreach (var c in res)
            // {
            //     Console.WriteLine($"{c.Id} - {c.Brand} - {c.Year} - {c.Price}");
            // }


            // Select with index

            // var res = cars
            //     .Select((c, i) =>
            //         new CarDto(c.Id, c.Brand, c.Year, c.Price, i)
            //     );


            // SelectMany: flattens collections

            // PetOwner[] petOwners =
            // {
            //     new PetOwner { Name="Higa", Pets = new List<string>{ "Scruffy", "Sam" } },
            //     new PetOwner { Name="Ashkenazi", Pets = new List<string>{ "Walker", "Sugar" } },
            //     new PetOwner { Name="Price", Pets = new List<string>{ "Scratches", "Diesel" } },
            //     new PetOwner { Name="Hines", Pets = new List<string>{ "Dusty" } }
            // };

            // var query = petOwners
            //     .SelectMany(petOwner => petOwner.Pets,
            //         (petOwner, petName) => new { petOwner, petName });

            // var nquery = query.Select(x => new
            // {
            //     Owner = x.petOwner.Name,
            //     Pet = x.petName
            // });

            // foreach (var obj in nquery)
            // {
            //     Console.WriteLine($"{obj.Owner} - {obj.Pet}");
            // }


            // Empty & DefaultIfEmpty

            // var emptyList = Enumerable.Empty<Car>();
            // Console.WriteLine(emptyList);

            // IEnumerable<Car> emptyCars = Enumerable.Empty<Car>();

            // var defaultResult = emptyCars.DefaultIfEmpty(
            //     new Car
            //     {
            //         Id = 1001,
            //         Brand = "BMW",
            //         Color = "Red",
            //         Model = "IDK",
            //         Price = 30000,
            //         Year = 2022
            //     });


            // more examples on selectMany

            List<Car> EditedCars = [
                new Car { Id = 1, Brand = "BMW", Model = "330i", Color = "Black", Price = 45000, Year = 2023, Safty = ["ABS", "Airbags", "Lane Departure Warning", "Active Blind Spot Detection"] },
                new Car { Id = 2, Brand = "Toyota", Model = "Camry", Color = "Super White", Price = 26500, Year = 2022, Safty = ["Pre-Collision System", "Lane Tracing Assist", "Airbags", "Radar Cruise Control"] },
                new Car { Id = 3, Brand = "Tesla", Model = "Model 3", Color = "Ultra Red", Price = 40000, Year = 2024, Safty = ["Autopilot", "Forward Collision Warning", "Automatic Emergency Braking", "Sentry Mode"] },
                new Car { Id = 4, Brand = "Ford", Model = "Mustang GT", Color = "Grabber Blue", Price = 43000, Year = 2021, Safty = ["ABS", "Traction Control", "Rearview Camera", "AdvanceTrac"] },
                new Car { Id = 5, Brand = "Honda", Model = "CR-V", Color = "Lunar Silver", Price = 31000, Year = 2023, Safty = ["Collision Mitigation", "Road Departure Mitigation", "Adaptive Cruise Control"] },
                new Car { Id = 6, Brand = "Audi", Model = "Q5", Color = "Daytona Grey", Price = 55000, Year = 2023, Safty = ["Audi Pre Sense", "Parking System Plus", "Blind Spot Warning", "Rear Cross Traffic Assist"] },
                new Car { Id = 7, Brand = "Mercedes-Benz", Model = "C-Class", Color = "Obsidian Black", Price = 48000, Year = 2022, Safty = ["Active Brake Assist", "Attention Assist", "Cross-Wind Assist", "Airbags"] },
                new Car { Id = 8, Brand = "Chevrolet", Model = "Silverado 1500", Color = "Summit White", Price = 42000, Year = 2024, Safty = ["Forward Collision Alert", "Front Pedestrian Braking", "Lane Keep Assist"] },
                new Car { Id = 9, Brand = "Porsche", Model = "911 Carrera", Color = "Guards Red", Price = 114000, Year = 2023, Safty = ["Warn and Brake Assist", "ParkAssist", "Wet Mode", "Traction Management"] },
                new Car { Id = 10, Brand = "Hyundai", Model = "Tucson", Color = "Amazon Grey", Price = 28500, Year = 2024, Safty = ["Safe Exit Warning", "Driver Attention Warning", "Rear Occupant Alert", "Lane Keeping Assist"] }
            ];




            // The first overload 
            var AllSafty = EditedCars.SelectMany(EC => EC.Safty);
            Console.WriteLine("The First OverLoad of the Select Many : 'Flatten the result'");
            foreach (var s in AllSafty)
                Console.WriteLine($"{s}");

            // the second overload
            var OneSafty = EditedCars.SelectMany(c => c.Safty , (c , s) => string.Join(',' , s));
            Console.WriteLine("The second overload of the select Many : 'Group the safty int one line'");
            foreach (var s in OneSafty)
                Console.WriteLine($"{s}");


        }
    }

    // class PetOwner
    // {
    //     public string Name { get; set; }
    //     public List<string> Pets { get; set; }
    // }
}



//List<Employee> employees = [
//    new("Ali" , "HR" , 45_000),
//    new("Samer" , "Technology" , 50_000),
//    new("Hamed" , "Sales" , 75_000)
//    ];

// we can get the result using the group by just same as sql
//var res = employees.
//    GroupBy(e => e.Department,
//    (key, group) =>
//    new
//    {
//        key = key,
//        TotalSalaries = group.Sum(e => e.Salary)
//    }
//    );
//var res = employees.AggregateBy(e => e.Department,
//    seed: 0.0, (total, eNext) => total + eNext.Salary);

//foreach (var item in res)
//{
//    Console.WriteLine($"The Department is {item.key} and the Total Salaries is {item.TotalSalaries}");
//}



// but we can do better and easier using AggregateBy() in .Net 9.0


//var res = employees.AggregateBy(e => e.Department,
//    seed: 0.0, 
//    (total, employee) => total + employee.Salary ////Aggregation result means KeyValuePair.Value
//    );

//foreach (var item in res)
//{
//    Console.WriteLine($"The Department is {item.Key} and the Total Salaries is {item.Value}");
//}





// Now what we want to do is , Getting the list of Models of cars per Brand

//List<Car> cars = Car.GetCars();

//var res = cars.AggregateBy(c => c.Brand,
//    seed: new List<string>(),
//    (models, car) => [.. models , car.Model]);



//foreach(var item in res)
//{
//    Console.WriteLine($"Brand is {item.Key}");
//    foreach(string model in item.Value.Distinct())
//    {
//        Console.WriteLine($"\t\t{model}");
//    }
//}




// we want to use AggregateBy() to return the maximum price or minumum price in group of cars based on Brand

//List<Car> cars = Car.GetCars();


//var res = cars.AggregateBy(c => c.Brand,
//    seed: decimal.MinValue,
//    (currentMax, currentCar) => Math.Max(currentMax, currentCar.Price));


//foreach (var item in res)
//    Console.WriteLine($"The Brand is {item.Key} & The Maximum Price is {item.Value}");

// right now we did it , but there is much easier way to do it using group by 
// its not about how to do it , its just about how will you understand each one of them 'extensions' to know when to use each of them


//List<Car> cars = Car.GetCars();

//var res = cars.GroupBy(c => c.Brand,
//    (Brand, groupOfCars) => new
//    {
//        Brand,
//        MinPrice = groupOfCars.Select(c => c.Price).Min(),
//        MaxPrice = groupOfCars.Select(c => c.Price).Max()
//    }
//    );



//foreach(var item in res)
//    Console.WriteLine($"The Brand is {item.Brand} \n\tThe Highest Price is {item.MaxPrice} & The lowest Price is {item.MinPrice}");

// you can see the difference and group by is much easier to understand but its not efficient if there is only one operatoin to be done ,
// like max , min , sum , using the AggregateBy() is better here in performance 
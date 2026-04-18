
// now with the linq joins
// the same thing applies on the joins in sql applies here 
// the difference is to know how to use the extension method 

(int Id, string Name)[] Makes =
{
    (1, "UnderTheWater"),
    (2, "AboveTheWater"),
    (3, "SameAsWater")
};

(int Id, string Name)[] Models =
{
    (1, "BMW"),
    (2, "Marcides"),
    (3, "Toyota")
};

(int Id, int MakeId, int ModelId, string VIN)[] Cars =
{
    (1, 2, 3, "VIN000001ABCD"),
    (2, 1, 2, "VIN000002XYZA"),
    (3, 3, 1, "VIN000003QWER"),
    (4, 1, 1, "VIN000004POIU"),
    (5, 2, 2, "VIN000005LKJH"),
    (6, 3, 3, "VIN000006MNBV"),
    (7, 1, 3, "VIN000007ZXCV"),
    (8, 2, 1, "VIN000008FDSA"),
    (9, 3, 2, "VIN000009TREW"),
    (10, 1, 2, "VIN000010UYTR")
};


// now lets apply the join 'inner join'
// till the .NET 9 there was no left and right joins
var res = from car in Cars
          join model in Models
          on car.ModelId equals model.Id
          join make in Makes
          on car.MakeId equals make.Id
          select new
          {
              Id = car.Id,
              model = model.Name,
              Make = make.Name,
              car.VIN
          };

// the same result using the extension method is
var resExt = Cars.Join(Makes,
    car => car.MakeId,
    make => make.Id,
    (car, make) => new
    {
        Id = car.Id,
        car.ModelId, // for the second join
        make = make.Name,
        car.VIN
    })
    .Join(Models,
    newCar => newCar.ModelId,
    model => model.Id,
    (newCar, model) => new
    {

        Id = newCar.Id,
        model = model.Name,
        newCar.VIN,
        newCar.make
    });



// now we can check the result 

foreach(var car in resExt)
    Console.WriteLine($"\nCarID :{car.Id} - VIN : {car.VIN} - Model : {car.model} - Make : {car.make}");

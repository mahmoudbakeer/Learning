
// now with the linq joins
// the same thing applies on the joins in sql applies here 
// the difference is to know how to use the extension method 

using System.Xml.Linq;

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
    (12, 4, 5, "VIN000009FEEW"),
    (10, 1, 2, "VIN000010UYTR")
};

var resInnerJoinQuery = from car in Cars
               join model in Models
                    on car.ModelId equals model.Id
               join make in Makes
                    on car.MakeId equals make.Id
               select new
               {
                   Id = car.Id,
                   Model = model.Name,
                   Make = make.Name,
                   VIN = car.VIN
               };
// now lets apply the join 'inner join'
// till the .NET 9 there was no left and right joins
var resInnerJoinExtension = Cars.Join(Makes,
    car => car.MakeId,
    make => make.Id,
    (car, make) => new
    {
        Id = car.Id,
        ModelId = car.ModelId,
        Make = make.Name,
        VIN = car.VIN,
    })
    .Join(Models,
    car => car.ModelId,
    model => model.Id,
    (car, model) => new
    {
        Id = car.Id,
        Model = model.Name,
        Make = car.Make,
        VIN = car.VIN
    });


// now we can check the result 
// if you want to see the result you can unComment this section
//foreach(var car in resInnerJoinExtension)
//    Console.WriteLine($"\nCarID :{car.Id} - VIN : {car.VIN} - Model : {car.Model} - Make : {car.Make}");

// now lets use the group join, means join the data based on specific
/*
 * The GroupJoin operator in LINQ is highly efficient for handling one-to-many relationships.
 * While a standard Join produces a flat result set (repeating the left element for every match on the right),
 * a GroupJoin produces a hierarchical result set.
 * It returns one element from the left side, paired with a collection of all matching elements from the right side.
 * This is exactly how you would group related data,
 * such as finding a Group and listing all employees inside that Group as an array,
 * rather than repeating the Group name on five different rows.
 */

// 1. Data Sources representing a 1-to-Many relationship
(int Id, string Name)[] Departments =
{
            (1, "IT"),
            (2, "HR"),
            (3, "Sales") // Notice that Sales will have no employees
        };

(int Id, string Name, int DepartmentId)[] Employees =
{
            (1, "Mahmoud", 1),
            (2, "Ahmad", 1),
            (3, "Sara", 2),
            (4, "Omar", 1)
        };

// The 'into' keyword is what triggers the GroupJoin in Query Syntax.
// It takes all the matching employees and puts them into the 'employeeGroup' collection.
var querySyntaxResult = from Dept in Departments
                        join Emp in Employees
                            on Dept.Id equals Emp.DepartmentId
                        into EmployeesGroup
                        select new
                        {
                            Department = Dept.Name,
                            Employees = EmployeesGroup,
                            // we can also get aggregation from the group
                            EmployeesCount = EmployeesGroup.Count()
                        };

var ExtSyntaxResult = Departments.GroupJoin(Employees,
                        Dept => Dept.Id,
                        Emp => Emp.DepartmentId,
                        (Dept, EmployeesGroup) => new
                            {
                                Department = Dept.Name,
                                Employees = EmployeesGroup,
                                // we can also get aggregation from the group
                                EmployeesCount = EmployeesGroup.Count()
                            }
                        );

// if you want to see the result you can unComment this section
//foreach (var Group in ExtSyntaxResult)
//{
//    Console.WriteLine($"\nDepartment: {Group.Department}");

//    if (Group.Employees.Any())
//    {
//        Console.WriteLine($"Employees Count : {Group.EmployeesCount}");
//        Console.WriteLine("They are : ");
//        foreach (var employee in Group.Employees)
//        {
//            Console.WriteLine($"  - {employee.Name}");
//        }
//    }
//    else
//    {
//        Console.WriteLine("  - No employees found in this Group.");
//    }
//}


// the difference with the groupby that groupby operate on single list and group the result based on attribute that can be shared among the objects with diff values
// the GroupJoin operate on more than list the parent and child list, works mostly with one-many relationship


// Now let's see how to do the left join using the GroupJoin.
(int Id, string Name)[] Persons =
{
    (1, "Mahmoud"), // Mahmoud has 2 pets
    (2, "Ahmad"),   // Ahmad has 1 pet
    (3, "Omar")     // Omar has 0 pets (He will disappear in an Inner Join, but stay in a Left Join!)
};

(int Id, string PetName, int OwnerId)[] Pets =
{
    (1, "Rex", 1),
    (2, "Whiskers", 1),
    (3, "Max", 2)
};

// ==========================================
// 1. QUERY SYNTAX
// ==========================================
var leftJoinQuery = from person in Persons
                    join pet in Pets
                         on person.Id equals pet.OwnerId
                    into PersonPetsGroup // Group the pets by their owner
                    from pet in PersonPetsGroup.DefaultIfEmpty() // Flatten the group, providing a default if empty
                    select new
                    {
                        PersonName = person.Name,
                        // Clarification: Because 'pet' is a ValueTuple, it cannot be null.
                        // However, if Omar has no pets, DefaultIfEmpty() generates an empty struct
                        // where 'PetName' is null. The '??' handles that null gracefully.
                        PetName = pet.PetName ?? "No Pet"
                    };


// ==========================================
// 2. METHOD SYNTAX (The exact same logic)
// ==========================================
var leftJoinMethod = Persons
    // Step A: Group the Right side (Pets) into the Left side (Persons)
    .GroupJoin(
        Pets,
        person => person.Id,
        pet => pet.OwnerId,
        (person, petsGroup) => new
        {
            Person = person,
            PetsGroup = petsGroup
        })
    // Step B: Flatten the hierarchy back into a flat list
    .SelectMany(
        // We iterate over the grouped pets. DefaultIfEmpty() saves Omar from disappearing!
        groupedItem => groupedItem.PetsGroup.DefaultIfEmpty(),

        // Final result selector matching the person with each of their pets (or default)
        (groupedItem, pet) => new
        {
            PersonName = groupedItem.Person.Name,
            PetName = pet.PetName ?? "No Pet"
        });


// Let's test the output!
Console.WriteLine("--- Left Outer Join Results ---");

foreach (var result in leftJoinMethod)
{
    Console.WriteLine($"Person: {result.PersonName,-10} | Pet: {result.PetName}");
}





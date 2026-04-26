using System;
using System.Linq;

// ==========================================
// 26. ARRAYS: ADVANCED OPERATIONS & LINQ PROJECTIONS
// ==========================================

Console.WriteLine("--- Array Basics & Copying ---");

// 1. Declaration and Initialization (Using C# 12 Collection Expressions)
int[] arr = [1, 2, 3];

Console.WriteLine($"Original Array: {string.Join(", ", arr)}");

// 2. Copying Arrays
// Method A: The Classic Way (Array.Copy)
int[] copy1 = new int[arr.Length];
Array.Copy(arr, copy1, arr.Length);

// Method B: The Instance Method (CopyTo)
int[] copy2 = new int[arr.Length];
arr.CopyTo(copy2, 0); // 0 is the starting index in the destination array

// Method C: The Modern C# Way (Range Operator - Highly Recommended!)
// This creates a perfect clone instantly without needing to initialize an empty array first.
int[] copy3 = arr[..];

Console.WriteLine($"Copied Array: {string.Join(", ", copy3)}");


// ==========================================
// 💡 ADVANCED LINQ: GROUPING WITH RESULT SELECTOR
// ==========================================

// An array of Tuples representing employees
(string name, int age, string department)[] people =
[
   ("Hamed", 23, "IT"),
   ("Hadi", 25, "Sales"),
   ("Mahmoud", 22, "IT"),
   ("Qamar", 22, "HR"),
   ("Sara", 25, "IT"),
   ("Haidar", 26, "HR"),
   ("Hasan", 32, "Sales")
];

Console.WriteLine("\n--- Advanced GroupBy (With Projection & Sorting) ---");

// 🏆 SENIOR TECHNIQUE: 
// Instead of using .GroupBy().Select(), we use the overloaded GroupBy that takes 
// a "Result Selector". This groups the data AND reshapes it in a single pass!
var departmentGroups = people.GroupBy(
    person => person.department, // 1. The Key Selector (Group by Department)
    (key, groupItems) => new     // 2. The Result Selector (What to do with each group)
    {
        DepartmentName = key,
        // We can even SORT the items inside the group right here!
        Employees = groupItems.OrderBy(person => person.name)
    }
);

// Displaying the projected data
foreach (var group in departmentGroups)
{
    Console.WriteLine($"\nDepartment: {group.DepartmentName}");
    foreach (var person in group.Employees)
    {
        Console.WriteLine($"\t Name: {person.name.PadRight(10)} | Age: {person.age}");
    }
}
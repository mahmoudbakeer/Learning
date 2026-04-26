using System;
using System.Collections.Generic;
using System.Linq;

// ==========================================
// SORTEDLIST<TKey, TValue>
// A collection of key/value pairs that are AUTOMATICALLY SORTED by the key.
// It uses less memory than SortedDictionary, making it great for read-heavy operations.
// ==========================================

SortedList<string, int> sortedList = new SortedList<string, int>();

// A. ADDING ELEMENTS 
// They will be automatically sorted alphabetically by the Key!
sortedList.Add("Banana", 10);
sortedList.Add("Pineapple", 22);
sortedList.Add("Apple", 30);
sortedList.Add("Guava", 23);
sortedList.Add("Berries", 28);
sortedList.Add("Orange", 25);

Console.WriteLine("--- Elements of the SortedList ---");
// Looping on the SortedList (It will print in alphabetical order: Apple, Banana...)
foreach (var kvp in sortedList)
{
    Console.WriteLine($"\tKey: {kvp.Key} - Value: {kvp.Value}");
}


// B. REMOVING ELEMENTS
sortedList.Remove("Apple");

Console.WriteLine("\n--- After removing 'Apple' ---");
foreach (var kvp in sortedList)
{
    Console.WriteLine($"\tKey: {kvp.Key} - Value: {kvp.Value}");
}


// ==========================================
// C. LINQ ON SORTEDLIST (Query vs Method Syntax)
// ==========================================

Console.WriteLine("\n--- Fruits with price > 10 (Query Syntax) ---");
var resQuery = from kvp in sortedList
               where kvp.Value > 10
               select kvp;

foreach (var item in resQuery)
{
    Console.WriteLine($"\tKey: {item.Key} - Value: {item.Value}");
}

Console.WriteLine("\n--- Fruits with price > 10 (Method Syntax) ---");
var resMethod = sortedList.Where(kvp => kvp.Value > 10);

foreach (var item in resMethod)
{
    Console.WriteLine($"\tKey: {item.Key} - Value: {item.Value}");
}


// ==========================================
// D. ADVANCED GROUPING & AGGREGATION 
// You can group by computed properties or methods (like .ToLower())
// ==========================================

// Adding items with different cases to test our grouping
sortedList.Add("apple", 10);
sortedList.Add("Apple", 12); // Allowed because 'A' and 'a' are different keys!
sortedList.Add("banana", 10);

Console.WriteLine("\n--- Grouping by Key.ToLower() ---");
var resGroups = sortedList.GroupBy(kvp => kvp.Key.ToLower());

foreach (var group in resGroups)
{
    Console.WriteLine($"\n\tGroup Key: {group.Key}");
    foreach (var item in group)
    {
        Console.Write($"\t[Original Key = {item.Key}, Price = {item.Value}] ");
    }
}
Console.WriteLine();

// Using AggregateBy (The .NET 9 Superpower) to sum values based on their lowercase name
Console.WriteLine("\n--- Sum of elements based on their type (AggregateBy) ---");
var resAgb = sortedList.AggregateBy(
    kvp => kvp.Key.ToLower(),        // The grouping Key
    seed : 0,                               // The Seed (starts at 0)
    (acc, item) => acc + item.Value  // The Accumulator (Summing prices)
);

foreach (var item in resAgb)
{
    Console.WriteLine($"\tType: {item.Key} | Total Price: {item.Value}");
}


// ==========================================
// E. COMPLEX OBJECTS: GROUPING AND SORTING WITHIN GROUPS
// ==========================================


SortedList<int, Employee> employees = new SortedList<int, Employee>()
{
    { 1, new Employee("Alice", "HR", 50000) },
    { 2, new Employee("Bob", "IT", 70000) },
    { 3, new Employee("Charlie", "HR", 52000) },
    { 4, new Employee("Daisy", "IT", 80000) },
    { 5, new Employee("Ethan", "Marketing", 45000) }
};

Console.WriteLine("\n--- Employees Grouped by Dept & Sorted by Salary (Descending) ---");

// Group employees by their Department
var employeesGroups = employees.GroupBy(kvp => kvp.Value.Department);

foreach (var group in employeesGroups)
{
    Console.WriteLine($"\nDepartment: {group.Key}");

    // 💡 PRO TIP: Sorting the elements INSIDE the group!
    // Now we order the employees inside this specific department by Salary (Highest to Lowest)
    var sortedEmployeesInDept = group.OrderByDescending(kvp => kvp.Value.Salary);

    foreach (var empKvp in sortedEmployeesInDept)
    {
        Console.WriteLine($"\tID: {empKvp.Key} | Name: {empKvp.Value.Name} | Salary: ${empKvp.Value.Salary}");
    }
}

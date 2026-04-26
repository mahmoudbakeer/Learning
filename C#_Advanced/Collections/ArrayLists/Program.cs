using System;
using System.Collections;
using System.Linq;

// ==========================================
// 23. ARRAYLIST (LEGACY COLLECTION - AVOID IN MODERN CODE)
// Belongs to System.Collections (Non-Generic).
// It stores everything as 'object', meaning it accepts mixed data types.
// ==========================================

// 1. Initialization
// Note: It's better to use the classic initialization for ArrayList
ArrayList arrList = new ArrayList { 1, "Hello", "Haland", true, 12.2 };

Console.WriteLine("--- ArrayList Basics ---");
Console.WriteLine($"Count (Actual items): {arrList.Count}");
Console.WriteLine($"Capacity (Allocated memory): {arrList.Capacity}");

// 2. Adding and Removing
arrList.Add(100);
arrList.Remove("Hello"); // Removes the first occurrence of "Hello"
arrList.RemoveAt(0);     // Removes the item at index 0


// ==========================================
// 💡 IMPORTANT FEATURE: MEMORY MANAGEMENT
// ==========================================
// As you noted, removing items decreases 'Count' but DOES NOT decrease 'Capacity'.
// To free up memory and shrink the capacity to match the actual count:
arrList.TrimToSize();
Console.WriteLine($"\nCapacity after TrimToSize(): {arrList.Capacity}");


// ==========================================
// 🚨 WHY WE DON'T USE IT: BOXING & TYPE SAFETY
// ==========================================

// 1. Type Safety Issue: 
// You have to cast elements back to their original type.
// If you guess wrong, the app crashes!
try
{
    // The 3rd item is 'true' (bool), but if we try to cast it to string:
    string badCast = (string)arrList[2];
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"\nCrash averted! Type casting failed: {ex.Message}");
}

// ==========================================
// 💡 HOW TO USE LINQ WITH ARRAYLIST
// Since ArrayList is not Generic (doesn't use <T>), standard LINQ won't work directly.
// You must use .OfType<T>() to safely filter and cast elements!
// ==========================================

Console.WriteLine("\n--- Extracting Specific Types using LINQ ---");

// Safely grabs ONLY the integers from the mixed ArrayList
var onlyNumbers = arrList.OfType<int>().ToList();

Console.WriteLine($"Numbers found: {string.Join(", ", onlyNumbers)}");

// Safely grabs ONLY the strings
var onlyStrings = arrList.OfType<string>().ToList();
Console.WriteLine($"Strings found: {string.Join(", ", onlyStrings)}");
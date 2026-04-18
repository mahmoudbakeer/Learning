using System;
using System.Linq;

// ==========================================
// 27. LINQ: ZIP EXTENSION METHOD
// Combines multiple sequences into one sequence of tuples or custom objects.
// 💡 GOLDEN RULE: The Zip method ALWAYS stops at the end of the SHORTEST sequence.
// Extra elements in longer sequences are simply ignored.
// ==========================================

// Declare the collections (sequences)
int[] first = [1, 2, 3, 4, 5];
string[] second = ["One", "Two", "Three", "Four"];

Console.WriteLine("--- 1. Zip Two Sequences ---");

// This will produce a collection of Tuples (int, string).
// Why 4 elements and not 5? Because the 'second' array only has 4 elements!
var resTwo = first.Zip(second);

foreach (var (firstE, secondE) in resTwo)
{
    // Notice how we can deconstruct the Tuple directly in the foreach loop!
    Console.WriteLine($"{firstE} - {secondE}");
}
// Output: 1 - One, 2 - Two, 3 - Three, 4 - Four


Console.WriteLine("\n--- 2. Zip Three Sequences ---");

string[] third = ["Ek", "Do", "Teen", "Char"]; // Hindi numbers!

// This overload zips three sequences together into a Tuple of 3 elements (int, string, string)
var resOfThree = first.Zip(second, third);

foreach (var (firstE, secondE, thirdE) in resOfThree)
{
    Console.WriteLine($"{firstE} - {secondE} - {thirdE}");
}


Console.WriteLine("\n--- 3. Zip with Result Selector (Custom Reshaping) ---");

// This is the most powerful overload. 
// Instead of returning a Tuple, it returns an IEnumerable of whatever you define in the lambda!
var resOfSelector = first.Zip(second, (num, text) => new
{
    Number = num,
    StringNumber = text
});

foreach (var item in resOfSelector)
{
    Console.WriteLine($"Number Property: {item.Number} | Text Property: {item.StringNumber}");
}



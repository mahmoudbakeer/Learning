using System;
using System.Collections.Generic;
using System.Linq;

// ==========================================
// 21. SORTEDDICTIONARY<TKey, TValue>
// Maintains the sort of the elements based on the KEYS using a Red-Black Tree.
// Best used when you need frequent ADD/REMOVE operations while keeping the data sorted!
// ==========================================

// Initialization (Notice the random order of keys, the tree will sort them automatically)
SortedDictionary<int, string> sDict = new SortedDictionary<int, string>{
    { 3, "ho" },
    { 1, "ha" },
    { 2, "he" }
};

Console.WriteLine("--- Basic SortedDictionary Iteration ---");
// Looping on the SortedDictionary (Will always print in order: 1, 2, 3)
foreach (var item in sDict)
{
    Console.WriteLine($"\tKey: {item.Key} - Value: {item.Value}");
}

// Adding and Removing elements (Very fast O(log N) operation)
sDict.Add(23, "Hello");
sDict.Remove(2);


// ==========================================
// 💡 ADDITIONAL FEATURES & BENEFITS OF SORTEDDICTIONARY
// ==========================================

Console.WriteLine("\n--- Feature 1: Custom Sorting (IComparer) ---");
// BENEFIT: You can completely change HOW the tree sorts the keys!
// Example: What if we want a LEADERBOARD sorted from Highest to Lowest?
// We pass 'Comparer<int>.Create()' to the constructor to reverse the sort order.

SortedDictionary<int, string> leaderBoard = new SortedDictionary<int, string>(
    Comparer<int>.Create((x, y) => y.CompareTo(x)) // Descending Order!
);

leaderBoard.Add(100, "Samer");
leaderBoard.Add(500, "Mahmoud");
leaderBoard.Add(250, "Ahmad");

foreach (var player in leaderBoard)
{
    // Prints: Mahmoud (500) -> Ahmad (250) -> Samer (100)
    Console.WriteLine($"\tScore: {player.Key} | Player: {player.Value}");
}


Console.WriteLine("\n--- Feature 2: Extremely Fast Extreme Values (Min/Max) ---");
// BENEFIT: Because it's a sorted tree, finding the smallest or largest key 
// is incredibly fast without having to scan the entire dictionary!

// Grabbing the lowest key (Using LINQ First())
var lowestKey = sDict.Keys.First();
Console.WriteLine($"\tSmallest Key in sDict: {lowestKey}");

// Grabbing the highest key (Using LINQ Last())
var highestKey = sDict.Keys.Last();
Console.WriteLine($"\tLargest Key in sDict: {highestKey}");


Console.WriteLine("\n--- Feature 3: Range Queries using LINQ ---");
// BENEFIT: Since the data is perfectly ordered, LINQ operations like Skip() and Take()
// or filtering ranges (Where key > X and key < Y) become very predictable and logical,
// which is perfect for building Pagination or Timeline features.

SortedDictionary<int, string> timelineEvents = new SortedDictionary<int, string>()
{
    { 2020, "Pandemic Started" },
    { 2023, "AI Boom" },
    { 2010, "Smartphones Rise" },
    { 2026, "DVLD Project Completed" } // Automatically sorted to the end!
};

// Get events between 2015 and 2025
var midEvents = timelineEvents.Where(kvp => kvp.Key >= 2015 && kvp.Key <= 2025);

Console.WriteLine("\tEvents between 2015 and 2025:");
foreach (var ev in midEvents)
{
    Console.WriteLine($"\t- Year {ev.Key}: {ev.Value}");
}
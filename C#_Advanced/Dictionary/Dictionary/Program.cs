// ==========================================
// DICTIONARY (Key-Value Pairs)
// ==========================================

// Declaring and Initializing a dictionary
Dictionary<string, int> myDict = new Dictionary<string, int>() {
    { "Banana" , 10 },
    { "Apple" , 29 },
    { "Pineapple" , 12 },
    { "Strawberry" , 54 },
    { "Orange" , 13 }
};


// ==========================================
// A. ADDING & UPDATING ELEMENTS
// ==========================================

// Method 1: The Indexer [] (Safe - Upsert)
// If "Hello" exists, it UPDATES the value. If not, it ADDS it.
myDict["Hello"] = 1;
Console.WriteLine($"The Value for 'Hello' is {myDict["Hello"]}");

// Method 2: The Add() Method (Strict)
// Throws an ArgumentException if the key ALREADY EXISTS!
myDict.Add("Hi", 12);
myDict.Add("TheSecondKey", 40);


// ==========================================
// B. LOOPING ON A DICTIONARY
// ==========================================
Console.WriteLine("\nLooping on Dictionary:");
// Each item in a Dictionary is a 'KeyValuePair<TKey, TValue>'
foreach (var kvp in myDict)
{
    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
}


// ==========================================
// C. REMOVING & SAFELY GETTING ELEMENTS
// ==========================================

// Remove() takes the KEY. Returns true if removed, false if not found.
myDict.Remove("Hello");

// TryGetValue: The SAFEST way to read from a dictionary.
// It prevents the "KeyNotFoundException" crash if the key doesn't exist.
Console.WriteLine("\nTesting TryGetValue:");
if (myDict.TryGetValue("Hello", out int retrievedValue))
{
    Console.WriteLine($"Found! Value is: {retrievedValue}");
}
else
{
    Console.WriteLine("Element 'Hello' Not Found (We just removed it!)");
}


// ==========================================
// D. LINQ ON DICTIONARY
// ==========================================

// 1. Selecting only Values (Returns IEnumerable<int>)
var listOfValues = myDict.Select(kvp => kvp.Value);

// ==========================================
//  DICTIONARY VS IEnumerable<KeyValuePair>
// ==========================================
// When you apply LINQ (like .Where or .Select) to a Dictionary, it loses its 
// superpowers and becomes a simple IEnumerable. Here is the technical comparison:
//
// | Feature          | Dictionary<TKey, TValue>               | IEnumerable<KeyValuePair<TKey, TValue>> |
// |------------------|----------------------------------------|-----------------------------------------|
// | Lookup Speed     | O(1) Extremely Fast! (e.g., dict["A"]) | O(N) Slow! Must use LINQ (FirstOrDefault|
// | Duplicate Keys   | ❌ NOT Allowed. Program will crash.     | ✅ Allowed. It's just a normal list.     |
// | Modification     | ✅ Allowed (Add, Remove, Update).       | ❌ NOT Allowed. It is Read-Only.         |
// | Common Use Case  | Storing base data in memory for speed. | The temporary result returned by LINQ.  |
//
// 💡 PRO TIP: 
// Always use .ToDictionary(kvp => kvp.Key, kvp => kvp.Value) at the end of your LINQ 
// query if you need to restore the O(1) lookup speed and the Dictionary capabilities!
// ==========================================

// 2. Filtering (Where) - Returns IEnumerable<KeyValuePair<string, int>>
var filteredDict = myDict.Where(kvp => kvp.Value > 12);

Console.WriteLine("\nDictionary items where Value > 12:");
foreach (var kvp in filteredDict)
{
    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
}

// 3. Ordering and Aggregating 
var orderedValues = listOfValues.Order(); // Using the clean Order() we learned!
int summation = myDict.Select(kvp => kvp.Value).Sum();

Console.WriteLine($"\nThe Summation of all values is: {summation}");


// ==========================================
// E. ADVANCED LINQ: GROUPING & FLATTENING (SelectMany)
// ==========================================

Dictionary<string, string> fruitDict = new Dictionary<string, string>()
{
    {"Banana" , "Tree"},
    {"Strawberry" , "Herb"},
    {"Berries" , "Herb"},
    {"Orange" , "Tree"}
};

// WHAT DOES THIS CRAZY LINE DO?
// 1. GroupBy: Groups fruits into two buckets ("Tree" and "Herb").
// 2. OrderBy: Sorts the buckets alphabetically by their name (Herb comes before Tree).
// 3. SelectMany: Flattens the buckets! Pours all items back into a single list.
// 4. Select: Transforms the KeyValuePair into a new Anonymous Object { Key, Value }.
var res = fruitDict
    .GroupBy(kvp => kvp.Value)
    .OrderBy(group => group.Key)
    .SelectMany(group => group.Select(kvp => new { kvp.Key, kvp.Value }));

Console.WriteLine("\nAdvanced GroupBy & SelectMany Result:");
foreach (var item in res)
{
    // Output will be ordered by the GROUP (Herbs first, then Trees)
    Console.WriteLine($"Fruit: {item.Key} belongs to {item.Value}");
}
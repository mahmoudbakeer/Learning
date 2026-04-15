using System;
using System.Collections.Generic;
using System.Linq;

// ==========================================
// 13. HASHSET: THE BASICS
// HashSets store a collection of UNIQUE elements.
// They provide ultra-fast O(1) performance for adding, removing, and searching.
// ==========================================

// Initialization using Collection Expressions
HashSet<string> fruitsSet = ["Apple", "Orange", "Berries"];

Console.WriteLine("--- Basic HashSet Operations ---");
Console.WriteLine($"Initial Set: {string.Join(", ", fruitsSet)}");

// A. Removing Elements
// Returns TRUE if the item existed and was deleted, FALSE if it wasn't found.
bool isRemoved = fruitsSet.Remove("Apple");
Console.WriteLine($"Was 'Apple' removed? {isRemoved}");

// B. Adding Elements
// Returns TRUE if the element was added successfully.
// Returns FALSE if the element ALREADY EXISTS (it ignores duplicates silently).
fruitsSet.Add("Pineapple");            // Returns true
bool isOrangeAdded = fruitsSet.Add("Orange"); // Returns false (already exists!)

Console.WriteLine($"Was 'Orange' added again? {isOrangeAdded}");

// C. Checking for Existence (Extremely Fast!)
if (fruitsSet.Contains("Apple"))
{
    Console.WriteLine("Apple exists in the set.");
}
else
{
    Console.WriteLine("Apple does NOT exist in the set.");
}

// ==========================================
// 14. REMOVING DUPLICATES FROM COLLECTIONS
// One of the most common and powerful uses of HashSet!
// ==========================================

int[] arrayWithDuplicates = [1, 1, 2, 2, 3, 3, 3, 4, 4, 5, 5, 6, 6];
Console.WriteLine($"\n--- Removing Duplicates ---");
Console.WriteLine($"Original Array: {string.Join(", ", arrayWithDuplicates)}");

// Pass the array/list directly into the HashSet constructor to instantly remove duplicates
HashSet<int> uniqueNumbers = new HashSet<int>(arrayWithDuplicates);
Console.WriteLine($"Clean HashSet:  {string.Join(", ", uniqueNumbers)}");


// ==========================================
// 15. LINQ ON HASHSETS
// HashSets fully support LINQ, but remember: LINQ returns IEnumerable, not HashSet.
// ==========================================

Console.WriteLine($"\n--- LINQ on HashSet ---");

// Example 1: Filtering numbers
var evenNumbers = uniqueNumbers.Where(n => n % 2 == 0);
Console.WriteLine($"Even numbers only: {string.Join(", ", evenNumbers)}");

var greaterThanThree = uniqueNumbers.Where(n => n > 3);
Console.WriteLine($"Numbers > 3: {string.Join(", ", greaterThanThree)}");

// Example 2: Filtering strings
HashSet<string> namesSet = ["Hani", "Ahmad", "Samer", "Moazz"];
var namesStartingWithS = namesSet.Where(name => name.StartsWith('S'));
Console.WriteLine($"Names starting with 'S': {string.Join(", ", namesStartingWithS)}");


// ==========================================
// 16. SET OPERATIONS (Union, Intersect, Except)
// ==========================================

HashSet<int> Oset = [1, 2, 6, 3];
HashSet<int> Eset = [4, 5, 3, 6];

Console.WriteLine("\n--- Set Operations ---");
Console.WriteLine($"Original Oset: {string.Join(", ", Oset)}");
Console.WriteLine($"Original Eset: {string.Join(", ", Eset)}\n");

// ==========================================
// METHOD A: Using LINQ (Creates a NEW collection, Original is Safe)
// ==========================================

// 1. Union (Combines both, removes duplicates)
HashSet<int> unionResult = Oset.Union(Eset).ToHashSet();
Console.WriteLine($"LINQ Union Result: {string.Join(", ", unionResult)}");

// 2. Intersect (Gets common elements only)
HashSet<int> intersectResult = Oset.Intersect(Eset).ToHashSet();
Console.WriteLine($"LINQ Intersect Result: {string.Join(", ", intersectResult)}\n");


// ==========================================
// METHOD B: Using HashSet NATIVE Methods (Modifies IN-PLACE)
// These methods are FASTER and allocate zero extra memory!
// ==========================================

// 1. UnionWith: Adds Eset elements directly into Oset
Oset.UnionWith(Eset);
Console.WriteLine($"Native UnionWith (Oset modified): {string.Join(", ", Oset)}");

Oset = [1, 2, 6, 3]; // Reset for next example

// 2. IntersectWith: Removes everything from Oset EXCEPT the common elements
Oset.IntersectWith(Eset);
Console.WriteLine($"Native IntersectWith (Oset modified): {string.Join(", ", Oset)}");

Oset = [1, 2, 6, 3]; // Reset for next example

// 3. ExceptWith: Removes elements from Oset that exist in Eset (Oset - Eset)
Oset.ExceptWith(Eset);
Console.WriteLine($"Native ExceptWith (Oset modified): {string.Join(", ", Oset)}");

Oset = [1, 2, 6, 3]; // Reset for next example

// 4. SymmetricExceptWith: Keeps elements that are in ONE of the sets, but NOT in BOTH!
Oset.SymmetricExceptWith(Eset);
Console.WriteLine($"Native SymmetricExceptWith (Oset modified): {string.Join(", ", Oset)}");

// ==========================================
// 17. SET COMPARISONS
// These methods return boolean values and are highly optimized.
// The parameter can be a HashSet, Array, List, or any IEnumerable<T>.
// ==========================================

HashSet<int> set1 = [1, 2, 3, 4];
HashSet<int> set2 = [1, 2, 3, 4]; // Identical to set1
HashSet<int> set3 = [1, 2, 3];    // A smaller piece of set1

Console.WriteLine("\n--- Set Comparisons ---");

// 1. SetEquals: Checks if both sets contain the EXACT same elements.
Console.WriteLine($"Is set1 equal to set2? {set1.SetEquals(set2)}"); // True
Console.WriteLine($"Is set1 equal to set3? {set1.SetEquals(set3)}"); // False

// 2. IsSubsetOf: Checks if the set is fully contained within the other.
// Rule of thumb: ChildSet.IsSubsetOf(ParentSet)
Console.WriteLine($"\nIs set1 a subset of set2? {set1.IsSubsetOf(set2)}"); // True (because they are equal)
Console.WriteLine($"Is set3 a subset of set1? {set3.IsSubsetOf(set1)}"); // True (all elements of set3 are in set1)
Console.WriteLine($"Is set1 a subset of set3? {set1.IsSubsetOf(set3)}"); // False (set1 has '4' which is missing in set3)

// 3. IsSupersetOf: Checks if the set fully contains the other.
// Rule of thumb: ParentSet.IsSupersetOf(ChildSet)
Console.WriteLine($"\nIs set1 a superset of set2? {set1.IsSupersetOf(set2)}"); // True
Console.WriteLine($"Is set1 a superset of set3? {set1.IsSupersetOf(set3)}"); // True (set1 contains all of set3)
Console.WriteLine($"Is set3 a superset of set1? {set3.IsSupersetOf(set1)}"); // False

// 4. Overlaps: Checks if they share AT LEAST ONE common element.
Console.WriteLine($"\nDoes set1 overlap with set3? {set1.Overlaps(set3)}"); // True (they share 1, 2, and 3)


// ==========================================
// 💡 PRO TIP: PROPER SUBSETS & SUPERSETS
// ==========================================
// What if you want to know if set3 is a subset of set1, BUT you want to 
// guarantee they are NOT exactly equal? (Strictly smaller or larger).
// Use IsProperSubsetOf / IsProperSupersetOf.

Console.WriteLine("\n--- Proper Comparisons ---");
// False, because they are exactly equal (A proper subset must be strictly smaller)
Console.WriteLine($"Is set1 a PROPER subset of set2? {set1.IsProperSubsetOf(set2)}");

// True, because set3 is fully inside set1 AND set3 is strictly smaller.
Console.WriteLine($"Is set3 a PROPER subset of set1? {set3.IsProperSubsetOf(set1)}");

// ==========================================
// SORTEDSET<T>
// A collection of UNIQUE elements that maintains a SORTED order automatically.
// Behind the scenes, it uses a self-balancing binary search tree (Red-Black Tree).
// Performance: O(log N) for add/remove/search (Slower than HashSet's O(1), but keeps order).
// ==========================================

// 1. Initialization (It automatically sorts them!)
SortedSet<int> sortedSet = [4, 5, 6, 2, 7, 9, 22];

Console.WriteLine("--- SortedSet Basics ---");
Console.WriteLine($"Sorted Set Elements: {string.Join(", ", sortedSet)}");
// Output: 2, 4, 5, 6, 7, 9, 22


// 2. LINQ Operations 
// Chaining Where (Filter) and Select (Transform)
var resEvenSquared = sortedSet
    .Where(n => n % 2 == 0) // Keep only even numbers (2, 4, 6, 22)
    .Select(n => n * n);    // Square them

Console.WriteLine($"Even Squared Result: {string.Join(", ", resEvenSquared)}");
// Output: 4, 16, 36, 484


// ==========================================
// 💡 THE SUPERPOWERS OF SORTEDSET
// Because it's sorted via a tree, it has special native methods!
// ==========================================

Console.WriteLine("\n--- SortedSet Special Powers ---");

// A. Instant Min & Max 
// It doesn't need to search the whole list! It just grabs the far-left or far-right tree node.
Console.WriteLine($"Minimum Value: {sortedSet.Min}"); // 2
Console.WriteLine($"Maximum Value: {sortedSet.Max}"); // 22

// B. GetViewBetween (Creates a "Window" or subset of the data)
// Gets all numbers between 5 and 10 (inclusive) extremely fast.
SortedSet<int> window = sortedSet.GetViewBetween(5, 10);
Console.WriteLine($"View Between 5 and 10: {string.Join(", ", window)}");
// Output: 5, 6, 7, 9

// ⚠️ DANGER NOTE ON 'GetViewBetween': 
// It does NOT copy the data. It is a "live view" of the original set.
// If you add or remove items from 'window', it will modify 'sortedSet' directly!



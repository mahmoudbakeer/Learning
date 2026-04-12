// 1. Initialization
List<int> numbers = [];
// Other valid initializations:
// List<int> numbers = new List<int>();
// List<int> numbers = new List<int> { 1, 2 };

// 2. Add: Modifies the original list by adding to the end. Returns void.
numbers.Add(1);
numbers.Add(2);
numbers.Add(3);
numbers.Add(4);
Console.WriteLine($"List after Add(): {string.Join(", ", numbers)}");

// NOTE ON APPEND: 
// Append is a LINQ extension. It DOES NOT modify the original list.
// It returns a new IEnumerable<int>. To save it as a list, use .ToList().
// Example: var newList = numbers.Append(5).ToList();

// 3. Insert: Modifies the list by inserting an element at a specific index. Returns void.
// Parameters: (Index, Value)
numbers.Insert(2, 5); // Inserts '5' at index 2 (the 3rd position)
Console.WriteLine($"List after Insert(2, 5): {string.Join(", ", numbers)}");

// 4. InsertRange: Inserts a collection of elements at a specific index. Returns void.
List<int> newNumbers = [1, 2, 3];
numbers.InsertRange(0, newNumbers); // Inserts the whole list at the beginning
Console.WriteLine($"List after InsertRange() at start: {string.Join(", ", numbers)}");

// ==========================================
// 5. REMOVING ELEMENTS
// ==========================================

// A. Remove(value): Searches for the FIRST occurrence of the value and removes it.
// Returns TRUE if successfully removed, FALSE if the item was not found.
bool isDeleted = numbers.Remove(3);
Console.WriteLine($"Was '3' removed? {isDeleted}");
Console.WriteLine($"List after Remove(3): {string.Join(", ", numbers)}");

// B. RemoveAt(index): Removes the element at the specified index.
// Returns void. (Throws ArgumentOutOfRangeException if index is invalid).
numbers.RemoveAt(4);
Console.WriteLine($"List after RemoveAt(4): {string.Join(", ", numbers)}");

// C. RemoveAll(Predicate): Removes ALL elements that match the given condition.
// Returns an INT representing the total number of elements removed.
int removedCount = numbers.RemoveAll(x => x % 2 == 0); // Remove all even numbers
Console.WriteLine($"How many even numbers were removed? {removedCount}");
Console.WriteLine($"Final List after RemoveAll(): {string.Join(", ", numbers)}");


// ==========================================
// 6. LOOPING ON ELEMENTS
// ==========================================

// There is one good way to Loop on List Using Lambda expre.
Console.WriteLine("Looping using lambda expression : ");
numbers.ForEach(n => Console.Write($"{n} "));
Console.WriteLine();



// ==========================================
// 7. FILTERING ON LIST
// ==========================================

// Using .Where() to filter on element
numbers = [1, 2, 3,  4, 10 , 15 , 16 , 19];
Console.WriteLine($"Filtering on elements 'Printing the Odd only' : ");
numbers.Where(e => e % 2 == 1).ToList().ForEach(n => Console.Write($"{n} ")); // this won't affect the origingal list numbers 
Console.WriteLine();
// the second overload of the where we can use the index of the element in the predicate .Where((n , ind) => ind % 2 == 0)


// ==========================================
// 8. SORTING ON LIST
// ==========================================

// Using Sort() returns void and the exception if the list is null
// Sort() by default will sort them in Ascending order 
numbers.Sort();
Console.WriteLine($"Sorting on elements 'Ascending' : ");
numbers.ForEach(n => Console.Write($"{n} ")); // this Will affect the origingal list numbers 

// Usint Reverse will sort them in descending order 
numbers.Reverse();
Console.WriteLine($"Sorting on elements 'Descending' : ");
numbers.ForEach(n => Console.Write($"{n} ")); // this Will affect the origingal list numbers 


// ==========================================
// 1. The Default Order() 
// Works perfectly with primitive types like int, double, string, etc.
// ==========================================

numbers = [10, 5, 20, 1];
// Sorts numbers from lowest to highest
List<int> sortedNumbers = numbers.Order().ToList();
Console.WriteLine($"Default Number Order: {string.Join(", ", sortedNumbers)}");
// Output: 1, 5, 10, 20


List<string> fruits = ["Zebra", "apple", "Banana", "cherry"];
// Sorts strings alphabetically based on ASCII values 
// (Note: Uppercase letters come before lowercase by default)
List<string> sortedFruits = fruits.Order().ToList();
Console.WriteLine($"\nDefault String Order: {string.Join(", ", sortedFruits)}");
// Output: Banana, Zebra, apple, cherry (Because 'B' and 'Z' are uppercase)


// ==========================================
// 2. The Overload: Order(IComparer<T>)
// Used when you want to change HOW things are compared.
// ==========================================

// Let's fix the string sorting issue above! 
// We will pass a built-in comparer that ignores case sensitivity.
List<string> caseInsensitiveSort = fruits.Order(StringComparer.OrdinalIgnoreCase).ToList();

Console.WriteLine($"\nOverload String Order (Ignore Case): {string.Join(", ", caseInsensitiveSort)}");
// Output: apple, Banana, cherry, Zebra (Now it's properly alphabetical!)


// ==========================================
// 3. OrderDescending() and its Overload
// Exactly the same, but sorts from highest to lowest (or Z to A)
// ==========================================

List<int> highestToLowest = numbers.OrderDescending().ToList();
Console.WriteLine($"\nDescending Order: {string.Join(", ", highestToLowest)}");

// Initializing a list of products using Collection Expressions
List<Product> products = [
    new Product { Id = 1, Name = "Laptop", Price = 1500m },
    new Product { Id = 2, Name = "Mouse", Price = 25m },
    new Product { Id = 3, Name = "keyboard", Price = 45m }, // Notice the lowercase 'k'
    new Product { Id = 4, Name = "Monitor", Price = 200m },
    new Product { Id = 5, Name = "Desk Pad", Price = 25m }  // Same price as the Mouse
];


// ==========================================
// 1. The Standard OrderBy() & OrderByDescending()
// Requires a "Key Selector" (a Lambda Expression) to tell LINQ 
// WHICH property should be used for sorting.
// ==========================================

// Sort products by Price (Lowest to Highest)
List<Product> sortedByPrice = products.OrderBy(p => p.Price).ToList();


// ==========================================
// 2. Chaining Sorts: ThenBy()
// What happens if two products have the EXACT same price? (Mouse & Desk Pad)
// We use ThenBy() or ThenByDescending() to add a secondary sorting condition.
// NOTE: You can only use ThenBy AFTER an OrderBy.
// ==========================================

List<Product> sortedByPriceThenName = products
    .OrderBy(p => p.Price)         // 1st Priority: Sort by price ascending
    .ThenByDescending(p => p.Name) // 2nd Priority: If prices are equal, sort by Name descending
    .ToList();


// ==========================================
// 3. The Overload: OrderBy(keySelector, IComparer)
// Used when we need custom rules for the selected property.
// ==========================================

// In default sorting, uppercase comes before lowercase. So "keyboard" would be last.
// We use StringComparer.OrdinalIgnoreCase to force alphabetical sorting regardless of case.
List<Product> sortedByNameIgnoreCase = products
    .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
    .ToList();


// ==========================================
// 4. EXCEPTIONS in OrderBy
// OrderBy can throw an 'ArgumentNullException' in two specific cases.
// ==========================================

// A. Null List Scenario
List<Product>? nullList = null;

try
{
    // EXCEPTION 1: The source collection itself is null.
    // This will throw ArgumentNullException.
    var result1 = nullList.OrderBy(p => p.Price).ToList();
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Exception Caught: The source list is null. Details: {ex.Message}");
}

// B. Null Selector Scenario
try
{
    // EXCEPTION 2: The Key Selector (the lambda function) is null.
    Func<Product, decimal>? nullSelector = null;
    var result2 = products.OrderBy(nullSelector).ToList();
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Exception Caught: The selector is null. Details: {ex.Message}");
}

// use Find(Predicate) to return The first element 
Product prd = products.Find(p => p.Price > 1000);
Console.WriteLine($"The product with Price > 1000 is {prd.Name} - {prd.Price}");

// ==========================================
// 💡 IMPORTANT NOTE ON NULL VALUES IN PROPERTIES
// ==========================================
// What if a property inside the object is null? (e.g., p.Name is null)
// OrderBy DOES NOT throw an exception if the property value is null!
// Instead, it safely places all null values at the VERY BEGINNING 
// of the sorted list (if ascending) or the end (if descending).

// List initialization
 numbers  = [ 44, 22, 55, 666, 9, -6, 345, 11, 3, 3 ];


// Using Contains
Console.WriteLine("List contains 9: " + numbers.Contains(9));


// Using Exists
Console.WriteLine("List contains negative numbers: " + numbers.Exists(n => n < 0));


// Using Find
Console.WriteLine("First negative number: " + numbers.Find(n => n < 0));


// Using FindAll
Console.WriteLine("All negative numbers: " + string.Join(", ", numbers.FindAll(n => n < 0)));


// Using Any
Console.WriteLine("Any numbers greater than 100: " + numbers.Any(n => n > 100));



// A simple list of integers
 numbers = [10, 20, 30];

// ==========================================
// 1. Testing Find() - The Safe and Quiet List Method
// ==========================================

// Searching for 50 (which does NOT exist)
int findResult = numbers.Find(x => x == 50);

// It simply returns the default value of int, which is 0
Console.WriteLine($"Find result: {findResult}"); // Output: 0


// ==========================================
// 2. Testing First() - The Strict LINQ Method
// ==========================================

try
{
    // Searching for 50 using First()
    // This expects at least ONE match. Since there is none, it CRASHES!
    int firstResult = numbers.First(x => x == 50);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"\nFirst() crashed! Exception: {ex.Message}");
    // Output: Sequence contains no matching element
}
// whenever you use the list use find not first or firstordefault

// ==========================================
// 3. Testing FirstOrDefault() - The Safe LINQ Alternative
// ==========================================

// This is the LINQ equivalent of Find()
// If it doesn't find 50, it gracefully returns 0 without crashing.
int safeFirstResult = numbers.FirstOrDefault(x => x == 50);

Console.WriteLine($"\nFirstOrDefault result: {safeFirstResult}"); // Output: 0


// Exists is only for list and much better performance than Any
// since Exists does not require calling the linq library overhead


// converting the list to array

int[] nums = numbers.ToArray();

// converting the array to list

List<int> nList = nums.ToList();


// ==========================================
// 9. PAGINATION (Skip & Take)
// Essential for building Web APIs to return data in chunks (pages).
// ==========================================

// Let's generate a mock list of 25 items using Enumerable.Range
// This creates a list containing numbers from 1 to 25.
List<int> allRecords = Enumerable.Range(1, 25).ToList();

// Variables coming from the user/frontend request
int pageNumber = 2; // The user requested the 2nd page
int pageSize = 10;  // The user wants 10 records per page

// ==========================================
// THE PAGINATION FORMULA
// Skip( (pageNumber - 1) * pageSize ) . Take( pageSize )
// ==========================================
// For Page 1: Skip((1 - 1) * 10) = Skip(0). Then Take(10). -> Returns 1 to 10
// For Page 2: Skip((2 - 1) * 10) = Skip(10). Then Take(10). -> Returns 11 to 20
// For Page 3: Skip((3 - 1) * 10) = Skip(20). Then Take(10). -> Returns 21 to 25

List<int> pageData = allRecords
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();

Console.WriteLine($"\n--- Pagination Results ---");
Console.WriteLine($"Displaying Page {pageNumber} (Size: {pageSize}):");
Console.WriteLine(string.Join(", ", pageData));
// Output: 11, 12, 13, 14, 15, 16, 17, 18, 19, 20


// ==========================================
// 💡 IMPORTANT NOTE ON PERFORMANCE
// ==========================================
// Always use Order() or OrderBy() BEFORE Skip() and Take().
// If you don't sort the data first, the database might return 
// unpredictable results for each page.

List<int> safePagedData = allRecords
    .OrderByDescending(x => x) // 1. Always Sort first!
    .Skip(10)                  // 2. Then Skip
    .Take(5)                   // 3. Then Take
    .ToList();

Console.WriteLine($"\nSafe Sorted Pagination: {string.Join(", ", safePagedData)}");
// Output: 15, 14, 13, 12, 11

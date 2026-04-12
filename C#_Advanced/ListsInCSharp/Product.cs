// 1. Initialization
// 2. Add: Modifies the original list by adding to the end. Returns void.

// NOTE ON APPEND: 
// Append is a LINQ extension. It DOES NOT modify the original list.
// It returns a new IEnumerable<int>. To save it as a list, use .ToList().
// Example: var newList = numbers.Append(5).ToList();

// 3. Insert: Modifies the list by inserting an element at a specific index. Returns void.
// Parameters: (Index, Value)

// 4. InsertRange: Inserts a collection of elements at a specific index. Returns void.

// ==========================================
// 5. REMOVING ELEMENTS
// ==========================================

// A. Remove(value): Searches for the FIRST occurrence of the value and removes it.
// Returns TRUE if successfully removed, FALSE if the item was not found.

// B. RemoveAt(index): Removes the element at the specified index.
// Returns void. (Throws ArgumentOutOfRangeException if index is invalid).

// C. RemoveAll(Predicate): Removes ALL elements that match the given condition.
// Returns an INT representing the total number of elements removed.


// ==========================================
// 6. LOOPING ON ELEMENTS
// ==========================================

// There is one good way to Loop on List Using Lambda expre.



// ==========================================
// 7. FILTERING ON LIST
// ==========================================

// Using .Where() to filter on element
// the second overload of the where we can use the index of the element in the predicate .Where((n , ind) => ind % 2 == 0)


// ==========================================
// 8. SORTING ON LIST
// ==========================================

// Using Sort() returns void and the exception if the list is null
// Sort() by default will sort them in Ascending order 

// Usint Reverse will sort them in descending order 


// ==========================================
// 1. The Default Order() 
// Works perfectly with primitive types like int, double, string, etc.
// ==========================================

// Sorts numbers from lowest to highest
// Output: 1, 5, 10, 20


// Sorts strings alphabetically based on ASCII values 
// (Note: Uppercase letters come before lowercase by default)
// Output: Banana, Zebra, apple, cherry (Because 'B' and 'Z' are uppercase)


// ==========================================
// 2. The Overload: Order(IComparer<T>)
// Used when you want to change HOW things are compared.
// ==========================================

// Let's fix the string sorting issue above! 
// We will pass a built-in comparer that ignores case sensitivity.

// Output: apple, Banana, cherry, Zebra (Now it's properly alphabetical!)


// ==========================================
// 3. OrderDescending() and its Overload
// Exactly the same, but sorts from highest to lowest (or Z to A)
// ==========================================

// Output: 20, 10, 5, 1


public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}


// ==========================================
// 💡 IMPORTANT NOTE ON NULL VALUES IN PROPERTIES
// ==========================================
// What if a property inside the object is null? (e.g., p.Name is null)
// OrderBy DOES NOT throw an exception if the property value is null!
// Instead, it safely places all null values at the VERY BEGINNING 
// of the sorted list (if ascending) or the end (if descending).

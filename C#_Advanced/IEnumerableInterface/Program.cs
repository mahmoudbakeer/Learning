using System;
using System.Collections;
using System.Collections.Generic;

// ==========================================
// CUSTOM COLLECTIONS (IEnumerable & yield)
// Making your own classes work with 'foreach' loops
// ==========================================

public class CustomCollection<T> : IEnumerable<T>
{
    // ==========================================
    // 💡 WHAT IS 'readonly'?
    // ==========================================
    // WHAT: It guarantees that this variable can ONLY be assigned a value 
    //       either right here (during declaration) or inside the Constructor.
    //
    // WHEN: Used for core components of a class that should never be swapped out.
    //
    // WHY:  It prevents bugs. It ensures that no one (not even you by mistake) 
    //       can accidentally do something like: _Collection = new List<T>(); 
    //       somewhere else in the code, which would wipe out all existing data!
    //       The list *contents* can change (Add/Remove), but the list *reference* cannot.
    //
    // 🚨 FIX applied: Instead of 'null!', we actually initialize the list so we can add to it!
    private readonly List<T> _Collection = new List<T>();

    // Made public so we can actually add items from the outside
    public void Add(T item)
    {
        _Collection.Add(item);
    }

    // ==========================================
    // 💡 WHAT IS 'yield return'?
    // ==========================================
    // WHAT: It is a "stateful return". Unlike a normal 'return' that ends the method, 
    //       'yield return' gives a value back to the caller, PAUSES the method, 
    //       and remembers its exact spot. When called again, it resumes from where it left off!
    //
    // WHEN: Used whenever you are implementing an Iterator (like GetEnumerator).
    //
    // WHY:  It saves you from having to write a massive, complex class that tracks 
    //       the "current state" and "current index" of the loop. The compiler builds 
    //       a state machine behind the scenes for you.
    //       It also enables "Lazy Evaluation" (it only gets the next item when the foreach loop asks for it).
    // ==========================================
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _Collection.Count; i++)
        {
            // Returns the current item, pauses here, and waits for the next iteration of 'foreach'
            yield return _Collection[i];
        }

        // 💡 PRO TIP: Because _Collection is a List (which already implements IEnumerable), 
        // you could actually delete the entire 'for' loop above and just write:
        // return _Collection.GetEnumerator();
    }

    // This is required because IEnumerable<T> inherits from the older, non-generic IEnumerable.
    // We just route it to the generic version above.
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// ==========================================
// 🚀 HOW TO USE IT
// ==========================================
class Program
{
    static void Main()
    {
        // 1. Create our custom collection
        CustomCollection<string> myTeam = new CustomCollection<string>();

        // 2. Add some items
        myTeam.Add("Mahmoud");
        myTeam.Add("Ahmad");
        myTeam.Add("Sara");

        Console.WriteLine("--- Iterating through Custom Collection ---");

        // 3. The Magic! We can use 'foreach' because we implemented IEnumerable<T>
        foreach (var member in myTeam)
        {
            Console.WriteLine(member);
        }
    }
}
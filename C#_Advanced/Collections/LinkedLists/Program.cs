using System;
using System.Collections.Generic;

// ==========================================
// 25. LINKEDLIST<T> (DOUBLY LINKED LIST)
// A collection where each element (Node) points to the Next AND Previous element.
// Superpower: Extremely fast Insertions and Deletions in the middle (O(1)) IF you have the Node reference.
// Weakness: No Indexing! You cannot do llist[2]. You must traverse to find an element (O(N)).
// ==========================================

LinkedList<int> llist = new LinkedList<int>();

// ==========================================
// A. BASIC INSERTION (At the ends)
// ==========================================
llist.AddLast(1);
llist.AddLast(2);
llist.AddFirst(3);
llist.AddFirst(4);
llist.AddFirst(5);
llist.AddFirst(6);

// Current Order: 6 -> 5 -> 4 -> 3 -> 1 -> 2
Console.WriteLine("--- Basic LinkedList Traversal ---");
Console.WriteLine($"Elements: {string.Join(" -> ", llist)}");

// ==========================================
// B. BASIC DELETION
// ==========================================
llist.Remove(2); // Searches for '2' (O(N)) and removes it (O(1))
Console.WriteLine($"\nAfter removing '2': {string.Join(" -> ", llist)}");


// ==========================================
// 💡 C. ADVANCED: WORKING WITH LinkedListNode<T>
// This is how you unlock the true O(1) performance of a Linked List!
// ==========================================

Console.WriteLine("\n--- Advanced Node Operations ---");

// 1. Finding a Node (Returns a LinkedListNode<int>, not just the integer)
LinkedListNode<int>? targetNode = llist.Find(4);

if (targetNode != null)
{
    Console.WriteLine($"Target Node found: {targetNode.Value}");

    // We can look at its neighbors without looping!
    Console.WriteLine($"Previous Node: {targetNode.Previous?.Value}"); // 5
    Console.WriteLine($"Next Node: {targetNode.Next?.Value}");         // 3

    // 2. O(1) Insertions using the Node reference!
    // Insert '99' right AFTER '4'
    llist.AddAfter(targetNode, 99);

    // Insert '88' right BEFORE '4'
    llist.AddBefore(targetNode, 88);
}

Console.WriteLine($"\nList after AddAfter and AddBefore around '4':");
Console.WriteLine(string.Join(" -> ", llist));
// Expected: 6 -> 5 -> 88 -> 4 -> 99 -> 3 -> 1

// ==========================================
// D. FAST REMOVALS FROM ENDS
// ==========================================
llist.RemoveFirst(); // Instantly removes '6'
llist.RemoveLast();  // Instantly removes '1'

Console.WriteLine($"\nList after removing First and Last:");
Console.WriteLine(string.Join(" -> ", llist));

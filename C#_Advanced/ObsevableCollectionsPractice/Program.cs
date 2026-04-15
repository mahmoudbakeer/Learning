using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

// ==========================================
// 24. OBSERVABLECOLLECTION<T> & EVENTS
// A collection that notifies listeners (fires events) when items get added, removed, or refreshed.
// Extremely useful for Real-Time data tracking!
// ==========================================

class Program
{
    static void Main()
    {
        // 1. Initialization
        ObservableCollection<string> liveUsers = new ObservableCollection<string>();

        // 2. Wiring up the Event Handler
        // We tell the collection: "Whenever you change, run this method!"
        liveUsers.CollectionChanged += Items_CollectionChanged;

        Console.WriteLine("--- Testing ObservableCollection ---");

        // 3. Triggering the "Add" Action
        liveUsers.Add("Mahmoud");
        liveUsers.Add("Ahmad");

        // 4. Triggering the "Replace" Action
        liveUsers[1] = "Ali"; // Ahmad leaves, Ali takes his place

        // 5. Triggering the "Move" Action (Moving Mahmoud from index 0 to index 1)
        liveUsers.Move(0, 1);

        // 6. Triggering the "Remove" Action
        liveUsers.Remove("Mahmoud");
    }

    // ==========================================
    // THE EVENT HANDLER (Your Code, slightly fortified)
    // ==========================================
    static void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine("\n[ALERT] Collection Changed!");

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                Console.WriteLine("Action: ADDED");
                // Added a null check (?) to prevent crashes
                if (e.NewItems != null)
                {
                    foreach (var newItem in e.NewItems)
                        Console.WriteLine($"  + {newItem}");
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                Console.WriteLine("Action: REMOVED");
                if (e.OldItems != null)
                {
                    foreach (var oldItem in e.OldItems)
                        Console.WriteLine($"  - {oldItem}");
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                Console.WriteLine("Action: REPLACED");
                if (e.OldItems != null && e.NewItems != null)
                {
                    // Assuming 1-to-1 replacement for simplicity
                    Console.WriteLine($"  ~ Out: {e.OldItems[0]} | In: {e.NewItems[0]}");
                }
                break;

            case NotifyCollectionChangedAction.Move:
                Console.WriteLine("Action: MOVED");
                Console.WriteLine($"  > Item moved from index [{e.OldStartingIndex}] to [{e.NewStartingIndex}]");
                break;

            case NotifyCollectionChangedAction.Reset:
                // Triggered when .Clear() is called
                Console.WriteLine("Action: RESET (Collection Cleared)");
                break;
        }
    }
}
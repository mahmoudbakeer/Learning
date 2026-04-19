using System;
using System.Collections;
using System.Collections.Generic;

public class MyCustomCollection<T> : IList<T>
{
    // Start with an empty array
    private T[] itemsCollection = Array.Empty<T>();

    // 'Size' represents the actual number of elements, not the total array capacity
    private int Size = 0;

    public int Count => Size;

    // Since we can add and remove items, the collection is not read-only
    public bool IsReadOnly => false;

    // --- NEW: Indexer ---
    // This allows you to access elements using brackets, e.g., myCollection[0]
    public T this[int index]
    {
        get
        {
            // Validate the index before accessing the array
            if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(nameof(index));
            return itemsCollection[index];
        }
        set
        {
            // Validate the index before modifying the array
            if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(nameof(index));
            itemsCollection[index] = value;
        }
    }

    public void Add(T item)
    {
        // If the array is full, create a new array with double the capacity
        if (Size == itemsCollection.Length)
        {
            int newCapacity = itemsCollection.Length == 0 ? 4 : itemsCollection.Length * 2;
            Array.Resize(ref itemsCollection, newCapacity);
        }

        // Add the item to the next available slot, then increment the size
        itemsCollection[Size] = item;
        Size++;
    }

    public void Clear()
    {
        // Clear the data by resetting the array and size
        itemsCollection = Array.Empty<T>();
        Size = 0;
    }

    public bool Contains(T item)
    {
        // Now that we have IndexOf, we can simplify Contains!
        return IndexOf(item) >= 0;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < Size) throw new ArgumentException("The destination array is too small.");

        Array.Copy(itemsCollection, 0, array, arrayIndex, Size);
    }

    // --- NEW: IndexOf ---
    // Returns the exact position of an item, or -1 if it doesn't exist
    public int IndexOf(T item)
    {
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < Size; i++)
        {
            if (comparer.Equals(itemsCollection[i], item)) return i;
        }
        return -1;
    }

    // --- NEW: Insert ---
    // Pushes an item into a specific position and shifts everything else to the right
    public void Insert(int index, T item)
    {
        // Notice we allow index == Size, which is the exact same behavior as Add()
        if (index < 0 || index > Size) throw new ArgumentOutOfRangeException(nameof(index));

        // 1. Ensure we have enough capacity
        if (Size == itemsCollection.Length)
        {
            int newCapacity = itemsCollection.Length == 0 ? 4 : itemsCollection.Length * 2;
            Array.Resize(ref itemsCollection, newCapacity);
        }

        // 2. If we aren't adding to the very end, shift elements to the right
        if (index < Size)
        {
            Array.Copy(itemsCollection, index, itemsCollection, index + 1, Size - index);
        }

        // 3. Insert the new item into the opened slot
        itemsCollection[index] = item;
        Size++;
    }

    // --- NEW: RemoveAt ---
    // Deletes an item at a specific position and shifts the rest to the left
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(nameof(index));

        Size--;

        // If we aren't removing the very last item, shift elements to the left to close the gap
        if (index < Size)
        {
            Array.Copy(itemsCollection, index + 1, itemsCollection, index, Size - index);
        }

        // Clear the last element's reference to prevent memory leaks
        itemsCollection[Size] = default!;
    }

    // REFACTORED: Remove now uses IndexOf and RemoveAt to avoid duplicating logic! (DRY Principle)
    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Size; i++)
        {
            yield return itemsCollection[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
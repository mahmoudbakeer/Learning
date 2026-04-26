using System;
using System.Collections;
using System.Collections.Generic;

public class MyCustomCollection<T> : ICollection<T>
{
    // Start with an empty array
    private T[] itemsCollection = Array.Empty<T>();

    // 'Size' represents the actual number of elements, not the total array capacity
    private int Size = 0;

    public int Count => Size;

    // Since we can add and remove items, the collection is not read-only
    public bool IsReadOnly => false;

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
        // Use EqualityComparer for safe comparison to avoid NullReferenceException
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        for (int i = 0; i < Size; i++)
        {
            if (comparer.Equals(itemsCollection[i], item)) return true;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < Size) throw new ArgumentException("The destination array is too small.");

        // The most efficient and standard way to copy arrays in C#
        Array.Copy(itemsCollection, 0, array, arrayIndex, Size);
    }

    public bool Remove(T item)
    {
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;

        for (int i = 0; i < Size; i++)
        {
            if (comparer.Equals(itemsCollection[i], item))
            {
                // If the item is found, shift all subsequent elements one step to the left to fill the gap
                Size--;
                Array.Copy(itemsCollection, i + 1, itemsCollection, i, Size - i);

                // Clear the last element's reference to prevent memory leaks
                itemsCollection[Size] = default!;
                return true;
            }
        }
        return false;
    }

    // The proper way to iterate over the elements using 'yield return'
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
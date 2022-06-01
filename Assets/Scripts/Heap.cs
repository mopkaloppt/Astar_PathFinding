using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    // T is a generic type array
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        // Always add a new item to the last spot in the array
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        // Put last item in the first spot
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int LeftChildIndex = item.HeapIndex * 2 + 1;
            int RightChildIndex = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            // If the parent has at least one child
            if (LeftChildIndex < currentItemCount)
            {
                // If it gets here, there is Left child -- save Left Child index to compare with Right child index below
                swapIndex = LeftChildIndex;

                if (RightChildIndex < currentItemCount)
                {
                    // If Left child has lower priority (i.e. higher fCost), then swap Left child with Right child
                    if (items[LeftChildIndex].CompareTo(items[RightChildIndex]) < 0)
                    {
                        swapIndex = RightChildIndex;
                    }
                }
                // Now Swap item with the highest priority child, if child has higher priority
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    // If child has lower priority, don't swap, return the item as is.
                    return;
                }
            }
            else
            {
                // The parent doesn't have any children.
                return;
            }
        }    
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];

            // If item has higher priority than the parent item (i.e. lower fCost)
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            // item here has been swapped
            parentIndex = (item.HeapIndex - 1) / 2;      
        }
    }

    void Swap(T item, T parentItem)
    {
        // Swap items 
        items[parentItem.HeapIndex] = item;
        items[item.HeapIndex] = parentItem;

        // Swap indices
        int parentItemIndex = parentItem.HeapIndex;
        parentItem.HeapIndex = item.HeapIndex;
        item.HeapIndex = parentItemIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
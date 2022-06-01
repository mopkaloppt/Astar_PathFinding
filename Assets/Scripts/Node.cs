using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int Xpos; // for Node to keep track of its own position
    public int Ypos; // for Node to keep track of its own position

    public int gCost;
    public int hCost;
    public Node parent;

    public int heapIndex;

    public Node (bool _walkable, Vector3 _worldPos, int _Xpos, int _Ypos)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        Xpos = _Xpos;
        Ypos = _Ypos;
    }

    public int fCost { get { return gCost + hCost; } }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        // fCost.CompareTo(x) normally returns 1 if fCost is higher than x
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            // If equal, use hCost as a tie-breaker
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        // In our case we see lower fCost as a higher priority, so we return -1
        return -compare;
    }
}

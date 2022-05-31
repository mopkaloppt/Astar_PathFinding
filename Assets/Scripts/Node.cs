using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector3 worldPosition;
    public int Xpos; // for Node to keep track of its own position
    public int Ypos; // for Node to keep track of its own position

    public int gCost;
    public int hCost;
    public Node parent;

    public Node (bool _walkable, Vector3 _worldPos, int _Xpos, int _Ypos)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        Xpos = _Xpos;
        Ypos = _Ypos;
    }

    public int fCost { get { return gCost + hCost; } }
}

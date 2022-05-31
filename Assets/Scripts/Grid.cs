using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize; // area in world coords that grid covers
    public float nodeRadius; // how much space each node covers
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        // transfor.position == center
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        // Loop through all nodes in grid to check for collision
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                grid[x,y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        // We are searching neighbor nodes in a 3x3 block
        //      _ _ _
        //     |_|_|_|
        //     |_|0|_|
        //     |_|_|_|
        // 0 is where the currentNode is.   
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    // We are at the currentNode, so skip
                    continue;
                                
                int checkXpos = node.Xpos + x;
                int checkYpos = node.Ypos + y;

                if (checkXpos >= 0 && checkXpos < gridSizeX && checkYpos >= 0 && checkYpos < gridSizeY)
                    neighbors.Add(grid[checkXpos, checkYpos]);     
            }
        }
        return neighbors;
    }
    
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        // In 3D world z in our y direction for 2D grid, hence worldPosition.z
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        // Must Clamp the value between (0,1) because we don't want player to fall of the grid
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        return grid[x,y];

    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        if (grid != null)
        {
            Node playerNode = GetNodeFromWorldPoint(player.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable)? Color.white : Color.red;
                // Test A* Pathfinding code
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.green;
                    
                // Test Grid Building code
                // if (playerNode == n)
                // {
                //     Gizmos.color = Color.green;
                // }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}

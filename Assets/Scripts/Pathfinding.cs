using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // To check Heap performance
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        // List<Node> openSet = new List<Node>();
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            //Node currentNode = openSet[0];
            Node currentNode = openSet.RemoveFirst();
            //Don't need this anymore for Heap Optimization, it's now done in openSet.RemoveFirst();
            // for (int i = 1; i < openSet.Count; i++)
            // {
            //     if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
            //     {
            //         currentNode = openSet[i];                 
            //     }  
            // }
            // openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                // Path found, stop the timer
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    // Skip to next neighbor
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < currentNode.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(targetNode, startNode);
                    // Check the parent of the neighbor next
                    neighbor.parent = currentNode;
                    // if the neighbor of the new currentNode is not in the openSet, add it
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }                    
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            // Backtrace the node the currentNode just came from (parent node)
            currentNode = currentNode.parent;
        }
        path.Reverse();
        // Test
        grid.path = path;
    }

    int GetDistance(Node startNode, Node targetNode)
    {
        int distanceX = Mathf.Abs(startNode.Xpos - targetNode.Xpos);
        int distanceY = Mathf.Abs(startNode.Ypos - targetNode.Ypos);

        if (distanceX > distanceY)
            return 14*distanceY + 10 * (distanceX - distanceY);
        return 14*distanceX + 10 * (distanceY - distanceX);
    }
}

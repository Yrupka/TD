using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind
{
    private const int MOVE_STRAIGHT = 10;
    private const int MOVE_DIAGONAL = 14;

    private class Node
    {
        public int x;
        public int y;

        public int gCost;
        public int hCost;
        public int fCost;
        public bool isWalkable;

        public Node lastNode;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    private Node[,] grid;
    private List<Node> openList;
    private HashSet<Node> closeList;
    private GridMap gridMap;

    public Pathfind(GridMap grid)
    {
        gridMap = grid;
        this.grid = new Node[grid.GetWidth(), grid.GetHeight()];
    }

    public List<Vector3> FindPath(Vector3Int start, Vector3Int end)
    {
        // gridMap.GetXZ(start, out int startX, out int startY);
        // gridMap.GetXZ(end, out int endX, out int endY);
        List<Node> path = FindPath(start.x, start.y, end.x, end.y);
        if (path == null)
            return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (var node in path)
            {
                vectorPath.Add(gridMap.GetWorldPos(node.x, node.y));
            }
            return vectorPath;
        }

    }

    private List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        openList = new List<Node>();
        closeList = new HashSet<Node>();

        for (int x = 0; x < gridMap.GetWidth(); x++)
        {
            for (int y = 0; y < gridMap.GetHeight(); y++)
            {
                Node node = new Node(x, y);
                node.isWalkable = gridMap.CanWalk(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.lastNode = null;

                grid[x, y] = node;
            }
        }

        Node startNode = grid[startX, startY];
        Node endNode = grid[endX, endY];
        openList.Add(startNode);
        
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            Node current = GetLowestFCostNode(openList);
            if (current.x == endNode.x && current.y == endNode.y)
                return CalculatePath(endNode);
                
        
            openList.Remove(current);
            closeList.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closeList.Contains(neighbor)) continue;
                if (!neighbor.isWalkable)
                {
                    closeList.Add(neighbor);
                    continue;
                }

                int gCost = current.gCost + CalculateDistanceCost(current, neighbor);
                if (gCost < neighbor.gCost)
                {
                    neighbor.lastNode = current;
                    neighbor.gCost = gCost;
                    neighbor.hCost = CalculateDistanceCost(neighbor, endNode);
                    neighbor.CalculateFCost();

                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }
        return null;
    }

    // получаем соседей, в этом порядке:
    // 3 6 9
    // 2 x 8
    // 1 4 7
    private List<Node> GetNeighbors(Node current)
    {
        List<Node> neighbors = new List<Node>();
        
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                // центр - это сама ячейка
                if (xOffset == 0 && yOffset == 0)
                    continue;
                // выход за границе по одной из координат
                if ((current.x + xOffset < 0 || current.x + xOffset > gridMap.GetWidth()) ||
                    (current.y + yOffset < 0 || current.y + yOffset > gridMap.GetHeight()))
                    continue;
                neighbors.Add(grid[current.x + xOffset, current.y + yOffset]);
            }
        }
        return neighbors;
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node current = endNode;
        while(current.lastNode != null)
        {
            path.Add(current.lastNode);
            current = current.lastNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL * Mathf.Min(xDist, yDist) +
            MOVE_STRAIGHT * remaining;
    }

    private Node GetLowestFCostNode(List<Node> list)
    {
        Node lowestF = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].fCost < lowestF.fCost)
                lowestF = list[i];
        }
        return lowestF;
    }
}

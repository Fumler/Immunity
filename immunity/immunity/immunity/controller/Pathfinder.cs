﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace immunity
{
    internal class Pathfinder
    {
        private SearchNode[,] searchNodes;
        private int width;
        private int height;
        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        private float Heuristic(Point start, Point end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        /// <summary>
        /// Creates searchNodes for all the tiles and connect them to their neighbours
        /// </summary>
        private void InitializeSearchNodes(Map map)
        {
            searchNodes = new SearchNode[width, height];

            //Create nodes
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SearchNode node = new SearchNode(new Point(x, y), map);

                    if (node.walkable == true)
                    {
                        node.neighbors = new SearchNode[4];
                        searchNodes[x, y] = node;
                    }
                }
            }

            //Connect nodes to neighbours
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    // If not relevant node, skip to next.
                    if (node == null || node.walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbours = new Point[] {
                        new Point (x, y - 1), // The node above the current node.
                        new Point (x, y + 1), // The node below the current node.
                        new Point (x - 1, y), // The node left of the current node.
                        new Point (x + 1, y), // The node right of the current node.
                    };

                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        Point position = neighbours[i];

                        // If position is not valid, skip to next.
                        if (position.X < 0 || position.X > width - 1 || position.Y < 0 || position.Y > height - 1)
                        {
                            continue;
                        }

                        SearchNode neighbour = searchNodes[position.X, position.Y];

                        // If unit can not walk on node, skip to next.
                        if (neighbour == null || neighbour.walkable == false)
                        {
                            continue;
                        }

                        node.neighbors[i] = neighbour;
                    }
                }
            }
        }

        /// <summary>
        /// Resets the search nodes, clearing lists and distance variables of all nodes.
        /// </summary>
        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.inOpenList = false;
                    node.inClosedList = false;

                    node.distanceTraveled = float.MaxValue;
                    node.distanceToGoal = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Finds the node in the openList that is closest to the goal.
        /// </summary>
        private SearchNode FindBestNode()
        {
            SearchNode currentNode = openList[0];
            float smallestDistanceToGoal = float.MaxValue;

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].distanceToGoal < smallestDistanceToGoal)
                {
                    currentNode = openList[i];
                    smallestDistanceToGoal = currentNode.distanceToGoal;
                }
            }

            return currentNode;
        }

        /// <summary>
        /// After arriving at the end node, follow path back from endNode to find shortest path.
        /// </summary>
        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            List<Vector2> finalPath = new List<Vector2>();
            closedList.Add(endNode);
            SearchNode parentNode = endNode.parent;

            // Add nodes in path to the closedList in reverse order.
            while (parentNode != startNode)
            {
                closedList.Add(parentNode);
                parentNode = parentNode.parent;
            }

            // Add nodes to final path as coordinates in the correct order.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].position.X * Map.TILESIZE, closedList[i].position.Y * Map.TILESIZE));
            }

            return finalPath;
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            ResetSearchNodes();

            SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];
            SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];

            startNode.inOpenList = true;

            startNode.distanceToGoal = Heuristic(startPoint, endPoint);
            startNode.distanceTraveled = 0;

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                SearchNode currentNode = FindBestNode();

                if (currentNode == null)
                {
                    break;
                }

                if (currentNode == endNode)
                {
                    // Trace the path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                for (int i = 0; i < currentNode.neighbors.Length; i++)
                {
                    SearchNode neighbor = currentNode.neighbors[i];

                    if (neighbor == null || neighbor.walkable == false)
                    {
                        continue;
                    }

                    float distanceTraveled = currentNode.distanceTraveled + 1;
                    float neighbourHeuristic = Heuristic(neighbor.position, endPoint);

                    if (neighbor.inOpenList == false && neighbor.inClosedList == false)
                    {
                        neighbor.distanceTraveled = distanceTraveled;
                        neighbor.distanceToGoal = distanceTraveled + neighbourHeuristic;
                        neighbor.parent = currentNode;
                        neighbor.inOpenList = true;
                        openList.Add(neighbor);
                    }
                    else if (neighbor.inOpenList || neighbor.inClosedList)
                    {
                        if (neighbor.distanceTraveled > distanceTraveled)
                        {
                            neighbor.distanceTraveled = distanceTraveled;
                            neighbor.distanceToGoal = distanceTraveled + neighbourHeuristic;
                            neighbor.parent = currentNode;
                        }
                    }
                }

                openList.Remove(currentNode);
                currentNode.inClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }

        public Pathfinder(Map map)
        {
            width = map.Width;
            height = map.Height;

            InitializeSearchNodes(map);
        }
    }
}
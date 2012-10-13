using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace immunity
{
    class SearchNode
    {
        public Point position;
        public bool walkable;
        public SearchNode[] neighbors;
        public SearchNode parent;
        public bool inOpenList;
        public bool inClosedList;
        public float distanceToGoal;
        public float distanceTraveled;

        public SearchNode(Point coords, Map map) {
            position = coords;
            walkable = map.getIndex(position.X, position.Y) == 0;
        }
    }
}

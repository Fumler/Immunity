using Microsoft.Xna.Framework;

namespace immunity
{
    internal class SearchNode
    {
        public Point position;
        public bool walkable;
        public SearchNode[] neighbors;
        public SearchNode parent;
        public bool inOpenList;
        public bool inClosedList;
        public float distanceToGoal;
        public float distanceTraveled;

        public SearchNode(Point coords, Map map)
        {
            position = coords;
            walkable = map.GetIndex(position.X, position.Y) == 0;
        }
    }
}
using Microsoft.Xna.Framework;

namespace immunity
{
    internal class SearchNode
    {
        //Variables
        //Position
        public Point position;
        public float distanceToGoal;
        public float distanceTraveled;
        
        //Relations
        public SearchNode[] neighbors;
        public SearchNode parent;

        //Connection to lists
        public bool inOpenList;
        public bool inClosedList;

        //Unit interaction
        public bool walkable;

        //Constructors
        /// <summary>
        /// Creates a new SearchNode.
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="map"></param>
        public SearchNode(Point coords, Map map)
        {
            position = coords;
            walkable = map.GetIndex(position.X, position.Y) == 0;
        }
    }
}
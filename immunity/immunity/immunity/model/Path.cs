using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace immunity
{
    internal class Path
    {
        //Variables
        private List<Vector2> travelPath;

        //Accessors
        public List<Vector2> TravelPath
        {
            get { return travelPath; }
            set { travelPath = value; }
        }

        //Constructors
        public Path()
        {
        }

        //Methods
        /// <summary>
        /// Updates the internal travelPath.
        /// </summary>
        /// <param name="pathfinder"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void GetPath(Pathfinder pathfinder, Point start, Point end)
        {
            travelPath = pathfinder.FindPath(start, end);
        }

        /// <summary>
        /// Checks if the next step in the path is valid.
        /// </summary>
        /// <param name="currentStep">units current step in the path.</param>
        /// <returns></returns>
        private bool IsValid(int currentStep)
        {
            if (currentStep + 1 > travelPath.Count - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the next position in the path.
        /// </summary>
        /// <param name="currentStep">Units current step in the path.</param>
        /// <returns>Return the next step if valid; -1,-1 if invalid.</returns>
        public Vector2 GetNextStep(int currentStep)
        {
            if (IsValid(currentStep))
            {
                return travelPath.ElementAt(currentStep + 1);
            }
            else
            {
                return new Vector2(-1f, -1f);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace immunity
{
    class Path
    {
        private List<Vector2> path;

        public void getPath(Pathfinder pathfinder, Point start, Point end) {
            path = pathfinder.FindPath(start, end);
        }

        private bool isValid(int currentStep)
        {
            if (currentStep+1 > path.Count-1) {
                return false;
            } else {
                return true;
            }
        }

        public Vector2 getNextStep(int currentStep) {
            if (isValid(currentStep)) {
                return path.ElementAt(currentStep+1);
            } else {
                return new Vector2(-1f, -1f);
            }
        }

        public Path() {

        }
    }
}

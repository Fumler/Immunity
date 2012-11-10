using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Unit
    {
        //Variables
        //Statics
        private static Path path = new Path();
        private static List<Texture2D> sprites;

        //Location
        private Vector2 unitPosition;
        private Vector2 moveToPosition;
        private Vector2 center;

        //Data
        private int health;
        private int speed;
        private int unitType;
        private int positionInPath;

        //Accessors
        public Vector2 Center
        {
            get { return center; }
        }

        //Constructors
        public Unit(int unitType)
        {
            positionInPath = 1;
            this.unitType = unitType;
            switch (unitType)
            {
                case 0:
                    health = 100;
                    speed = 2;
                    break;

                case 1:
                    health = 200;
                    speed = 4;
                    break;
            }
        }

        //Static methods
        public static void LoadPath(Pathfinder pathfinder, Point start, Point end)
        {
            path.getPath(pathfinder, start, end);
        }

        public static void SetSprites(List<Texture2D> textures)
        {
            sprites = textures;
        }

        public static List<Vector2> getPath()
        {
            return path.getPath();
        }

        //Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[unitType], new Vector2(unitPosition.X, unitPosition.Y + 24), Color.White);
        }

        public int NextNode()
        {
            return positionInPath;
        }

        public void Update()
        {
            if (unitPosition != moveToPosition)
            {
                if (moveToPosition.X != unitPosition.X)
                {
                    if (moveToPosition.X > unitPosition.X)
                    {
                        unitPosition.X += speed;
                    }
                    else
                    {
                        unitPosition.X -= speed;
                    }
                }
                else
                {
                    if (moveToPosition.Y > unitPosition.Y)
                    {
                        unitPosition.Y += speed;
                    }
                    else
                    {
                        unitPosition.Y -= speed;
                    }
                }
            }
            else
            {
                moveToPosition = path.getNextStep(positionInPath);

                if (moveToPosition == new Vector2(-1f, -1f))
                {
                    moveToPosition = unitPosition;
                }
                else
                {
                    positionInPath++;
                }
            }
        }
    }
}
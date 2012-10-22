using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class Unit
    {
        private static Path path = new Path();
        
        private List<Texture2D> sprites;

        private Vector2 unitPosition;
        private Vector2 moveToPosition;

        private int health;
        private int speed;
        private int unitType;
        private int positionInPath;

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

        public static void loadPath(Pathfinder pathfinder, Point start, Point end)
        {
            path.getPath(pathfinder, start, end);
        }

        public void setSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[unitType],new Vector2(unitPosition.X,unitPosition.Y),Color.White);
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


                if (moveToPosition == new Vector2(-1f, -1f)) {
                    moveToPosition = unitPosition;
                } else {
                    positionInPath++;
                }
            }
        }
    }
}

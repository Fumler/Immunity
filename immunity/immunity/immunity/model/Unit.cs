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
        private List<Texture2D> sprites;

        private Vector2 unitPosition,moveToPosition;
        private int health,speed,unitType, positionInPath;


        private List<Vector2> path;
        //private static Path path;

        public Unit(int unitType)
        {
            path = new List<Vector2>();
            positionInPath = 0;
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
            path.Add(new Vector2(64, 0));
            path.Add(new Vector2(64, 64));
            path.Add(new Vector2(0, 128));
            path.Add(new Vector2(32, 128));
            path.Add(new Vector2(32, 160));
            path.Add(new Vector2(32*3, 128));
            path.Add(new Vector2(0,0));
        
        }

        public void setSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[0],new Vector2(unitPosition.X,unitPosition.Y),Color.White);
        }

        public void Update()
        {
            System.Diagnostics.Debug.WriteLine("x=" +unitPosition.X + " y=" + unitPosition.Y);
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
            else {
                if(positionInPath < path.Count)
                moveToPosition = path[positionInPath++];
            }
        }

        /*public void setPath(Path newPath) {
            path = newPath.getPath();
        }
        */
    }
}

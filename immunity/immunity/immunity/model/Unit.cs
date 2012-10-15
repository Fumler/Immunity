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
        private List<Vector2> path;
        private Point unitPosition,moveToPosition;
        private int health,speed,unitType;

        public Unit(int unitType)
        {
            this.unitType = unitType;
            moveToPosition = new Point(0,128);
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
                    if (moveToPosition.X > unitPosition.X){
                        unitPosition.X += speed;
                    } else {
                        unitPosition.X -= speed;
                    }
                }
                else
                {
                    if (moveToPosition.Y > unitPosition.Y) {
                        unitPosition.Y += speed;
                    }
                    else
                    {
                        unitPosition.Y -= speed;
                    }
                }
            }
        }

        public void setPosition(Point set) {
            this.unitPosition = set;
        }

    }
}

using System;
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
        private static SpriteFont font;

        //Location
        private Vector2 unitPosition;
        private Vector2 moveToPosition;
        private Vector2 center;

        //Data
        private int health;
        private bool alive = true;
        private int speed;
        private int unitType;
        private int positionInPath;

        // Events
        public static event EventHandler onDeath, onLastTile;

        //Accessors
        public Vector2 Center
        {
            get { return center; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public bool IsDead
        {
            get { return health < 1/* || 'at end of path'*/; }
        }

        //Constructors
        /// <summary>
        /// Creates a new unit.
        /// </summary>
        /// <param name="unitType">The type of unit you want ot create.</param>
        public Unit(int unitType)
        {
            positionInPath = 1;
            this.unitType = unitType;
            switch (unitType)
            {
                case 0:
                    health = 100;
                    speed = 1;
                    break;

                case 1:
                    health = 200;
                    speed = 2;
                    break;
            }
        }
        //Static methods
        /// <summary>
        /// Updates the path the units follow.
        /// </summary>
        /// <param name="pathfinder"></param>
        /// <param name="start">Start node.</param>
        /// <param name="end">End node</param>
        public static void LoadPath()
        {
            path.GetPath();
        }

        /// <summary>
        /// Set the sprite list for Units.
        /// </summary>
        /// <param name="textures"></param>
        public static void SetSprites(List<Texture2D> textures, SpriteFont theFont)
        {
            sprites = textures;
            font = theFont;
        }

        public static void SetPathfinder(Pathfinder pathfinder, Map map)
        {
            path.Start = map.Start;
            path.End = map.End;
            path.PFinder = pathfinder;
        }

        /// <summary>
        /// Returns the path the units follow.
        /// </summary>
        /// <returns></returns>
        public static List<Vector2> Path
        {
            get { return path.TravelPath; }
            set { path.TravelPath = value; }
        }

        //Methods
        public void OnBulletHit(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                onDeath(this, EventArgs.Empty);
            }
        }

        public bool IsOnLastTile()
        {
            if (this.unitPosition.X == path.End.X * 32 && this.unitPosition.Y == (path.End.Y * 32))
            {
                this.health = 0;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Draws the unit.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.DrawString(font, this.health.ToString(), new Vector2(unitPosition.X, unitPosition.Y), Color.White);
                spriteBatch.Draw(sprites[unitType], new Vector2(unitPosition.X, unitPosition.Y + 24), Color.White);
            }
        }

        /// <summary>
        /// Updates the unit.
        /// </summary>
        public void Update()
        {
            if (Health <= 0)
            {
                alive = false;
            }

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
                if (IsOnLastTile())
                {
                    onLastTile(this, EventArgs.Empty);
                }
                else
                {
                    moveToPosition = path.GetNextStep(positionInPath);
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

            center = new Vector2(unitPosition.X + (sprites[unitType].Width / 2), unitPosition.Y + (sprites[unitType].Height / 2) + 24);
        }
    }
}
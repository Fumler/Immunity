using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Unit : ScreenObject
    {
        //Variables
        //Statics
        private static Path path = new Path();

        private static List<Texture2D> sprites;
        private static SpriteFont font;

        //Location
        private int positionInPath;

        private Vector2 moveToPosition;

        //Data
        private bool alive = true;

        private int health;
        private int speed;

        // Events
        public static event EventHandler onDeath, onLastTile;

        //Accessors
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
        /// <param name="type">The type of unit you want ot create.</param>
        public Unit(int type, int healthModifier)
            : base(Unit.sprites[type])
        {
            positionInPath = 1;
            this.type = type;
            switch (type)
            {
                case 0:
                    health = (int)(100 * (healthModifier * 1.30f));
                    speed = 2;
                    break;

                case 1:
                    health = (int)(75 * (healthModifier * 1.30f));
                    speed = 4;
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
            if (this.position.X == path.End.X * 32 && this.position.Y == (path.End.Y * 32))
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.DrawString(font, this.health.ToString(), new Vector2(position.X, position.Y), Color.White);
                spriteBatch.Draw(texture, new Vector2(position.X, position.Y + 24), Color.White);
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
                return;
            }

            if (position != moveToPosition)
            {
                if (moveToPosition.X != position.X)
                {
                    if (moveToPosition.X > position.X)
                    {
                        position.X += speed;
                    }
                    else
                    {
                        position.X -= speed;
                    }
                }
                else
                {
                    if (moveToPosition.Y > position.Y)
                    {
                        position.Y += speed;
                    }
                    else
                    {
                        position.Y -= speed;
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
                        moveToPosition = position;
                    }
                    else
                    {
                        positionInPath++;
                    }
                }
            }

            center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2) + 24);
        }
    }
}
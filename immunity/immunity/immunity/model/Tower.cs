using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Tower : ScreenObject
    {
        protected List<Ammunition> ammunitionList;
        protected int cellPositionX, cellPositionY;
        protected int cost;
        protected int damage;
        protected int level;
        protected int range;
        protected float fireDelay;
        protected int ammunitionSpeed;
        protected float ammunitionTimer;
        private static Texture2D turret;

        protected Unit target;

        public int Cost
        {
            get { return cost; }
        }

        public static Texture2D Turret
        {
            set { turret = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { this.type = value; }
        }

        public int Level
        {
            get { return level; }
        }

        public int Range
        {
            get { return range; }
        }

        public Unit Target
        {
            get { return target; }
        }

        public Tower(int type, int cellX, int cellY)
            : base(turret)
        {
            cellPositionX = cellX;
            cellPositionY = cellY;
            ammunitionList = new List<Ammunition>();
            rotation = 0.0f;
            range = 200;
            ammunitionTimer = 0;
            UpdateAttributes(type);
            center = new Vector2(((cellPositionX * Map.TILESIZE) + Map.TILESIZE / 2), ((cellPositionY * Map.TILESIZE) + Map.TILESIZE / 2) + 24);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public static int GetCost(int type)
        {
            switch (type)
            {
                case 1:
                    return 10;


                case 10:
                    return 100;

                case 11:
                    return 150;

                case 20:
                    return 200;

                case 21:
                    return 250;

                default:
                    return 0;
            }
        }

        private void UpdateAttributes(int type)
        {
            this.type = type;
            switch (type)
            {
                case 1:
                    fireDelay = 0;
                    ammunitionSpeed = 0;
                    damage = 0;
                    ammunitionList.Clear();
                    range = 0;
                    break;
                case 10:
                    fireDelay = 1;
                    ammunitionSpeed = 5;
                    damage = 50;
                    break;

                case 11:
                    fireDelay = 0.9f;
                    ammunitionSpeed = 6;
                    damage = 75;
                    break;

                case 20:
                    fireDelay = 0.5f;
                    ammunitionSpeed = 10;
                    damage = 30;
                    break;

                case 21:
                    fireDelay = 0.4f;
                    ammunitionSpeed = 20;
                    damage = 40;
                    break;
            }
        }

        protected void GetClosestEnemy(ref List<Unit> enemies)
        {
            target = null;
            float shortestRange = range;
            foreach (Unit enemy in enemies)
            {
                if (Vector2.Distance(center, enemy.Center) < shortestRange)
                {
                    shortestRange = Vector2.Distance(center, enemy.Center);
                    target = enemy;
                }
            }
        }

        protected bool IsInRange(Vector2 targetPosition)
        {
            if (Vector2.Distance(center, targetPosition) <= range)
            {
                return true;
            }

            return false;
        }

        protected void FaceTarget()
        {
            Vector2 direction = center - target.Center;
            direction.Normalize();
            rotation = (float)Math.Atan2(-direction.X, direction.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ammunition ammunition in ammunitionList)
            {
                ammunition.Draw(spriteBatch);
            }
            spriteBatch.Draw(texture, center, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }

        public void Update(ref List<Unit> enemies, GameTime gameTime)
        {
            ammunitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            GetClosestEnemy(ref enemies);

            if (target != null)
            {
                FaceTarget();

                if (ammunitionTimer >= fireDelay)
                {
                    Ammunition ammunition = new Ammunition(type, center, rotation, ammunitionSpeed, damage, ref target);
                    ammunition.SetRotation(rotation);
                    ammunitionList.Add(ammunition);
                    ammunitionTimer = 0;
                }
            }
            else
            {
                ammunitionTimer = 0;
            }

            for (int i = 0; i < ammunitionList.Count; i++)
            {
                ammunitionList[i].Update(ref enemies);

                if (!IsInRange(ammunitionList[i].Center))
                {
                    ammunitionList[i].Kill();
                }

                if (ammunitionList[i].IsDead())
                {
                    ammunitionList.Remove(ammunitionList[i]);
                    i--;
                }
            }
        }
    }
}
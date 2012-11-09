using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class Tower
    {
        protected List<Ammunition> ammunitionList;
        protected Vector2 position;
        protected Vector2 center;
        protected float rotation;
        protected int type;
        protected int cost;
        protected int damage;
        protected int level;
        protected int range;
        protected float fireRate;
        protected int ammunitionSpeed;
        protected float ammunitionTimer;

        protected Unit target;

        public int Type
        {
            get { return type; }
        }

        public int Cost
        {
            get { return cost; }
        }

        public int Damage
        {
            get { return damage; }
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

        public Tower()
        {
            ammunitionList = new List<Ammunition>();
            rotation = 0.0f;
            fireRate = 1;
            ammunitionSpeed = 1;
            ammunitionTimer = 0;
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

        public void Shoot()
        {
            Ammunition temp = new Ammunition(0, center, rotation, ammunitionSpeed, damage);

        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Ammunition ammunition in ammunitionList)
            {
                ammunition.Draw(spriteBatch);
            }
        }

        public void Update(ref List<Unit> enemies, GameTime gameTime)
        {
            ammunitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            GetClosestEnemy(ref enemies);
            if (target != null)
            {
                //Turn to face target here

                if (ammunitionTimer >= fireRate)
                {
                    Ammunition ammunition = new Ammunition(type, center, rotation, ammunitionSpeed, damage);
                    ammunitionList.Add(ammunition);
                }
            }
            else 
            {
                ammunitionTimer = 0;
            }

            for (int i = 0; i < ammunitionList.Count; i++)
            {
                Ammunition ammunition = ammunitionList[i];

                ammunition.SetRotation(rotation);
                ammunition.Update();

                if (!IsInRange(ammunition.Center))
                {
                    ammunition.Kill();
                }

                if (ammunition.IsDead())
                {
                    ammunitionList.Remove(ammunition);
                    i--;
                }
            }
        }
    }
}

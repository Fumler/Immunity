using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Tower
	{
		protected List<Ammunition> ammunitionList;
		protected int cellPositionX, cellPositionY;
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
        private static Texture2D turret;

		protected Unit target;

		public int Type
		{
			get { return type; }
		}

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
		{
            cellPositionX = cellX;
            cellPositionY = cellY;
			ammunitionList = new List<Ammunition>();
			rotation = 0.0f;
            UpdateAttributes(type);
		}

        public static int GetCost(int type) {
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
				case 10:
					fireRate = 2;
					ammunitionSpeed = 2;
					ammunitionTimer = 1.0f;
                    damage = 50;
					break;

				case 11:
					fireRate = 3;
					ammunitionSpeed = 3;
					ammunitionTimer = 0.5f;
                    damage = 60;
					break;

				case 20:
					fireRate = 1;
					ammunitionSpeed = 1;
					ammunitionTimer = 3.0f;
                    damage = 90;
					break;

				case 21:
					fireRate = 1;
					ammunitionSpeed = 1;
					ammunitionTimer = 1.0f;
                    damage = 120;
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

		public void Shoot()
		{
			Ammunition temp = new Ammunition(0, center, rotation, ammunitionSpeed, damage);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            System.Diagnostics.Debug.WriteLine("-_-");
            spriteBatch.Draw(turret, new Vector2(cellPositionX * Map.TILESIZE, (cellPositionY * Map.TILESIZE) + 24), Color.White);
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
				//TODO: Turn to face target here

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
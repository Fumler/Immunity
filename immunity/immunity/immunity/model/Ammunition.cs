using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Ammunition : ScreenObject
    {
        //Variables
        //Static
        private static List<Texture2D> sprites;

        //Location
        private Vector2 velocity;

        //Data
        private int damage;
        private int speed;
        private int age = 0;
        private int decayTimer = 100;
        private Unit target;

        //Accessors
        public int Damage
        {
            get { return damage; }
        }

        //Constructors
        /// <summary>
        /// Creates a new instance of Ammunition.
        /// </summary>
        /// <param name="type">The tower type.</param>
        /// <param name="position">The center of the tower.</param>
        /// <param name="rotation">The rotation of the tower.</param>
        /// <param name="speed">How fast the ammunition moves.</param>
        /// <param name="damage">The towers damage.</param>
        public Ammunition(int type, Vector2 position, float rotation, int speed, int damage, ref Unit target) : base(Ammunition.sprites[type], position)
        {
            this.type = type;
            this.rotation = rotation;
            this.speed = speed;
            this.damage = damage;
            this.target = target;
        }

        //Static methods
        /// <summary>
        /// Sets sprites for Ammunition.
        /// </summary>
        /// <param name="textures"></param>
        public static void SetSprites(List<Texture2D> textures)
        {
            sprites = textures;
        }

        //Methods
        /// <summary>
        /// Checks if the ammunition is old and should  be removed.
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return age > decayTimer;
        }

        /// <summary>
        /// Sets the ammunition to die on next update.
        /// </summary>
        public void Kill()
        {
            age += decayTimer;
        }

        /// <summary>
        /// Updates the ammunition
        /// </summary>
        public void Update(ref List<Unit> enemies)
        {
            age++;
            position += velocity;
            this.center = new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);

            foreach (Unit enemy in enemies)
            {
                if (Vector2.Distance(Center, enemy.Center) < 12)
                {
                    enemy.OnBulletHit(Damage);
                    Kill();
                }
            }
        }

        /// <summary>
        /// Bends the ammunition towards the target.
        /// </summary>
        /// <param name="rotation">The towers rotation.</param>
        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
            velocity = Vector2.Transform(new Vector2(0, -speed), Matrix.CreateRotationZ(rotation));
        }
    }
}
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Ammunition
    {
        //Variables
        //Static
        private static List<Texture2D> sprites;

        //Location
        private Vector2 position;

        private Vector2 velocity;
        private Vector2 center;
        private Vector2 origin;
        private float rotation;

        //Data
        private int type;

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

        public Vector2 Center
        {
            get { return center; }
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
        public Ammunition(int type, Vector2 position, float rotation, int speed, int damage, ref Unit target)
        {
            this.type = type;
            this.position = position;
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
            this.center = new Vector2(position.X + (sprites[type].Width / 2), position.Y + (sprites[type].Height / 2));
            this.origin = new Vector2(sprites[type].Width / 2, sprites[type].Height / 2);

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
        /// Draws the ammunition.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[type], position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
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
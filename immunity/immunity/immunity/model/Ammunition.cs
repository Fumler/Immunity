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
        public Ammunition(int type, Vector2 position, float rotation, int speed, int damage)
        {
            this.type = type;
            this.position = position;
            this.rotation = rotation;
            this.speed = speed;
            this.damage = damage;
        }

        //Static methods
        public static void SetSprites(List<Texture2D> textures)
        {
            sprites = textures;
        }
       
        //Methods
        public bool IsDead()
        {
            return age > decayTimer;
        }

        public void Kill()
        {
            age = ++decayTimer;
            position += velocity;
        }

        public void Update()
        {
            age++;
            position += velocity;
            this.center = new Vector2(position.X + (sprites[type].Width / 2), position.Y + (sprites[type].Height / 2));
            this.origin = new Vector2(sprites[type].Width / 2, sprites[type].Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[type], center, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
            velocity = Vector2.Transform(new Vector2(0, -speed), Matrix.CreateRotationZ(rotation));
        }
    }
}
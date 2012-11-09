using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class Ammunition
    {
        private static List<Texture2D> sprites;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 center;
        private Vector2 origin;
        private float rotation;

        private int type;
        private int damage;
        private int speed;
        private int age = 0;
        private int decayTimer = 100;

        public Ammunition(int type, Vector2 position, float rotation, int speed, int damage)
        {
            this.type = type;
            this.position = position;
            this.rotation = rotation;
            this.speed = speed;
            this.damage = damage;

            this.center = new Vector2(position.X + (sprites[type].Width / 2), position.Y + (sprites[type].Height / 2));
        }

        public int Damage
        {
            get{ return damage; }
        }

        public bool IsDead() {
            return age > decayTimer;
        }

        public void Kill() {
            age = ++decayTimer;
            position += velocity;
        }

        public void Update() {
            age++;
            position += velocity;
            this.center = new Vector2(position.X + (sprites[type].Width / 2), position.Y + (sprites[type].Height / 2));
            this.origin = new Vector2(sprites[type].Width / 2, sprites[type].Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprites[type], center, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }

        public void SetRotation(float value) {
            rotation = value;
            velocity = Vector2.Transform(new Vector2(0, -speed), Matrix.CreateRotationZ(rotation));
        }
    }
}

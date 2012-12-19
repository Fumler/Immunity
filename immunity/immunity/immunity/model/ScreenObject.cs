using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity.model
{
    internal class ScreenObject
    {
        protected Texture2D texture;

        protected Vector2 position;
        protected Vector2 center;
        protected Vector2 origin;
        protected float rotation;

        protected Rectangle bounds;

        protected int type;

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Center
        {
            get { return center; }
        }

        public int Type
        {
            get { return type; }
        }

        public ScreenObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            center = new Vector2(position.X + texture.Width / 2,
                position.Y + texture.Height / 2 + 24);

            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.center = new Vector2(position.X + texture.Width / 2,
                position.Y + texture.Height / 2 + 24);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, center, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
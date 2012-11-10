using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class PathView
    {
        //Variables
        private List<Vector2> thePath;
        private Texture2D pathTile;

        //Accessors
        public Texture2D Texture
        {
            set { this.pathTile = value; }
        }

        public List<Vector2> Path
        {
            set { this.thePath = value; }
        }

        //Methods
        /// <summary>
        /// Draws the path the units follow.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector2 tile in thePath)
            {
                spriteBatch.Draw(pathTile, new Vector2(tile.X, tile.Y + 24), Color.White);
            }
        }
    }
}
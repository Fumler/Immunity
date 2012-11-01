using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class PathView
    {
        private List<Vector2> thePath;
        private Texture2D pathTile;

        public Texture2D texture {
            set { this.pathTile = value; }
        }

        public List<Vector2> path {
            set { this.thePath = value; }
        }

        public void draw(SpriteBatch spriteBatch) {
            foreach(Vector2 tile in thePath) {
                spriteBatch.Draw(pathTile, new Vector2(tile.X, tile.Y + 24), Color.White);
            }
        }
    }
}

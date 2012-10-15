using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity.model
{
    class Unit
    {
        private List<Texture2D> sprites;
        private Point unitPosition;
        private int health;
        
        public Unit()
        {
        }

        public void setSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }

        public void draw(SpriteBatch spriteBatch)
        {
        }

        public void Update()
        {
        }


    }
}

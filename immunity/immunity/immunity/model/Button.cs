using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace immunity
{

    class Button
    {
        public bool pressed = false;

        private List<Texture2D> buttons;

        private Rectangle bounds;

        public Button(Rectangle bounds) 
        {
            this.bounds = bounds;
        }

        public void setSprites(List<Texture2D> sprites)
        {
            this.buttons = sprites;
        }

        public void draw(SpriteBatch spriteBatch, int texture)
        {
            spriteBatch.Draw(buttons[texture], bounds, Color.White);
        }
    }
}

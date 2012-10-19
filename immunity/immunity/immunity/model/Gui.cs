using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace immunity
{
    class Gui
    {

        private List<Texture2D> sprites;
        private List<SpriteFont> fonts;

        private Vector2 position;
        private Vector2 textPosition;

        private Rectangle actionbar;

        public Gui(Rectangle actionbar)
        {
            //this.position = position;
            this.actionbar = actionbar;

            textPosition = new Vector2(5, 5);

        }

        public void setSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }

        public void setFonts(List<SpriteFont> fonts)
        {
            this.fonts = fonts;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites[2], actionbar, Color.White);

            string text = "Gold: 0 Lives: 0";
            spriteBatch.DrawString(fonts[0], text, textPosition, Color.White);
        }

    }
}

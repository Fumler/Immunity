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

        public Gui(Vector2 position)
        {
            this.position = position;

            textPosition = new Vector2(500, 500);

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
            spriteBatch.Draw(sprites[2], position, Color.White);

            string text = "Gold: 0 Lives: 0";
            spriteBatch.DrawString(fonts[0], text, textPosition, Color.White);
        }

    }
}

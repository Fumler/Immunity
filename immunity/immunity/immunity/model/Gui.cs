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

        private Vector2 textPosition;

        private Rectangle actionbar;

        string text = "GOLD 0 LIVES 0";

        Game1 game = new Game1();

        public Gui(Rectangle actionbar)
        {
            //this.position = position;
            this.actionbar = actionbar;

        }

        public void setSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }

        public void setFonts(List<SpriteFont> fonts)
        {
            this.fonts = fonts;

            Vector2 stringCenter = fonts[0].MeasureString(text) * 0.5f;
            textPosition.X = (int)((game.width / 2) - stringCenter.X);
            textPosition.Y = 5;

        }

        public void draw(SpriteBatch spriteBatch, int texture)
        {
            spriteBatch.Draw(sprites[texture], actionbar, Color.White);

            
            spriteBatch.DrawString(fonts[1], text, textPosition, Color.White);
        }

    }
}

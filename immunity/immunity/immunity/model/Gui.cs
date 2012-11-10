using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Gui
    {
        private Rectangle actionbar;
        private List<SpriteFont> fonts;
        private Game1 game = new Game1();
        private List<Texture2D> sprites;
        private Vector2 textPosition;

        public Gui(Rectangle actionbar)
        {
            this.actionbar = actionbar;
        }

        public void Draw(SpriteBatch spriteBatch, int texture, Player player)
        {
            spriteBatch.Draw(sprites[texture], actionbar, Color.White);

            string topbarText = String.Format("GOLD: {0} - LEVEL: {1}", player.Gold, player.Lives);
            spriteBatch.DrawString(fonts[1], topbarText, textPosition, Color.White);
        }

        public void SetFonts(List<SpriteFont> fonts)
        {
            this.fonts = fonts;

            Vector2 stringCenter = fonts[0].MeasureString("GOLD: xx - LEVEL: xx") * 0.5f;
            textPosition.X = (int)((game.width / 2) - stringCenter.X);
            textPosition.Y = 5;
        }

        public void SetSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Gui
    {
        private Rectangle actionbar;
        private static SpriteFont font;
        private Game1 game = new Game1();
        private static List<Texture2D> sprites;
        private Vector2 textPosition;
        private Player playerObject;

        private String topbarText;

        public static SpriteFont Font
        {
            set { font = value; }
        }

        public static List<Texture2D> Sprites
        {
            set { sprites = value; }
        }

        public Gui(Rectangle actionbar)
        {
            this.actionbar = actionbar;
        }

        public void Draw(SpriteBatch spriteBatch, int texture, Player player)
        {
            topbarText = String.Format("GOLD: {0} - WAVE: {1} - LIVES: {2}", player.Gold, player.Wave, player.Lives);
            Vector2 stringCenter = font.MeasureString(topbarText);
            textPosition.X = (int)((game.width / 2) - stringCenter.X * 0.5);
            textPosition.Y = 5;
            spriteBatch.Draw(sprites[texture], actionbar, Color.White);
            spriteBatch.DrawString(font, topbarText, textPosition, Color.White);

        }
    }
}
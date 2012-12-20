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
        private static Vector2 screen;
        private static List<Texture2D> sprites;
        private Vector2 textPosition;
        private Vector2 stringCenter;
        private String topbarText;

        public static SpriteFont Font
        {
            set { font = value; }
        }

        public static void SetScreenSize(int width, int height)
        {
            screen = new Vector2(width, height);
        }

        public static List<Texture2D> Sprites
        {
            set { sprites = value; }
        }

        public Gui(Rectangle actionbar)
        {
            this.actionbar = actionbar;
        }

        public Gui(Rectangle actionbar, string text)
        {
            this.actionbar = actionbar;
            this.topbarText = text.ToUpper();

            stringCenter = font.MeasureString(text.ToUpper());

        }

        public void Draw(SpriteBatch spriteBatch, int texture, Player player)
        {
            topbarText = String.Format("GOLD: {0} - WAVE: {1} - LIVES: {2}", player.Gold, player.Wave, player.Lives);
            stringCenter = font.MeasureString(topbarText);
            textPosition.X = (int)((screen.X / 2) - stringCenter.X * 0.5);
            textPosition.Y = 5;
            spriteBatch.Draw(sprites[texture], actionbar, Color.White);
            spriteBatch.DrawString(font, topbarText, textPosition, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, int texture)
        {
            spriteBatch.Draw(sprites[texture], actionbar, Color.White);
            spriteBatch.DrawString(font, topbarText, new Vector2(actionbar.X + 5, actionbar.Y), Color.White);

        }
    }
}
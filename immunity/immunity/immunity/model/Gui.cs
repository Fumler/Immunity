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
        private static Player playerObject;

        private static String topbarText;


        public static void PlayerObject (ref Player player) {
            playerObject = player;
            topbarText = String.Format("GOLD: {0} - WAVE: {1} - LIVES: {2}", playerObject.Gold, playerObject.Wave, playerObject.Lives);
        }

        public Gui(Rectangle actionbar)
        {
            this.actionbar = actionbar;
        }

        public void Draw(SpriteBatch spriteBatch, int texture, Player player)
        {
            spriteBatch.Draw(sprites[texture], actionbar, Color.White);

            //topbarText = String.Format("GOLD: {0} - WAVE: {1} - LIVES: {2}", player.Gold, player.Wave, player.Lives);
            System.Diagnostics.Debug.WriteLine("TextPos: " + textPosition);
            spriteBatch.DrawString(fonts[1], topbarText, textPosition, Color.White);

        }

        public void SetFonts(List<SpriteFont> fonts)
        {
            this.fonts = fonts;

            // Seems XNA/C# calculates MeasureString by the widest letter, so the length of the string is incorrect for this font. 
            Vector2 stringCenter = fonts[1].MeasureString(topbarText);
            textPosition.X = (int)((game.width / 2) - stringCenter.X * 0.5);
            textPosition.Y = 5;
            //System.Diagnostics.Debug.WriteLine("X: " + textPosition.X + " Y: " + textPosition.Y + " GAMEWIDTH: " + game.width + " FONTWIDTH: " + stringCenter.X);

        }

        public void SetSprites(List<Texture2D> sprites)
        {
            this.sprites = sprites;
        }
    }
}
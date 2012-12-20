using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace immunity
{
    class TextInput
    {
        private Rectangle bounds;
        private string input;
        private Input hwInput = new Input();
        private Texture2D texture;
        private List<SpriteFont> fonts;
        private bool typing = false;
        private Keys[] lastPressedKeys;

        public TextInput(Rectangle bounds)
        {
            this.bounds = bounds;
            this.input = "";
            this.lastPressedKeys = new Keys[0];
        }

        public string Value
        {
            set { this.input = value; }
            get { return input; }
        }

        public void InitVars(Texture2D texture, List<SpriteFont> fonts)
        {
            this.texture = texture;
            this.fonts = fonts;
        }

        public void Update()
        {
            hwInput.Update();

            bool isMouseOver = bounds.Contains((int)hwInput.Position.X, (int)hwInput.Position.Y);
            if (hwInput.ReleaseLeftClick && isMouseOver)
            {
                typing = true;
            }
            else if (typing && !isMouseOver && hwInput.ReleaseLeftClick)
            {
                typing = false;
            }
            if (typing)
            {
                Keys[] pressed = hwInput.currentKeyState.GetPressedKeys();
                foreach(Keys key in pressed)
                {
                    if (hwInput.previousKeyState.IsKeyUp(key))
                    {
                        switch(key) {
                            case Keys.Back:
                                if(input.Length > 0)
                                    input = input.Remove(input.Length-1);
                                break;
                            case Keys.Escape:
                                this.typing = false;
                                break;
                            case Keys.Space:
                                input += " ";
                                break;
                            case Keys.A:
                            case Keys.B:
                            case Keys.C:
                            case Keys.D:
                            case Keys.E:
                            case Keys.F:
                            case Keys.G:
                            case Keys.H:
                            case Keys.I:
                            case Keys.J:
                            case Keys.K:
                            case Keys.L:
                            case Keys.M:
                            case Keys.N:
                            case Keys.O:
                            case Keys.P:
                            case Keys.Q:
                            case Keys.R:
                            case Keys.S:
                            case Keys.T:
                            case Keys.U:
                            case Keys.V:
                            case Keys.W:
                            case Keys.X:
                            case Keys.Y:
                            case Keys.Z:
                                input += key;
                                break;
                            case Keys.D0:
                            case Keys.D1:
                            case Keys.D2:
                            case Keys.D3:
                            case Keys.D4:
                            case Keys.D6:
                            case Keys.D7:
                            case Keys.D8:
                            case Keys.D9:
                                input += key.ToString()[1];
                                break;
                            default:
                                break;
                        }
                    }
                }
                this.lastPressedKeys = pressed;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.Black);
            spriteBatch.DrawString(fonts[1], input, new Vector2(bounds.X, bounds.Y), Color.White);
            if(typing)
                spriteBatch.DrawString(fonts[1], "|", new Vector2(bounds.X + (int)fonts[1].MeasureString(input).X + 1, bounds.Y), Color.White);
        }
    }
}

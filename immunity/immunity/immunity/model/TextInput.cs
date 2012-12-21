using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace immunity
{
    internal class TextInput
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

        public bool Active
        {
            set { this.typing = value; }
            get { return typing; }
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
                foreach (Keys key in pressed)
                {
                    if (hwInput.previousKeyState.IsKeyUp(key))
                    {

                        //foreach (Keys k in System.Enum.GetValues(typeof(Keys)))
                        //{
                        //    System.Diagnostics.Debug.WriteLine("Key: " + k);

                        //    if (k.ToString().Contains("F"))
                        //    {

                        //    }

                        //    else if (k.ToString().Contains("Space"))
                        //    {
                        //        input += " ";
                        //    }

                        //    else if (k.ToString().Contains("D"))
                        //    {
                                
                        //        if(k.ToString()[1] >= 0 && k.ToString()[1] <= 9) 
                        //        {
                        //            if(hwInput.IsKeyPressed(Keys.LeftShift) || hwInput.IsKeyPressed(Keys.RightShift)) {
                        //                switch (k) {
                        //                    case Keys.D1: input += "!"; break;
                        //                    case Keys.D2: input += "\""; break;
                        //                    case Keys.D3: input += "#"; break;
                        //                    case Keys.D4: input += "¤"; break;
                        //                    case Keys.D5: input += "%"; break;
                        //                    case Keys.D6: input += "&"; break;
                        //                    case Keys.D7: input += "/"; break;
                        //                    case Keys.D8: input += "("; break;
                        //                    case Keys.D9: input += ")"; break;
                        //                    case Keys.D0: input += "="; break;
                        //                }
                        //            }
                        //             else if(hwInput.IsKeyPressed(Keys.RightAlt)) {
                        //                switch (k) {
                        //                    case Keys.D1: input += "1"; break;
                        //                    case Keys.D2: input += "@"; break;
                        //                    case Keys.D3: input += "£"; break;
                        //                    case Keys.D4: input += "$"; break;
                        //                    case Keys.D5: input += "€"; break;
                        //                    case Keys.D6: input += "6"; break;
                        //                    case Keys.D7: input += "{"; break;
                        //                    case Keys.D8: input += "["; break;
                        //                    case Keys.D9: input += "]"; break;
                        //                    case Keys.D0: input += "}"; break;
                        //                }
                        //            }
                        //            else {
                                        
                        //                input += k.ToString()[1];
                                        
                        //             }
                        //        }

                        //    }

                        //    else (k.ToString().Contains("F"))
                        //    {

                        //    }
    
                        //}
                        switch (key)
                        {
                            case Keys.Back:
                                if (input.Length > 0)
                                    input = input.Remove(input.Length - 1);
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
            if (typing)
                spriteBatch.DrawString(fonts[1], "|", new Vector2(bounds.X + (int)fonts[1].MeasureString(input).X + 1, bounds.Y), Color.White);
        }
    }
}

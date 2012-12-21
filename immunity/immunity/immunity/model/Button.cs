using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace immunity
{
    public enum MouseStatus
    {
        Normal,
        Clicked,
        Released
    }

    internal class Button
    {
        private Input input = new Input();

        private static List<Texture2D> buttons;

        private Rectangle bounds;

        private MouseStatus state = MouseStatus.Normal;

        private static List<SpriteFont> fonts;

        private String tooltip;

        private static GameTime gametime;
        private int textureID;

        public event EventHandler clicked;

        public delegate void EventHandler(string n);

        private Keys key;

        public string type = "10";
        private int cellX;
        private int cellY;
        private static int gameWidth;
        private static int gameHeight;

        public static int GameWidth
        {
            set { gameWidth = value; }
        }

        public static int GameHeight
        {
            set { gameHeight = value; }
        }

        public static GameTime Gametime
        {
            set { gametime = value; }
        }

        public int TextureID
        {
            set { this.textureID = value; }
        }

        public static List<SpriteFont> Fonts
        {
            set { fonts = value; }
        }

        public static List<Texture2D> Buttons
        {
            set { buttons = value; }
        }

        public Button(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        public Button(Rectangle bounds, string type, Keys key)
        {
            this.bounds = bounds;
            this.type = type;
            this.key = key;
        }

        public Button(Rectangle bounds, string type, string tooltip, Keys key, int texture)
        {
            this.bounds = bounds;
            this.type = type;
            this.tooltip = tooltip;
            this.key = key;
            this.textureID = texture;
        }

        public Button(Rectangle bounds, string type, string tooltip, int cellX, int cellY, Keys key)
        {
            this.bounds = bounds;
            this.type = type;
            this.tooltip = tooltip;
            this.cellX = cellX;
            this.cellY = cellY;
            this.key = key;
        }

        public void Update()
        {
            input.Update();

            state = MouseStatus.Normal;

            bool isMouseOver = bounds.Contains((int)input.Position.X, (int)input.Position.Y);

            if (isMouseOver && !input.LeftClick)
            {
                state = MouseStatus.Released;
            }
            else if (!isMouseOver && !input.LeftClick)
                state = MouseStatus.Normal;
            else if (isMouseOver && input.LeftClick)
                state = MouseStatus.Clicked;

            if (input.NewLeftClick)
            {
                if (isMouseOver)
                {
                    state = MouseStatus.Clicked;
                }
            }

            if (input.ReleaseLeftClick)
            {
                if (isMouseOver)
                {
                    state = MouseStatus.Released;
                }
            }

            if (input.IsKeyPressed(key))
            {
                if (clicked != null)
                {
                    clicked(this.type);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case MouseStatus.Normal:
                    spriteBatch.Draw(buttons[textureID], bounds, Color.White);

                    break;

                case MouseStatus.Released:
                    spriteBatch.Draw(buttons[textureID], bounds, Color.DarkGray);

                    int measureString = (int)fonts[1].MeasureString(tooltip.ToUpper()).X;

                    if (tooltip != null)
                    {
                        if (bounds.X + 5 >= gameWidth - bounds.X)
                        {
                            spriteBatch.Draw(buttons[1], new Rectangle(bounds.X + buttons[1].Width - measureString, bounds.Y - 27, measureString + 20, 20), Color.Black);
                            spriteBatch.DrawString(fonts[1], tooltip.ToUpper(), new Vector2(bounds.X + buttons[1].Width + 5 - measureString, bounds.Y - 25), Color.White);
                        }
                        else
                        {
                            if (bounds.X >= 100)
                            {
                                spriteBatch.Draw(buttons[1], new Rectangle(bounds.X + 7, bounds.Y - 13, measureString + 25, 20), Color.Black);
                                spriteBatch.DrawString(fonts[1], tooltip.ToUpper(), new Vector2(bounds.X + 13, bounds.Y - 9), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(buttons[1], new Rectangle(bounds.X - 5, bounds.Y - 27, measureString + 25, 20), Color.Black);
                                spriteBatch.DrawString(fonts[1], tooltip.ToUpper(), new Vector2(bounds.X + 5, bounds.Y - 25), Color.White);
                            }
                        }
                    }
                    break;

                case MouseStatus.Clicked:
                    if (input.currentMouseState.LeftButton != input.previousMouseState.LeftButton)
                    {
                        if (clicked != null)
                        {
                            clicked(this.type);
                            state = MouseStatus.Normal;
                        }
                    }
                    spriteBatch.Draw(buttons[textureID], bounds, Color.Red);

                    break;

                default:
                    spriteBatch.Draw(buttons[textureID], bounds, Color.Black);

                    break;
            }
        }
    }
}
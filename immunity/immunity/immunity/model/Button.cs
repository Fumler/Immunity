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

        private MouseState previousState;

        private static List<Texture2D> buttons;

        private Rectangle bounds;

        private MouseStatus state = MouseStatus.Normal;

        private static List<SpriteFont> fonts;

        private String tooltip;

        private GraphicsDevice graphicsDevice;

        public event EventHandler clicked;

        private Keys key;

        public int type = 10;
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

        public Button(Rectangle bounds, int type, Keys key)
        {
            this.bounds = bounds;
            this.type = type;
            this.key = key;
        }

        public Button(Rectangle bounds, int type, string tooltip, Keys key)
        {
            this.bounds = bounds;
            this.type = type;
            this.tooltip = tooltip;
            this.key = key;
        }

        public Button(Rectangle bounds, int type, string tooltip, int cellX, int cellY, Keys key)
        {
            this.bounds = bounds;
            this.type = type;
            this.tooltip = tooltip;
            this.cellX = cellX;
            this.cellY = cellY;
            this.key = key;
        }

        public void Update(GameTime gameTime)
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
                    clicked(this, EventArgs.Empty);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int texture)
        {
            switch (state)
            {
                case MouseStatus.Normal:
                    spriteBatch.Draw(buttons[texture], bounds, Color.White);

                    break;

                case MouseStatus.Released:
                    spriteBatch.Draw(buttons[texture], bounds, Color.DarkGray);

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
                            clicked(this, EventArgs.Empty);
                            state = MouseStatus.Normal;
                        }
                    }
                    spriteBatch.Draw(buttons[texture], bounds, Color.Red);

                    break;

                default:
                    spriteBatch.Draw(buttons[texture], bounds, Color.Black);

                    break;
            }
        }
    }
}

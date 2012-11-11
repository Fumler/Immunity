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
        private Input mouse = new Input();

        private MouseState previousState;

        private static List<Texture2D> buttons;

        private Rectangle bounds;

        private MouseStatus state = MouseStatus.Normal;

        private static List<SpriteFont> fonts;

        private String tooltip;

        GraphicsDevice graphicsDevice;

        public event EventHandler clicked;

        public int type = 10;

        public static List<SpriteFont> Fonts
        {
            set { fonts = value; }
        }

        public static List<Texture2D> Buttons
        {
            set { buttons = value; }
        }

        public List<Texture2D> Buttons
        {
            set { this.buttons = value; }
        }

        public Button(Rectangle bounds)
        {
            this.bounds = bounds;
        }
        public Button(Rectangle bounds, int type) {
            this.bounds = bounds;
            this.type = type;
        }

        public Button(Rectangle bounds, int type, string tooltip)
        {
            this.bounds = bounds;
            this.type = type;
            this.tooltip = tooltip;
        }

        public void Update(GameTime gameTime)
        {
            mouse.Update();

            state = MouseStatus.Normal;

            bool isMouseOver = bounds.Contains((int)mouse.Position.X, (int)mouse.Position.Y);

            if (isMouseOver && !mouse.LeftClick)
            {
                state = MouseStatus.Released;
            }
            else if (!isMouseOver && !mouse.LeftClick)
                state = MouseStatus.Normal;
            else if (isMouseOver && mouse.LeftClick)
                state = MouseStatus.Clicked;

            if (mouse.NewLeftClick)
            {
                if (isMouseOver)
                {
                    state = MouseStatus.Clicked;
                }
            }

            if (mouse.ReleaseLeftClick)
            {
                if (isMouseOver)
                {
                    state = MouseStatus.Released;
                }
            }
        }

        public void SetSprites(List<Texture2D> sprites)
        {
            this.buttons = sprites;
        }

        public void Draw(SpriteBatch spriteBatch, int texture)
        {
            switch (state)
            {
                case MouseStatus.Normal:
                    spriteBatch.Draw(buttons[texture], bounds, Color.White);
                    
                    break;

                case MouseStatus.Released:
                    spriteBatch.Draw(buttons[texture], bounds, Color.Blue);
                    if (tooltip != null)
                    {
                        spriteBatch.Draw(buttons[1], new Rectangle(bounds.X - 5, bounds.Y - 27, (int)fonts[1].MeasureString(tooltip.ToUpper()).X + 25, 20), Color.Black);
                        spriteBatch.DrawString(fonts[1], tooltip.ToUpper(), new Vector2(bounds.X + 5, bounds.Y - 25), Color.White);

                    }
                    break;

                case MouseStatus.Clicked:
                    if (mouse.currentMouseState.LeftButton != mouse.previousMouseState.LeftButton)
                    {
                        if (clicked != null)
                        {
                            clicked(this, EventArgs.Empty);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


namespace immunity
{

    public enum MouseStatus
    {
        Normal,
        Clicked,
        Released
    }

    class Button
    {
        Input mouse = new Input();

        private MouseState previousState;

        private List<Texture2D> buttons;

        private Rectangle bounds;

        private MouseStatus state = MouseStatus.Normal;

        public event EventHandler clicked;

        public int type = 20;

        public Button(Rectangle bounds) 
        {
            this.bounds = bounds;
        }

        public void Update(GameTime gameTime)
        {
            mouse.update();

            state = MouseStatus.Normal;

            bool isMouseOver = bounds.Contains((int)mouse.Position.X, (int)mouse.Position.Y);

            if (isMouseOver && !mouse.leftClick)
            {
                state = MouseStatus.Released;
            }
            else if (!isMouseOver && !mouse.leftClick)
                state = MouseStatus.Normal;
            else if (isMouseOver && mouse.leftClick)
                state = MouseStatus.Clicked;


            if (mouse.newLeftClick)
            {
                if (isMouseOver)
                {
                    state = MouseStatus.Clicked;
                }
            }

            if (mouse.releaseLeftClick)
            {
                if (isMouseOver)
                {
                    state = MouseStatus.Released;
                }
            }
            
        }

        

        public void setSprites(List<Texture2D> sprites)
        {
            this.buttons = sprites;
        }

        public void draw(SpriteBatch spriteBatch, int texture)
        {
            
            switch (state)
            {
                case MouseStatus.Normal:
                    spriteBatch.Draw(buttons[texture], bounds, Color.White);

                    break;
                case MouseStatus.Released:
                    spriteBatch.Draw(buttons[texture], bounds, Color.Blue);
                    break;
                case MouseStatus.Clicked:
                    if (mouse.currentMouseState.LeftButton != mouse.previousMouseState.LeftButton)
                    {
                        System.Diagnostics.Debug.WriteLine("If mouse is click and held down, do this only once!");
                        if (clicked != null) {
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

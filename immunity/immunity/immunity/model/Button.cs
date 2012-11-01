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

        public Button(Rectangle bounds) 
        {
            this.bounds = bounds;
        }

        public void Update(GameTime gameTime)
        {
            mouse.update();
            // do it here instead of mouse class to test
            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            bool isMouseOver = bounds.Contains(mouseX, mouseY);

            if (isMouseOver && !mouse.leftClick)
            {
                state = MouseStatus.Released;
            }
            else if (!isMouseOver && !mouse.leftClick)
                state = MouseStatus.Normal;

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
            else if (state == MouseStatus.Clicked)
            {
                state = MouseStatus.Normal;
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
                     spriteBatch.Draw(buttons[texture], bounds, Color.Red);
                    break;
                default:
                    spriteBatch.Draw(buttons[texture], bounds, Color.White);
              
                    break;
            }
        }
    }
}

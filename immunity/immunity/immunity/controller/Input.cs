using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace immunity
{
    internal class Input
    {
        /// <summary>
        /// Handles the states of the mouse and keyboard.
        /// </summary>
        private KeyboardState previousKeyState, currentKeyState;

        public MouseState previousMouseState, currentMouseState;

        ////////////////////////////////////////////////////////////////////////// MOUSE

        public Input()
        {
        }

        /// <summary>
        /// The coordinates of the mouse
        /// </summary>
        public Vector2 Position
        {
            get;
            protected set;
        }

        /// <summary>
        /// Checks if the left button is pressed
        /// </summary>
        public bool LeftClick
        {
            get { return currentMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the left button is clicked now but not before
        /// </summary>
        public bool NewLeftClick
        {
            get
            {
                return currentMouseState.LeftButton == ButtonState.Pressed
                  && previousMouseState.LeftButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Checks if the left button was released
        /// </summary>
        public bool ReleaseLeftClick
        {
            get { return !LeftClick && previousMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the right button is pressed
        /// </summary>
        public bool RightClick
        {
            get { return currentMouseState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the right button is clicked now but not before
        /// </summary>
        public bool NewRightClick
        {
            get
            {
                return currentMouseState.RightButton == ButtonState.Pressed
                  && previousMouseState.RightButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// Checks if the right button was released
        /// </summary>
        public bool ReleaseRightClick
        {
            get { return !RightClick && previousMouseState.RightButton == ButtonState.Pressed; }
        }

        public bool normalState
        {
            get { return !LeftClick && !ReleaseLeftClick && !RightClick && !ReleaseRightClick; }
        }

        ////////////////////////////////////////////////////////////////////////// KEYBOARD

        public bool IsKeyPressed(Keys key)
        {
            return (currentKeyState.IsKeyDown(key));
        }

        ////////////////////////////////////////////////////////////////////////// OTHER

        public void Update()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            Position = new Vector2(currentMouseState.X, currentMouseState.Y);
        }
    }
}
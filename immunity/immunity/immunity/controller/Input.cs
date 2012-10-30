using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace immunity
{
    class Input
    {
        /// <summary>
        /// Handles the states of the mouse and keyboard.
        /// </summary>
        KeyboardState previousKeyState, currentKeyState;
        MouseState previousMouseState, currentMouseState;


        ////////////////////////////////////////////////////////////////////////// MOUSE

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
        public bool leftClick
        {
            get { return currentMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the left button is clicked now but not before
        /// </summary>
        public bool newLeftClick
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
        public bool releaseLeftClick
        {
            get { return !leftClick && previousMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the right button is pressed
        /// </summary>
        public bool rightClick
        {
            get { return currentMouseState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the right button is clicked now but not before
        /// </summary>
        public bool newRightClick
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
        public bool releaseRightClick
        {
            get {return !rightClick && previousMouseState.RightButton == ButtonState.Pressed;}
        }

        public bool normalState
        {
            get { return !leftClick && !releaseLeftClick && !rightClick && !releaseRightClick; }
        }

        ////////////////////////////////////////////////////////////////////////// KEYBOARD

        public bool isKeyPressed(Keys key)
        {
            return (currentKeyState.IsKeyDown(key));
        }


        ////////////////////////////////////////////////////////////////////////// OTHER


        public void Update() {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            Position = new Vector2(currentMouseState.X, currentMouseState.Y);
        }
    }
}

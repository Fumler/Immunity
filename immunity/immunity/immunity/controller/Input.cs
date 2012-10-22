using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace immunity
{
    class Input
    {
        KeyboardState previousKeyState, currentKeyState;
        MouseState previousMouseState, currentMouseState;

        public bool IsMouseOnePressed() {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public void Update() {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
    }
}

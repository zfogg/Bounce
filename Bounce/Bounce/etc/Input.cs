using System;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public static class Input
    {
        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static MouseState MouseState
        {
            get { return mouseState; }
        }

        private static KeyboardState keyboardState, previousKeyboardState;
        private static MouseState mouseState, previousMouseState;

        public static void Update(MouseState newMouseState, KeyboardState newKeyboardState)
        {
            previousMouseState = mouseState;
            mouseState = newMouseState;

            previousKeyboardState = keyboardState;
            keyboardState = newKeyboardState;
        }

        public static bool IsNewState()
        {
            return (keyboardState == previousKeyboardState) && (mouseState == previousMouseState)
                ? false : true;
        }

        public static bool KeyPressUnique(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
                ? true : false;
        }

        public static bool KeyPressRelease(Keys key)
        {
            return (previousKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key))
                ? true : false;
        }

        public static bool LeftClickUnique()
        {
            return (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                ? true : false;
        }

        public static bool LeftClickRelease()
        {
            return (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                ? true : false;
        }

        public static bool RightClickUnique()
        {
            return (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                ? true : false;
        }

        public static bool RickClickRelease()
        {
            return (mouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
                ? true : false;
        }
    }
}

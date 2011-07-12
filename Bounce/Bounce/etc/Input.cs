using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public static class Input
    {
        public static MouseState MouseState { get { return mouseState; } }
        public static KeyboardState KeyboardState { get { return keyboardState; } }
        private static MouseState mouseState, previousMouseState;
        private static KeyboardState keyboardState, previousKeyboardState;
        
        public static bool IsNewState { get; set; }
        private static bool isNewMouseState { set { IsNewState = true; } }
        private static bool isNewKeyboardState { set { IsNewState = true; } }

        public static void Update(MouseState newMouseState, KeyboardState newKeyboardState)
        {
                if (newMouseState != previousMouseState)
                    isNewMouseState = true;
                previousMouseState = mouseState;
                mouseState = newMouseState;

                if (newKeyboardState != previousKeyboardState)
                    isNewKeyboardState = true;
                previousKeyboardState = keyboardState;
                keyboardState = newKeyboardState;
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

        public static bool MiddleClickUnique()
        {
            return (mouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released)
                ? true : false;
        }

        public static bool MiddleClickRelease()
        {
            return (mouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed)
                ? true : false;
        }
    }
}

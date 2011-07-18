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

        public static Vector2 MouseCursorVector2 { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public static Point MouseCursorPoint { get { return new Point(mouseState.X, mouseState.Y); } }

        public static bool IsNewState { get { return isNewState; } set { isNewState = value; } }
        private static bool isNewState;

        public static void Update(MouseState newMouseState, KeyboardState newKeyboardState)
        {
            IsNewState = false;

            if (newMouseState != previousMouseState)
                isNewState = true;

            previousMouseState = mouseState;
            mouseState = newMouseState;

            if (newKeyboardState != previousKeyboardState || newKeyboardState.GetPressedKeys().Length != 0)
                isNewState = true;

            previousKeyboardState = keyboardState;
            keyboardState = newKeyboardState;
        }

        public static bool KeyPressUnique(Keys key)
        {
            return (keyboardState.IsKeyDown(key) &&
                    previousKeyboardState.IsKeyUp(key));
        }

        public static bool KeyPressRelease(Keys key)
        {
            return (previousKeyboardState.IsKeyDown(key) &&
                    keyboardState.IsKeyUp(key));
        }

        public static bool LeftClickUnique()
        {
            return (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released);
        }

        public static bool LeftClickRelease()
        {
            return (mouseState.LeftButton == ButtonState.Released &&
                    previousMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool RightClickUnique()
        {
            return (mouseState.RightButton == ButtonState.Pressed &&
                    previousMouseState.RightButton == ButtonState.Released);
        }

        public static bool RickClickRelease()
        {
            return (mouseState.RightButton == ButtonState.Released &&
                    previousMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool MiddleClickUnique()
        {
            return (mouseState.MiddleButton == ButtonState.Pressed &&
                    previousMouseState.MiddleButton == ButtonState.Released);
        }

        public static bool MiddleClickRelease()
        {
            return (mouseState.MiddleButton == ButtonState.Released &&
                    previousMouseState.MiddleButton == ButtonState.Pressed);
        }

        public static bool MouseWheelForwards()
        {
            return (mouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue);
        }

        public static bool MouseWheelReverse()
        {
            return (mouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue);
        }
    }
}

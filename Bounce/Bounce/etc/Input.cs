using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public delegate void MouseEvent();

    public static class Input
    {
        public static MouseState MouseState { get { return mouseState; } }
        public static KeyboardState KeyboardState { get { return keyboardState; } }
        private static MouseState mouseState, previousMouseState;
        private static KeyboardState keyboardState, previousKeyboardState;

        public static Vector2 MouseCursorVector2 { get { return new Vector2(mouseState.X, mouseState.Y); } }
        private static Vector2 previousMouseCursorVector2;
        public static Point MouseCursorPoint { get { return new Point(mouseState.X, mouseState.Y); } }

        public static bool IsNewState { get { return isNewState; } set { isNewState = value; } }
        private static bool isNewState;

        //public static event MouseEvent OnMouseHover;
        //public static event MouseEvent OnLeftClick;
        //public static event MouseEvent OnRightClick;
        //public static event MouseEvent OnMouseWheel;

        private static PhysicalItem currentlySelectedItem;

        public static void Update(MouseState newMouseState, KeyboardState newKeyboardState)
        {
            IsNewState = false;

            if (newMouseState != previousMouseState)
                isNewState = true;

            if (isNewState)
            {
                try
                {
                    currentlySelectedItem = (PhysicalItem)BounceGame.World.TestPoint(
                                            ConvertUnits.ToSimUnits(MouseCursorVector2))
                                            .Body.UserData;
                    if (LeftClickUnique())
                        currentlySelectedItem.OnLeftClick();
                    if (RightClickUnique())
                        currentlySelectedItem.OnRightClick();
                    if (MouseWheelForwards() || MouseWheelReverse())
                        currentlySelectedItem.OnMouseWheel();
                }
                catch { currentlySelectedItem = null; }
            }

            previousMouseState = mouseState;
            previousMouseCursorVector2 = MouseCursorVector2;
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

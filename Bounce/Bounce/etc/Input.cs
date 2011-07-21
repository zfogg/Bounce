using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public delegate void KeyboardEvent(KeyboardState keyboardState);
    public delegate void MouseEvent(int selectedItemID, MouseState mouseState);

    public static class Input
    {
        public static KeyboardState KeyboardState { get { return keyboardState; } }
        private static KeyboardState keyboardState, previousKeyboardState;

        public static Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        private static Vector2 previousMousePosition;
        public static Point MousePoint { get { return new Point(mouseState.X, mouseState.Y); } }
        public static MouseState MouseState { get { return mouseState; } }
        private static MouseState mouseState, previousMouseState;

        public static PhysicalItem SelectedItem { get { return selectedItem; } private set { selectedItem = value; } }
        private static PhysicalItem selectedItem;
        private static int selectedItemID;

        public static event KeyboardEvent OnKeyDown;
        public static event KeyboardEvent OnKeyHoldDown;
        public static event KeyboardEvent OnKeyUp;

        public static event MouseEvent OnLeftClickDown;
        public static event MouseEvent OnLeftClickUp;
        public static event MouseEvent OnRightClickDown;
        public static event MouseEvent OnRightClickUp;
        public static event MouseEvent OnMiddleClickDown;
        public static event MouseEvent OnMiddleClickUp;
        public static event MouseEvent OnMouseHover;
        public static event MouseEvent OnMouseWheel;

        public static void Update(MouseState newMouseState, KeyboardState newKeyboardState)
        {
            previousKeyboardState = keyboardState;
            keyboardState = newKeyboardState;

            previousMouseState = mouseState;
            previousMousePosition = MousePosition;
            mouseState = newMouseState;

            //Keyboard events.
            if (keyboardState != previousKeyboardState)
            {
                if (keyboardState.GetPressedKeys().Length > previousKeyboardState.GetPressedKeys().Length)
                    if (OnKeyDown != null) OnKeyDown(keyboardState);
                if (keyboardState.GetPressedKeys().Length < previousKeyboardState.GetPressedKeys().Length)
                    if (OnKeyUp != null) OnKeyUp(keyboardState);
            }
            if (keyboardState.GetPressedKeys().Length != 0)
                if (OnKeyHoldDown != null) OnKeyHoldDown(keyboardState);

            //Mouse events.
            if (mouseState != previousMouseState)
            {
                if (LeftClickUnique())
                    if (OnLeftClickDown != null) OnLeftClickDown(selectedItemID, mouseState);
                else if (LeftClickRelease())
                    if (OnLeftClickUp != null) OnLeftClickUp(selectedItemID, mouseState);

                if (RightClickUnique())
                    if (OnRightClickDown != null) OnRightClickDown(selectedItemID, mouseState);
                else if (RightClickRelease())
                    if (OnRightClickUp != null) OnRightClickUp(selectedItemID, mouseState);

                if (MiddleClickUnique())
                    if (OnMiddleClickDown != null) OnMiddleClickDown(selectedItemID, mouseState);
                else if (MiddleClickRelease())
                    if (OnMiddleClickUp != null) OnMiddleClickUp(selectedItemID, mouseState);

                if (MouseWheelForwards() || MouseWheelReverse())
                    if (OnMouseWheel != null) OnMouseWheel(selectedItemID, mouseState);
            }
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

        public static bool RightClickRelease()
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

        public static float MouseWheelVelocity()
        {
            return MathHelper.Clamp(
                (mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) * 0.20f, -1, 1);
        }

        public static PhysicalItem MouseHoverPhysicalItem(World world)
        {
            try
            {
                selectedItem = (PhysicalItem)world.TestPoint(ConvertUnits.ToSimUnits(MousePosition)).Body.UserData;
                selectedItemID = selectedItem.Body.BodyId;
            }
            catch (NullReferenceException)
            {
                selectedItemID = -1;
                selectedItem = null;
            }
            finally
            {
                if (OnMouseHover != null) OnMouseHover(selectedItemID, mouseState);
            }

            return selectedItem;
        }
    }
}

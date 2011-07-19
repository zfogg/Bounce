using System;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public delegate void MouseEvent(int selectedItemID, MouseState mouseState);
    public delegate void KeyboardEvent(KeyboardState keyboardState);

    public static class Input
    {
        public static MouseState MouseState { get { return mouseState; } }
        public static KeyboardState KeyboardState { get { return keyboardState; } }
        private static MouseState mouseState, previousMouseState;
        private static KeyboardState keyboardState, previousKeyboardState;

        public static Vector2 MousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }
        private static Vector2 previousMousePosition;
        public static Point MousePoint { get { return new Point(mouseState.X, mouseState.Y); } }

        public static bool IsNewState { get { return isNewState; } set { isNewState = value; } }
        private static bool isNewState;

        //public static event MouseEvent OnMouseHover;
        public static event MouseEvent OnLeftClick;
        public static event MouseEvent OnRightClick;
        public static event MouseEvent OnMiddleClick;
        public static event MouseEvent OnMouseHover;
        public static event MouseEvent OnMouseWheel;

        public static event KeyboardEvent OnKeyDown;
        public static event KeyboardEvent OnKeyUp;
        public static event KeyboardEvent OnKeyHoldDown;

        public static PhysicalItem SelectedItem { get { return selectedItem; } private set { selectedItem = value; } }
        private static PhysicalItem selectedItem;

        public static void Update(MouseState newMouseState, KeyboardState newKeyboardState)
        {
            IsNewState = false;

            previousMouseState = mouseState;
            previousMousePosition = MousePosition;
            mouseState = newMouseState;

            previousKeyboardState = keyboardState;
            keyboardState = newKeyboardState;

            if (newMouseState != previousMouseState)
                isNewState = true;

            if (newKeyboardState != previousKeyboardState)
            {
                isNewState = true;
                if (OnKeyDown != null) OnKeyDown(keyboardState);
                if (OnKeyUp != null) OnKeyUp(keyboardState);
            }

            if (newKeyboardState.GetPressedKeys().Length != 0)
                if (OnKeyHoldDown != null) OnKeyHoldDown(keyboardState);

            //Mouse hover event.
            if (newMouseState != previousMouseState)
            {
                int selectedItemID = 0;
                try
                {
                    selectedItem = mouseOverItem();
                    selectedItemID = selectedItem.Body.BodyId;
                }
                catch (NullReferenceException e) { selectedItem = null; }
                finally { if (OnMouseHover != null) OnMouseHover(selectedItemID, mouseState); }

                if (LeftClickUnique())
                {
                    try { selectedItemID = selectedItem.Body.BodyId; }
                    catch (NullReferenceException e) { selectedItemID = -1; }
                    finally { if (OnLeftClick != null) OnLeftClick(selectedItemID, mouseState); }
                }

                if (RightClickUnique())
                {
                    try { selectedItemID = selectedItem.Body.BodyId; }
                    catch (NullReferenceException e) { selectedItemID = -1; }
                    finally { if (OnRightClick != null) OnRightClick(selectedItemID, mouseState); }
                }

                if (MiddleClickUnique())
                {
                    try { selectedItemID = selectedItem.Body.BodyId; }
                    catch (NullReferenceException e) { selectedItemID = -1; }
                    finally { if (OnMiddleClick != null) OnMiddleClick(selectedItemID, mouseState); }
                }

                if (MouseWheelForwards() || MouseWheelReverse())
                {
                    try { selectedItemID = selectedItem.Body.BodyId; }
                    catch (NullReferenceException e) { selectedItemID = -1; }
                    finally { if (OnMouseWheel != null) OnMouseWheel(selectedItemID, mouseState); }
                }
            }
        }

        private static PhysicalItem mouseOverItem()
        {
            return (PhysicalItem)BounceGame.World.TestPoint(ConvertUnits.ToSimUnits(MousePosition)).Body.UserData;
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

        public static float MouseWheelVelocity()
        {
            return MathHelper.Clamp(
                (mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) * 0.125f, -1, 1);
        }
    }
}

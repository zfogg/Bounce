using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public delegate void KeyboardEvent(KeyboardState keyboardState);
    public delegate void MouseEvent(int selectedItemID, MouseState mouseState);

    public class Input
    {
        public KeyboardState KeyboardState { get { return keyboardState; } }
        private KeyboardState keyboardState, previousKeyboardState;

        public Vector2 MouseVector2 { get { return new Vector2(mouseState.X, mouseState.Y); } }
        public Point MousePoint { get { return new Point(mouseState.X, mouseState.Y); } }
        private Vector2 previousMousePosition;
        public MouseState MouseState { get { return mouseState; } }
        private MouseState mouseState, previousMouseState;

        private PhysicalItem selectedItem;
        private int selectedItemID;

        public event KeyboardEvent OnKeyDown;
        public event KeyboardEvent OnKeyHoldDown;
        public event KeyboardEvent OnKeyUp;

        public event MouseEvent OnLeftClickDown;
        public event MouseEvent OnLeftClickUp;
        public event MouseEvent OnRightClickDown;
        public event MouseEvent OnRightClickUp;
        public event MouseEvent OnMiddleClickDown;
        public event MouseEvent OnMiddleClickUp;
        public event MouseEvent OnMouseWheel;
        public event MouseEvent OnMouseHover;

        public void Update()
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            previousMouseState = mouseState;
            previousMousePosition = MouseVector2;
            mouseState = Mouse.GetState();

            //Keyboard events.
            if (keyboardState != previousKeyboardState)
            {
                if (keyboardState.GetPressedKeys().Length > previousKeyboardState.GetPressedKeys().Length)
                    if (OnKeyDown != null) OnKeyDown(keyboardState);
                if (keyboardState.GetPressedKeys().Length < previousKeyboardState.GetPressedKeys().Length)
                    if (OnKeyUp != null) OnKeyUp(previousKeyboardState);
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

        public bool KeyPressUnique(Keys key)
        {
            return (keyboardState.IsKeyDown(key) &&
                    previousKeyboardState.IsKeyUp(key));
        }

        public bool KeyPressRelease(Keys key)
        {
            return (previousKeyboardState.IsKeyDown(key) &&
                    keyboardState.IsKeyUp(key));
        }

        public bool LeftClickUnique()
        {
            return (mouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released);
        }

        public bool LeftClickRelease()
        {
            return (mouseState.LeftButton == ButtonState.Released &&
                    previousMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool RightClickUnique()
        {
            return (mouseState.RightButton == ButtonState.Pressed &&
                    previousMouseState.RightButton == ButtonState.Released);
        }

        public bool RightClickRelease()
        {
            return (mouseState.RightButton == ButtonState.Released &&
                    previousMouseState.RightButton == ButtonState.Pressed);
        }

        public bool MiddleClickUnique()
        {
            return (mouseState.MiddleButton == ButtonState.Pressed &&
                    previousMouseState.MiddleButton == ButtonState.Released);
        }

        public bool MiddleClickRelease()
        {
            return (mouseState.MiddleButton == ButtonState.Released &&
                    previousMouseState.MiddleButton == ButtonState.Pressed);
        }

        public bool MouseWheelForwards()
        {
            return (mouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue);
        }

        public bool MouseWheelReverse()
        {
            return (mouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue);
        }

        public float MouseWheelVelocity()
        {
            return MathHelper.Clamp(
                (mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) * 0.20f, -1, 1);
        }

        public PhysicalItem MouseHoverPhysicalItem(World world)
        {
            try
            {
                selectedItem = (PhysicalItem)world.TestPoint(
                    ConvertUnits.ToSimUnits(
                        MouseVector2)).Body.UserData;

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

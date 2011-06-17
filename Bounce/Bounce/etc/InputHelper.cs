using System;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public static class InputHelper
    {
        public static bool KeyPressUnique(Keys key)
        {
            return (BounceGame.KeyboardState.IsKeyDown(key) && BounceGame.PreviousKeyboardState.IsKeyUp(key))
                ? true : false;
        }

        public static bool KeyPressRelease(Keys key)
        {
            return (BounceGame.PreviousKeyboardState.IsKeyDown(key) && BounceGame.KeyboardState.IsKeyUp(key))
                ? true : false;
        }

        public static bool LeftClickUnique()
        {
            return (BounceGame.MouseState.LeftButton == ButtonState.Pressed &&
                    BounceGame.PreviousMouseState.LeftButton == ButtonState.Released)
                ? true : false;
        }

        public static bool LeftClickRelease()
        {
            return (BounceGame.MouseState.LeftButton == ButtonState.Released && BounceGame.PreviousMouseState.LeftButton == ButtonState.Pressed)
                ? true : false;
        }

        public static bool RightClickUnique()
        {
            return (BounceGame.MouseState.RightButton == ButtonState.Pressed && BounceGame.PreviousMouseState.RightButton == ButtonState.Released)
                ? true : false;
        }

        public static bool RickClickRelease()
        {
            return (BounceGame.MouseState.RightButton == ButtonState.Released && BounceGame.PreviousMouseState.RightButton == ButtonState.Pressed)
                ? true : false;
        }
    }
}

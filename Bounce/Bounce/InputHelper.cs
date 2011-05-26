using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Controllers;


namespace Bounce
{
    public static class InputHelper
    {
        public static bool KeyPressUnique(Keys key)
        {
            return (BounceGame.KeyboardState.IsKeyDown(key) && BounceGame.PreviousKeyboardState.IsKeyUp(key))
                ?
                    true
                    :
                    false;
        }

        public static bool KeyPressRelease(Keys key)
        {
            return (BounceGame.PreviousKeyboardState.IsKeyDown(key) && BounceGame.KeyboardState.IsKeyUp(key))
                ?
                    true
                    :
                    false;
        }

        public static bool LeftClickUnique()
        {
            return (BounceGame.MouseState.LeftButton == ButtonState.Pressed && BounceGame.PreviousMouseState.LeftButton == ButtonState.Released) == true
                ?
                    true
                    :
                    false;
        }

        public static bool LeftClickRelease()
        {
            return (BounceGame.MouseState.LeftButton == ButtonState.Released && BounceGame.PreviousMouseState.LeftButton == ButtonState.Pressed) == true
                ?
                    true
                    :
                    false;
        }

        public static bool RightClickUnique()
        {
            return (BounceGame.MouseState.RightButton == ButtonState.Pressed && BounceGame.PreviousMouseState.RightButton == ButtonState.Released) == true
                ?
                    true
                    :
                    false;
        }

        public static bool RickClickRelease()
        {
            return (BounceGame.MouseState.RightButton == ButtonState.Released && BounceGame.PreviousMouseState.RightButton == ButtonState.Pressed) == true
                ?
                    true
                    :
                    false;
        }
    }
}

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

namespace Bounce
{
    class Camera2D// : GameComponent
    {
        protected float zoom;
        protected float rotation;
        public Camera2D()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            Position = Vector2.Zero;
        }
        //public Camera2D(Game game)
        //    : base(game)
        //{
        //    game.Components.Add(this);
        //}

        private Vector2 cameraPosition;

        public Matrix Transform;
        public Vector2 Position
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        public float Rotation
        {
            get { return Rotation; }
            set { rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }
    }
}

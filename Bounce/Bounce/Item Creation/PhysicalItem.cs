using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public abstract class PhysicalItem
    {
        protected World world;
        protected Scene scene;
        protected static Random r = new Random();
        public Body Body;
        public bool IsAlive { get; protected set; }
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        protected Color drawColor = Color.White;
        protected Vector2 origin;
        public IndexKey IndexKey;

        public KeyboardEvent OnKeyDown { get { return _onKeyDown; } set { Input.OnKeyDown += value; _onKeyDown += value; } }
        public KeyboardEvent OnKeyHoldDown { get { return _onKeyHoldDown; } set { Input.OnKeyHoldDown += value; _onKeyDown += value; } }
        public KeyboardEvent OnKeyUp { get { return _onKeyUp; } set { Input.OnKeyUp += value; _onKeyUp += value; } }
        protected KeyboardEvent _onKeyDown;
        protected KeyboardEvent _onKeyHoldDown;
        protected KeyboardEvent _onKeyUp;

        public MouseEvent OnLeftClickDown { get { return _onLeftClickDown; } set { Input.OnLeftClickDown += value; _onLeftClickDown += value; } }
        public MouseEvent OnLeftClickUp { get { return _onLeftClickUp; } set { Input.OnLeftClickUp += value; _onLeftClickUp += value; } }
        public MouseEvent OnRightClickDown { get { return _onRightClickDown; } set { Input.OnRightClickDown += value; _onRightClickDown += value; } }
        public MouseEvent OnRightClickUp { get { return _onRightClickUp; } set { Input.OnRightClickUp += value; _onRightClickUp += value; } }
        public MouseEvent OnMiddleClickDown { get { return _onMiddleClickDown; } set { Input.OnMiddleClickDown += value; _onMiddleClickDown += value; } }
        public MouseEvent OnMiddleClickUp { get { return _onMiddleClickUp; } set { Input.OnMiddleClickUp += value; _onMiddleClickUp += value; } }
        public MouseEvent OnMouseHover { get { return _onMouseHover; } set { Input.OnMouseHover += value; _onMouseHover += value; } }
        public MouseEvent OnMouseWheel { get { return _onMouseWheel; } set { Input.OnMouseWheel += value; _onMouseWheel += value; } }
        protected MouseEvent _onLeftClickDown;
        protected MouseEvent _onLeftClickUp;
        protected MouseEvent _onRightClickDown;
        protected MouseEvent _onRightClickUp;
        protected MouseEvent _onMiddleClickDown;
        protected MouseEvent _onMiddleClickUp;
        protected MouseEvent _onMouseHover;
        protected MouseEvent _onMouseWheel;

        public PhysicalItem(Scene scene, World world)
        {
            this.world = world;
            this.scene = scene;
            this.IsAlive = true;

            world.ContactManager.OnBroadphaseCollision += OnBroadphaseCollision;
            OnRightClickDown += delegate(int ID, MouseState mouseState)
            {
                if (this.IndexKey == ID)
                    if (Input.KeyboardState.IsKeyDown(Keys.Delete)) Kill();
            };
        }

        public virtual void OnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2) { }

        public virtual void Kill()
        {
            this.IsAlive = false;
            Body.Dispose();

            Input.OnKeyDown -= OnKeyDown;
            Input.OnKeyHoldDown -= OnKeyHoldDown;
            Input.OnKeyUp -= OnKeyUp;

            Input.OnLeftClickDown -= OnLeftClickDown;
            Input.OnLeftClickUp -= OnLeftClickUp;
            Input.OnRightClickDown -= OnRightClickDown;
            Input.OnRightClickUp -= OnRightClickUp;
            Input.OnMiddleClickDown -= OnMiddleClickDown;
            Input.OnMiddleClickUp -= OnMiddleClickUp;
            Input.OnMouseHover -= OnMouseHover;
            Input.OnMouseWheel -= OnMouseWheel;
        }

        public virtual void Update(GameTime gametime)
        {
            if (Body.IsDisposed)
                this.IsAlive = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(
                    Texture,
                    ConvertUnits.ToDisplayUnits(Body.Position),
                    null,
                    drawColor,
                    Body.Rotation,
                    origin,
                    1f,
                    spriteEffects,
                    0 + scene.SceneDepth);
        }
    }
}

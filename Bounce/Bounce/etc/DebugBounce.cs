using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.DebugViews;

namespace Bounce
{
    public class DebugBounce : Microsoft.Xna.Framework.GameComponent
    {
        DebugViewXNA DebugViewXNA;
        Game game;
        public DebugBounce(Game game)
            : base(game)
        {
            this.game = game;
            DebugViewXNA = new DebugViewXNA(BounceGame.World);

            game.Components.Add(this);
        }

        private Matrix projection, view;

        public override void Initialize()
        {
            DebugViewXNA.LoadContent(game.GraphicsDevice, game.Content);
            DebugViewXNA.RemoveFlags(DebugViewFlags.Shape);

            projection = Matrix.CreateOrthographic(
                BounceGame.Graphics.PreferredBackBufferWidth / 100.0f,
                -BounceGame.Graphics.PreferredBackBufferHeight / 100.0f, 0, 1000000);

            Vector3 campos = new Vector3();
            campos.X = (-BounceGame.Graphics.PreferredBackBufferWidth / 2) / 100.0f;
            campos.Y = (BounceGame.Graphics.PreferredBackBufferHeight / 2) / -100.0f;
            campos.Z = 0;
            Matrix tran = Matrix.Identity;
            tran.Translation = campos;
            view = tran;

            base.Initialize();
        }
       
        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            base.Update(gameTime);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
            {
                if (InputHelper.KeyPressUnique(Keys.F1))
                {
                    EnableOrDisableFlag(DebugViewFlags.Shape);
                }
                if (InputHelper.KeyPressUnique(Keys.F2))
                {
                    EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                    EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
                }
                if (InputHelper.KeyPressUnique(Keys.F3))
                {
                    EnableOrDisableFlag(DebugViewFlags.Joint);
                }
                if (InputHelper.KeyPressUnique(Keys.F4))
                {
                    EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                    EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                }
                if (InputHelper.KeyPressUnique(Keys.F5))
                {
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
                }
                if (InputHelper.KeyPressUnique(Keys.F6))
                {
                    EnableOrDisableFlag(DebugViewFlags.Controllers);
                }
                if (InputHelper.KeyPressUnique(Keys.F7))
                {
                    EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
                }
                if (InputHelper.KeyPressUnique(Keys.F8))
                {
                    EnableOrDisableFlag(DebugViewFlags.AABB);
                }
            }
        }

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugViewXNA.Flags & flag) == flag)
            {
                DebugViewXNA.RemoveFlags(flag);
            }
            else
            {
                DebugViewXNA.AppendFlags(flag);
            }
        }

        public void Draw()
        {
            DebugViewXNA.RenderDebugData(ref projection, ref view);
        }
    }
}

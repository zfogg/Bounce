using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    public class BounceContentManager : ContentManager
    {
        public BounceContentManager(IServiceProvider services)
            :base(services)
        {
        }
    }
}

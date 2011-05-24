using System;

namespace Bounce
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BounceGame game = new BounceGame())
            {
                game.Run();
            }
        }
    }
#endif
}


using System;

/* SUPER SMASH BROS. BRAWL 3D MODEL VIEWER
 * Programmers: Michael Yoon Huh (huhx0015), Steve Chou (azntofu2000/chou0069)
 * Last Updated: 12/15/2011 */

namespace _3DPoringModel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/* SUPER SMASH BROS. BRAWL 3D MODEL VIEWER
 * Programmers: Michael Yoon Huh (huhx0015), Steve Chou (azntofu2000/chou0069)
 * Last Updated: 12/15/2011 */

namespace _3DPoringModel
{
    public class worldobj
    {
        public Vector2 Velocity;
        public Vector2 Center;
        public BoundingBox c1;
        public Vector2 spritePosition;
        public int width;
        public int height;
        //public bool selected;
        public bool show;
        public Color tint;
    }
}

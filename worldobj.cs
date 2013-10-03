using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

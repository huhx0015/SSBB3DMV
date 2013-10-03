using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _3DPoringModel
{
    // "ball" class. Code by azntofu2000.
    public class ball
    {
        public Vector3 distance;
        public float velocity;
        public float acc;
        public float time;
        public bool isfalling;
        public bool reverse;
        public float Vinit;
        public bool hasBounced;
        public bool finishBouncing;
        private int movementVector;
        public int counter;

        /// <summary>
        /// Starting clockwise on the 3 o'clock position with a total of 8 vectors.
        /// 0 = east
        /// 1 = NE
        /// 2 = North
        /// 3 = NW
        /// 4 = W
        /// 5 = SW
        /// 6 = S
        /// 7 = SE
        /// </summary>
        public int Direction
        {
            get { return movementVector; }
            set { movementVector = value; }
        }
        

    }
   
    // "modelMatrix" class.
    public class modelMatrix
    {
        public Matrix worldMatrix;
        public Matrix projectionMatrix;
        public Matrix viewMatrix;
        
        public Matrix setWorld
        {
            get { return worldMatrix; }
            set { worldMatrix = value; }
        }
        public Matrix setProjection
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }
        public Matrix setView
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }
    }

    // "ModelObject" class.
    public class ModelObject
    {
        public Model model;
        public Texture2D[] textures;
        public Vector3 startPos;
        public Vector3 direction;
        public ball gravity;
        public Matrix matrix;
        public Vector3 yloc;
    }

    // "Drawing": This is the drawing class for the Super Smash Bros. Brawl Model Viewer application. This class is responsible for drawing and animating the model.
    public class Drawing : Microsoft.Xna.Framework.Game
    {
        // Ball motion global variables.
        public float Scale = 0.5f;

        // "DrawModel()": Draws the model and textures for the model.
        public void DrawModel(modelMatrix modMatrix, ModelObject theModel)
        {
            Matrix[] transforms = new Matrix[theModel.model.Bones.Count];
            theModel.model.CopyAbsoluteBoneTransformsTo(transforms);
            int j = 0;

            foreach (ModelMesh mesh in theModel.model.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix wMatrix = transforms[mesh.ParentBone.Index] * modMatrix.worldMatrix; // Model + motion.
                    //Matrix wMatrix = transforms[mesh.ParentBone.Index]; // Model draw only.
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"]; // Specifies the loading of textures onto model.
                    currentEffect.Parameters["xWorld"].SetValue(wMatrix);
                    currentEffect.Parameters["xView"].SetValue(modMatrix.viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(modMatrix.projectionMatrix);
                    currentEffect.Parameters["xEnableLighting"].SetValue(true); // Enables lighting.
                    currentEffect.Parameters["xLightDirection"].SetValue(new Vector3(3, -2, 5)); // Sets the direction of the light.
                    currentEffect.Parameters["xAmbient"].SetValue(0.5f);
                    currentEffect.Parameters["xTexture"].SetValue(theModel.textures[j++]); // Apply the textures.
                }
                mesh.Draw(); // Draws the mesh.
            }
        }

    }
}
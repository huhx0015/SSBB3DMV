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

// AI algorithms and programming written by chou0069.
namespace _3DPoringModel
{
    class AI
    {
        //float Bmax = 15;
        float Bmin = 0;
        //private int counter = 0;

        // "modelBounce()": Calculates the bouncing motion for model objects.
        public Vector3 modelBounce(ball myballs)
        {
            float increment = 0.1f;
            // Main code for calculating the velocity and acceleration of the bouncing object. (v = vo + at)
            myballs.time++;
            myballs.distance.Y += (myballs.Vinit + (float)(0.2 * myballs.acc * myballs.time * myballs.time));
            myballs.velocity += myballs.acc * myballs.time;

            // Scenario 1: Object is falling.
            // Hits the floor.
            if (myballs.distance.Y < Bmin && myballs.isfalling == true)
            {
                myballs.isfalling = false;
                myballs.Vinit = myballs.velocity;
                myballs.distance.Y = Bmin;
                myballs.hasBounced = true;
                myballs.finishBouncing = true;
                //myballs.Vinit *= -1;

                // 29 prevents it from sinking, 30 keeps it bouncing for a while and anything less eventually brings it to a stop.
                //myballs.time = 34;
            }
            // In the process of falling down...
            else if (myballs.distance.Y > Bmin && myballs.isfalling == true)
            {
            }

            /*Antiquated code: use if you need a bouncing object. 
            // Hits the ceiling.
            if (myballs.distance > Bmax && myballs.isfalling != true)
            {
                myballs.distance = Bmax;
                myballs.isfalling = true;
                myballs.time = 0;
                myballs.Vinit = 0;
                myballs.velocity = 0;
            }

            // In the process of rising.
            else if (myballs.distance < Bmax && myballs.isfalling != true)
            {
            }
            */

            //checker to prevent it from falling through the floor. 
            if (myballs.Vinit != 0 && myballs.Vinit + (float)(0.5 * myballs.acc * myballs.time * myballs.time) < 0)
            {
                myballs.isfalling = true;
                myballs.time = 0;
                myballs.Vinit = 0;
                myballs.velocity = 0;
            }

            if (!myballs.finishBouncing)
            {
                switch (myballs.Direction)
                {
                    case 0:
                        myballs.distance.X += increment;

                        break;
                    case 1:
                        myballs.distance.Z += increment;
                        myballs.distance.X += increment;
                        break;
                    case 2:
                        myballs.distance.Z += increment;
                        break;
                    case 3:
                        myballs.distance.Z += increment;
                        myballs.distance.X -= increment;
                        break;
                    case 4:
                        myballs.distance.X -= increment;
                        break;
                    case 5:
                        myballs.distance.Z -= increment;
                        myballs.distance.X -= increment;
                        break;
                    case 6:
                        myballs.distance.Z -= increment;
                        break;
                    case 7:
                        myballs.distance.Z -= increment;
                        myballs.distance.X += increment;
                        break;
                }
            }
            return myballs.distance;
        }

        // "fullGravity()": Takes the ball class and calculates the bounce for a given amount of bounces.
        public Vector3 fullGravity(ball myballs, int bounces)
        {

            //infinite bouncing, don't increment the counter. 
            if (bounces == -1 && myballs.hasBounced)
            {
                myballs.Vinit = 0.8f;
                myballs.velocity = 0;
                myballs.hasBounced = false;
                return modelBounce(myballs); // Calculate the bounce motion for models.
            }

            //bounced but not at its limits yet so we increment the counter. 
            else if (myballs.hasBounced && myballs.counter < bounces)
            {
                myballs.Vinit = 0.8f;
                myballs.velocity = 0;
                myballs.hasBounced = false;
                myballs.counter++;
                return myballs.distance;
                //return modelBounce(myballs);
            }
                //else just bounce it.   
            else
            {
                return modelBounce(myballs); // Calculate the bounce motion for models.
            }

        }

        // [IN PROGRESS] Auto motion: determine how to move the object randomly. 
        float timer = 0.0f;
    
        public ModelObject autoMotion(ModelObject myball, int seed)
        {
            Random random = new Random(seed);
            //GameTime gameTime = new GameTime();
            float waitTime = 400;
            //timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            
            if (myball.gravity.finishBouncing == true)
            {
                myball.gravity.Direction = random.Next(0, 7);
                myball.gravity.finishBouncing = false;
            }

            //-1 = bounce infinitely
            myball.yloc = fullGravity(myball.gravity, -1);

            if (timer < waitTime)
            {
            }
            
            //hop in place
            

            //increment timer and set Y direction to equal to gravity calculation. 
            //timer++;
            myball.direction = myball.yloc;
            return myball;
        }
    }
}

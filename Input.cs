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

/* SUPER SMASH BROS. BRAWL 3D MODEL VIEWER
 * Programmers: Michael Yoon Huh (huhx0015), Steve Chou (azntofu2000/chou0069)
 * Last Updated: 12/15/2011 */

// Input functionality programmed by huhx0015.
namespace _3DPoringModel
{
    public class Input : Microsoft.Xna.Framework.Game
    {
        // Input global variables.
        public KeyboardState keyState; // Keyboard state.
        MouseState mPreviousMouseState; // Previous mouse state.
        public Vector3 cameraPosition = Vector3.Zero;
        public bool isCameraUpdate = false;

        // Font global variables.
        public bool isStringDisplay = false;
        public String displayString;

        // "UpdateInput()": Retrives keyboard input and performs the appropriate action.
        public void UpdateInput(Vector3 cam)
        {
            KeyboardState newState = Keyboard.GetState();
            float cameraPos = 1.0f; // Used for adjusting the camera position.
            isCameraUpdate = false;
            cameraPosition = cam;

            // If the Spacebar Key is pressed...
            if (newState.IsKeyDown(Keys.Space))
            {
                // If not down in last update, the key was just pressed.
                if (!keyState.IsKeyDown(Keys.Space))
                {
                    soundEffects("Jump"); // Outputs a Mario jump sound.
                }
            }

            // If the Right Key is pressed...
            if (newState.IsKeyDown(Keys.Right))
            {
                isCameraUpdate = true;
                cameraPosition.X = cameraPosition.X + cameraPos; // Moves the camera to the right.
            }

            // If the Left Key is pressed...
            if (newState.IsKeyDown(Keys.Left))
            {
                isCameraUpdate = true;
                cameraPosition.X = cameraPosition.X - cameraPos; // Moves the camera to the left.
            }

            // If the Up Key is pressed...
            if (newState.IsKeyDown(Keys.Up))
            {
                isCameraUpdate = true;
                cameraPosition.Y = cameraPosition.Y + cameraPos; // Moves the camera down.
            }

            // If the Down Key is pressed...
            if (newState.IsKeyDown(Keys.Down))
            {
                isCameraUpdate = true;
                cameraPosition.Y = cameraPosition.Y - cameraPos; // Moves the camera up.
            }

            // If the Page Up Key is pressed...
            if (newState.IsKeyDown(Keys.PageUp))
            {
                isCameraUpdate = true;
                cameraPosition.Z = cameraPosition.Z + cameraPos; // Moves the camera in the positive Z-direction.

            }

            // If the Page Down Key is pressed...
            if (newState.IsKeyDown(Keys.PageDown))
            {
                isCameraUpdate = true;
                cameraPosition.Z = cameraPosition.Z - cameraPos; // Moves the camera in the negative Z-direction.
            }

            // If the Left Control Key is pressed...
            if (newState.IsKeyDown(Keys.LeftControl))
            {
                // If not down in last update, the key was just pressed.
                if (!keyState.IsKeyDown(Keys.LeftControl))
                {
                    displayString = "MUSIC DISABLED.";
                    MediaPlayer.Stop();
                }
            }

            // If the Left Alt Key is pressed...
            if (newState.IsKeyDown(Keys.LeftAlt))
            {
                // If not down in last update, the key was just pressed.
                if (!keyState.IsKeyDown(Keys.LeftAlt))
                {
                }
            }

            // If the 'M' key is pressed...
            if (newState.IsKeyDown(Keys.M))
            {
                // If not down in last update, the key was just pressed.
                if (!keyState.IsKeyDown(Keys.M))
                {
                }
            }


            // If the 'N' key is pressed...
            if (newState.IsKeyDown(Keys.N))
            {
                // If not down in last update, the key was just pressed.
                if (!keyState.IsKeyDown(Keys.N))
                {
                }
            }

            // If the 'Delete' key is pressed...
            if (newState.IsKeyDown(Keys.Delete))
            {
            }

            keyState = newState;
        }
       
        // "musicPlay()": Plays music files (.mp3) specified by the name of the song in string format.
        public void musicPlay(string songs)
        {
            Song music; // Music Object.
            ContentManager contentManager = new ContentManager(this.Services, @"Content\Music"); // Creates a new ContentManager object and specifies the path to the music.
            music = contentManager.Load<Song>(songs); // Load the specified song into contentManager.
            MediaPlayer.Play(music); // Plays the specified music using the built-in MediaPlayer.
            displayString = "MUSIC ENABLED";
        }

        // "soundEffects()": Plays sound files specified by the name of the sound in string format.
        public void soundEffects(string sound)
        {
            SoundEffect soundEffect; // Audio object.
            ContentManager contentManager = new ContentManager(this.Services, @"Content\Sounds"); // Creates a new ContentManager object and specifies the path to the sounds.
            soundEffect = contentManager.Load<SoundEffect>(sound); // Load the specified sound into contentManager.
            soundEffect.Play(); // Play the specified sound effect.
        }
           
    }
}
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

/* SUPER SMASH BROS. BRAWL 3D MODEL VIEWER
 * Programmers: Michael Yoon Huh (huhx0015), Steve Chou (azntofu2000/chou0069)
 * Last Updated: 12/15/2011 */

// Menu GUI interface programming by chou0069. Modified by huhx0015.
namespace _3DPoringModel{

    public class Menu : Microsoft.Xna.Framework.Game
    { 
        //private static int MAX = 20;
        //private static int MAXLEVEL = 2;

        //Base menu
        private List<worldobj> m_base;
        //sub menus
        private List<worldobj> m_lstItem1;
        private List<worldobj> m_lstItem2;
        private List<worldobj> m_lstItem3;
        private List<worldobj> m_lstItem4;
        private List<worldobj> m_lstItem5;
        private Vector2 curpos;
        private Rectangle[] m_rcSource, subSource;

        //Textures for the menu items. 
        Texture2D menuicon;
        Texture2D Sub1;
        Texture2D Sub2;
        Texture2D Sub3;
        Texture2D Sub4;
        Texture2D Sub5;

        //Frame information for the texture. 
        Rectangle mainFrame;
        worldobj obj1;

        //scale for the items. 
        private Vector2 m_vScale;

        //color items
        MouseState mPreviousMouseState; // Previous mouse state.

        Input menu = new Input(); // Input object for actions triggered by the menu.

        // Publically accessible variables.
        public bool bounceSwitch = false; // Used for enabling the bouncing option.
        public string songString = " "; // Stores the string for the currently played song.
        public bool songEnabled = false; // Used for displaying the song name onscreen.
        public bool stageSwitch = false; // Used for switching stages.

        // Song list array variables.
        int songCount = 0;
        int songCountMax = 9;
        string[] songList = new string[9] { "Battlefield", "Battlefield_II", "Final_Destination", "Final_Destination_II", "Final_Destination_III", "Final_Destination_IV", "Menu_1", "Menu_2", "Menu_3" };
        string[] songListName = new string[9] { "SONG: Battlefield [Super Smash Bros. Brawl]",
                                                "SONG: Battlefield II [Super Smash Bros. Brawl]",
                                                "SONG: Final Destination [Super Smash Bros. Brawl]",
                                                "SONG: Final Destination II [Super Smash Bros. Brawl]",
                                                "SONG: Final Destination (Another) [Super Smash Bros. Brawl]",
                                                "SONG: Final Destination [Super Smash Bros. Melee]",
                                                "SONG: Menu 1 [Super Smash Bros. Brawl]",
                                                "SONG: Menu 1 [Super Smash Bros. Melee]",
                                                "SONG: Menu 2 [Super Smash Bros. Brawl]"};

        // Sound list array variables.
        int soundCount = 0;
        int soundCountMax = 6;
        string[] soundList = new string[6] { "MarioSFX", "DonkeyKongSFX", "KirbySFX", "NessSFX", "PeachSFX", "FoxSFX" };

        public Menu()
        {
            m_base = new List<worldobj>();
        }

        //initializer
        public void init()
        {
            m_base = new List<worldobj>();
            m_lstItem1 = new List<worldobj>();
            m_lstItem2 = new List<worldobj>();
            m_lstItem3 = new List<worldobj>();
            m_lstItem4 = new List<worldobj>();
            m_lstItem5 = new List<worldobj>();

            curpos = new Vector2(0, 0);
            obj1 = new worldobj();
            obj1.spritePosition = Vector2.Zero;
       
        }

        public Rectangle[] Source
        {
            get { return m_rcSource; }
            set { m_rcSource = value; }
        }

        public Rectangle[] Submenu
        {
            get { return subSource; }
            set { subSource = value; }
        }

        public Rectangle Frame
        {
            get { return mainFrame; }
            set { mainFrame = value; }
        }


        //add an item to the index
        public void Add(worldobj item)
        {
            m_base.Add(item);
        }

        public void AddSub1(worldobj item)
        {
            m_lstItem1.Add(item);
        }

        public void AddSub2(worldobj item)
        {
            m_lstItem2.Add(item);
        }

        public void AddSub3(worldobj item)
        {
            m_lstItem3.Add(item);
        }

        public void AddSub4(worldobj item)
        {
            m_lstItem4.Add(item);
        }

        public void AddSub5(worldobj item)
        {
            m_lstItem5.Add(item);
        }

        public void checkTint(MouseState aMouse)
        {
            for (int i = 0; i < m_base.Count; i++)
            {
                m_base[i].tint.R = 255;
                m_base[i].tint.G = 255;
                m_base[i].tint.B = 255;
                m_base[i].tint.A = 255;

                //mouse position > menu base sprite's left x, x + width, and Y + height. 
                if ((aMouse.X > m_base[i].spritePosition.X) &&
                    (aMouse.X < m_base[i].spritePosition.X + m_base[i].width) &&
                    (aMouse.Y < m_base[i].spritePosition.Y + m_base[i].height)
                    )
                {
                    m_base[i].tint.R = 255;
                    m_base[i].tint.G = 55;
                    m_base[i].tint.B = 155;
                    m_base[i].tint.A = 255;

                    // If a user presses one of the menu buttons...
                    if (aMouse.LeftButton == ButtonState.Pressed && mPreviousMouseState.LeftButton == ButtonState.Released)
                    {
                        switch(i){

                            case 0:

                                // Action for Button 1 (Controls). Enables and disables the characters' bouncing motion.
                                menu.soundEffects("Menu"); // Outputs the menu select sound when button is pressed.

                                // Enable character bouncing.
                                if (bounceSwitch == false)
                                {
                                    bounceSwitch = true;
                                }
                                // Disable character bouncing.
                                else if (bounceSwitch == true)
                                {
                                    bounceSwitch = false;
                                }

                                break;
                            case 1:

                                // Action for Button 2 (Deflicker). Changes the stage and background.
                                menu.soundEffects("Menu"); // Outputs the menu select sound when button is pressed.

                                // If the current stage is Stadium (default stage), switch to Final Destination.
                                if (stageSwitch == false)
                                {
                                    stageSwitch = true;
                                }
                                // If the current stage is Final Destination, switch to Stadium.
                                else if (stageSwitch == true)
                                {
                                    stageSwitch = false;
                                }

                            break;
                            case 2:
                                // Action for Button 3 (Sound).
                                menu.soundEffects("Menu"); // Outputs the menu select sound when button is pressed.
                                for (int j = 0; j < m_lstItem3.Count; j++)
                                {
                                    if (m_lstItem3[j].show)
                                    {
                                        m_lstItem3[j].show = false;
                                    }
                                    else
                                    {
                                        m_lstItem3[j].show = true;
                                    }
                                    m_lstItem2[j].show = false;
                                    m_lstItem1[j].show = false;
                                    m_lstItem4[j].show = false;
                                    m_lstItem5[j].show = false;
                                }
                                break;
                            case 3:

                                // Action for Button 4 (My Music). Changes the song being played.
                                menu.soundEffects("Menu"); // Outputs the menu select sound when button is pressed.

                                // If songCount is greater than songCountMax, reset the count to 0.
                                if (songCount >= songCountMax)
                                {
                                    songCount = 0;
                                }
                                menu.musicPlay(songList[songCount]);
                                songString = songListName[songCount]; // Play the specified music file in the songList.
                                songEnabled = true;
                                songCount++;

                            break;
                            case 4:
                                // Action for Button 5 (My Music). Outputs a narrator sound effect.
                                menu.soundEffects("Menu"); // Outputs the menu select sound when button is pressed.

                                // If soundCount is greater than soundCountMax, reset the count to 0.
                                if (soundCount >= soundCountMax)
                                {
                                    soundCount = 0;
                                }
                                menu.soundEffects(soundList[soundCount]);
                                soundCount++;

                                break;
                        }
                    }
                }
            }
            mPreviousMouseState = aMouse;  // Store the previous mouse state

        }

        public Texture2D Texture
        {
            get { return menuicon; }
            set { menuicon = value; }
        }

        public Texture2D setSub1
        {
            get { return Sub1; }
            set { Sub1 = value; }
        }
        public Texture2D setSub2
        {
            get { return setSub2; }
            set { Sub2 = value; }
        }
        public Texture2D setSub3
        {
            get { return Sub3; }
            set { Sub3 = value; }
        }
        public Texture2D setSub4
        {
            get { return Sub4; }
            set { Sub4 = value; }
        }
        public Texture2D setSub5
        {
            get { return Sub5; }
            set { Sub5 = value; }
        }

        //remove the item from the menu
        public void RemoveAt(int index)
        {
            m_base.RemoveAt(index);
        }

        //public void setgraphic(Texture2D newtexture)
        //{
            //m_circleTex = newtexture;
        //}

        public void draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            for (int i = 0; i < m_base.Count; i++)
            {
                //draw the object if its enabled
                if (m_base[i].show)
                {
                    spritebatch.Draw(menuicon, new Vector2(m_base[i].spritePosition.X, m_base[i].spritePosition.Y),
                                    m_rcSource[i], m_base[i].tint); // Draws the background.
                }
            }

            //submenu checkers
            for (int i = 0; i < m_lstItem1.Count; i++)
            {
                //draw the object if its enabled
                if (m_lstItem1[i].show)
                {
                    m_vScale = new Vector2(0.5f);
                    spritebatch.Draw(Sub1, new Vector2(m_lstItem1[i].spritePosition.X, m_lstItem1[i].spritePosition.Y),
                                    subSource[i], m_lstItem1[i].tint, 0f, Vector2.Zero, m_vScale, SpriteEffects.None, 0f); // Draws the background.
                }
                if (m_lstItem2[i].show)
                {
                    spritebatch.Draw(Sub1, new Vector2(m_lstItem2[i].spritePosition.X, m_lstItem2[i].spritePosition.Y),
                                    subSource[i], m_lstItem2[i].tint); // Draws the background.
                }
                if (m_lstItem3[i].show)
                {
                    spritebatch.Draw(Sub1, new Vector2(m_lstItem3[i].spritePosition.X, m_lstItem3[i].spritePosition.Y),
                                    subSource[i], m_lstItem3[i].tint); // Draws the background.
                }
                if (m_lstItem4[i].show)
                {
                    spritebatch.Draw(Sub1, new Vector2(m_lstItem4[i].spritePosition.X, m_lstItem4[i].spritePosition.Y),
                                    subSource[i], m_lstItem4[i].tint); // Draws the background.
                }
                if (m_lstItem5[i].show)
                {
                    spritebatch.Draw(Sub1, new Vector2(m_lstItem5[i].spritePosition.X, m_lstItem5[i].spritePosition.Y),
                                    subSource[i], m_lstItem5[i].tint); // Draws the background.
                }

            }


            //checking to see if we have a drag and drop event?


        }

        //pass in the mouse position and the list we're checking
        public void DragThis(MouseState aMouse, List<worldobj> dragList)
        {
            //go through the list
            for (int i = 0; i < dragList.Count; i++)
            {
                //check if the position is right. 
                if ((aMouse.X > dragList[i].spritePosition.X) &&
                    (aMouse.X < dragList[i].spritePosition.X + dragList[i].width) &&
                    (aMouse.Y < dragList[i].spritePosition.Y + dragList[i].height))
                {
                    //draw an additional texture thing here. 

                }
                //if not here then i guess we don't drag
            }

        }

    }
}

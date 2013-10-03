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

// Graphics initialization and transformation programming, texture loading, and input functionality by huhx0015.
// Menu GUI interface and AI algorithms programming by chou0069.
namespace _3DPoringModel
{
    // "Game1": This is the main class for the 3D Poring Model application. All initialization and main program functionality exists here.
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Graphics-related global variables.
        Color backColor = Color.White; // Background color.
        protected Color tint;
        Effect effect; // Effect object.
        GraphicsDevice device; // Graphics device.
        GraphicsDeviceManager graphics; // Graphics device manager object.
        SpriteBatch spriteBatch, spriteBatch1; // Sprite batch.

        // Input global variables.
        KeyboardState keyState; // Keyboard state.
        MouseState mPreviousMouseState; // Previous mouse state.
        Input inputState = new Input();

        // Title screen global variables.
        Texture2D titleScreen; // Instance of Texture2D that contains the texture for the title screen on program start.
        Texture2D loadScreen; // Instance of Texture2D that contains the loading screen.
        bool loadStart = true; // Flag variable used for loading and unloading the title screen.
        bool loadComplete = false; // Flag variable used for loading and unloading the title screen.
        bool loading = false; // Flag variable used for loading and unloading the title screen.

        // 2D image and menu global variables.
        public float Scale = 0.5f;
        Rectangle mainFrame; // A Rectangle that defines the limits for the main game screen.
        Texture2D background, fdbackground, currentbackground; // Instances of Texture2D that will contain the 2D textures for the stages.

        // 3D World Space global variables.
        float aspectRatio; // The aspect ratio determines how to scale 3D to 2D projection.
        Vector3 cameraPosition = new Vector3(0, 0, 250); // Initialize the camera position.
        modelMatrix mMatrix = new modelMatrix(); // The main matrix used for drawing and displaying models in the 3D model space.
        modelMatrix mMatrix1 = new modelMatrix(); // The main matrix used for drawing and displaying models in the 3D model space.
        modelMatrix mMatrix2 = new modelMatrix(); // The main matrix used for drawing and displaying models in the 3D model space.
        modelMatrix mMatrix3 = new modelMatrix(); // The main matrix used for drawing and displaying models in the 3D model space.
        Drawing drawModels = new Drawing(); // The object that references the classes in Drawing1.cs.
        AI objAI = new AI();
        AI objAI1 = new AI();
        AI objAI2 = new AI();
        AI objAI3 = new AI();
        AI objAI4 = new AI();
        AI objAI5 = new AI();

        // Stage global variables.
        ModelObject Skybox;// Stadium variables.
        ModelObject Skybox1; // Stadium variables.
        ModelObject FinalDest; // Final Destination variables.

        // Character model variables.
        ModelObject Mario;
        ModelObject Mario1; // Alternate Mario in Blue.
        ModelObject Mario2; // Alternate Mario in White.
        ModelObject Mario3; // Alternate Mario in Yellow.
        ModelObject DonkeyKong;
        ModelObject Kirby;
        ModelObject Ness;
        ModelObject Peach;
        ModelObject Fox;

        // Font global variables.
        SpriteFont Font1;
        Vector2 FontPos;

        // Menu global variables.
        Menu mymenu = new Menu();
        worldobj basemenu, obj2, obj3, obj4, obj5, obj6;
        Texture2D icon1, icon2;
        Rectangle[] Source, Sourcesub;

        // "Game1()": 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true; // Enables anti-aliasing, if graphics device supports it. Set to false if performance is slow!
            Content.RootDirectory = "Content";
        }

        // "Initialize()": Allows the game to perform any initialization it needs to before starting to run.
        // This is where it can query for any required services and load any non-graphic
        // related content. Calling base.Initialize will enumerate through any components
        // and initialize them as well.
        protected override void Initialize()
        {
            // Graphics initializer variables.
            graphics.PreferredBackBufferWidth = 1024; // Window width.
            graphics.PreferredBackBufferHeight = 768; // Window height.
            graphics.IsFullScreen = false; // Used to set the application to fullscreen or windowed mode.
            graphics.ApplyChanges(); // Updates the graphics with the updated changes.
            Window.Title = "Super Smash Bros. Brawl Model Viewer v0.50 (WIP) - NOT FOR PUBLIC RELEASE"; // Title of the window application.

            // Initialize the 3D stage models.
            Skybox = new ModelObject(); // Main stage model.
            Skybox.startPos = new Vector3(0, 0, 0); // Starting coordinates.
            Skybox1 = new ModelObject(); // Additional stage objects.
            Skybox1.startPos = new Vector3(0, 0, 0);
            FinalDest = new ModelObject(); // Final Destination model coordinates.
            FinalDest.startPos = new Vector3(0, 0, 0);

            // Initialize the 3D character models.
            //Mario = new ModelObject();
            //Mario.startPos = new Vector3(0, 0, 0);
            Mario = initModel(0, 0);
            Mario.startPos = new Vector3(0, -1, 0); // Used to correct position on Y.
            DonkeyKong = initModel(40, 0);
            DonkeyKong.startPos = new Vector3(40, 10, 0); // Used to correct position on Y.
            Kirby = initModel(80, 0);
            Kirby.startPos = new Vector3(80, 10, 90); // Used to correct position on Y & Z.
            Mario1 = initModel(20, 0); // Load an additional Mario.
            Mario2 = initModel(40, 0); // Load an additional Mario.
            Mario3 = initModel(-40, 0);
            Ness = initModel(-40, 0);
            Ness.startPos = new Vector3(-40, -2, 0); // Used to correct position on Y.
            Peach = initModel(-80, 0);
            Peach.startPos = new Vector3(-80, -1, 0); // Used to correct position on Y.
            Fox = initModel(-60, 0);
            Fox.startPos = new Vector3(-60, 7, 0); // Used to correct position on Y.

            // Input initializer variables.
            keyState = Keyboard.GetState(); // Retrive the keyboard status.
            this.IsMouseVisible = true;

            // Initialize the matrix object.
            mMatrix.worldMatrix = new Matrix();
            mMatrix.viewMatrix = new Matrix();
            mMatrix.projectionMatrix = new Matrix();

            // Initialize the menu.
            mymenu.init();

            base.Initialize();
        }

        // "LoadContent()": will be called once per game and is the place to load
        // all of your content.
        protected override void LoadContent()
        {
            device = graphics.GraphicsDevice; // Initializes the graphics device.
            effect = Content.Load<Effect>("Effects\\effects"); // Loads the specified effect.
            SetUpCamera(); // Sets up the camera position.

            titleScreen = Content.Load<Texture2D>("Title\\Title"); // Loads the title screen.
            loadScreen = Content.Load<Texture2D>("Title\\Loading"); // Loads the title screen.

            inputState.musicPlay("Menu_1"); // Plays the default song on load.
            mymenu.songString = "SONG: Menu 1 [Super Smash Bros. Brawl]";

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch1 = new SpriteBatch(GraphicsDevice);

            Font1 = Content.Load<SpriteFont>("Fonts\\Verdana"); // Sets the font to 'Verdana'.
            FontPos = new Vector2((graphics.GraphicsDevice.Viewport.Width * 0.50f), graphics.GraphicsDevice.Viewport.Height - 14); // Sets the font position.

            // Loads the 2D textures into the program.
            background = Content.Load<Texture2D>("Textures\\Sky"); // Loads the background image.
            fdbackground = Content.Load<Texture2D>("Textures\\Stars"); // Loads the star background for Final Destination.
            currentbackground = background; // Uses the sky background as the default stage background.

            // Load menu textures.
            icon1 = Content.Load<Texture2D>("GUI\\Menu");
            icon2 = Content.Load<Texture2D>("GUI\\Characters");
            // TO DO: PUT IN REAL TEXTURES FOR THE MENU ICONS. 
            mymenu.Texture = icon1;
            mymenu.setSub1 = icon2;
            mymenu.setSub2 = icon2;
            mymenu.setSub3 = icon2;
            mymenu.setSub4 = icon2;
            mymenu.setSub5 = icon2;

            Source = new Rectangle[]
                     {
                         new Rectangle(0, 0, 105, 95),
                         new Rectangle(100, 0, 105, 95),
                         new Rectangle(205, 0, 105, 95),
                         new Rectangle(305, 0, 105, 95),
                         new Rectangle(405, 0, 105, 95)
                     };
            Sourcesub = new Rectangle[]
                     {
                         new Rectangle(0, 0, 128, 128),
                         new Rectangle(128, 0, 128, 128),
                         new Rectangle(256, 0, 128, 128),
                         new Rectangle(384, 0, 128, 128)
                     };

            //TODO: ADD MORE SOURCE - added 1 sub menu
            mymenu.Source = Source;
            mymenu.Submenu = Sourcesub;

            //TODO: CHANGE TO NONSTATIC
            Color temptint = new Color();
            temptint.R = 255;
            temptint.G = 255;
            temptint.B = 255;
            temptint.A = 255;
            //add top menu
            for (int i = 0; i < 5; i++)
            {
                basemenu = new worldobj();
                basemenu.spritePosition.X = Source[i].X;
                basemenu.spritePosition.Y = Source[i].Y;
                basemenu.width = 105;
                basemenu.height = 95;
                basemenu.show = true;
                basemenu.tint = temptint;
                mymenu.Add(basemenu);
            }

            //TO-DO adding submenus... not working

            for (int i = 0; i < 4; i++)
            {
                int tempadd = 128;
                //TODO: INITIALIZE OTHER VARIABLES FOR SUB MENUS. 
                obj2 = new worldobj();
                obj3 = new worldobj();
                obj4 = new worldobj();
                obj5 = new worldobj();
                obj6 = new worldobj();

                //INITIALIZE OBJECT 2 AND PASS IT IN.... but how to change pos?
                obj2.spritePosition.X = Sourcesub[i].X + tempadd * 0;
                obj2.spritePosition.Y = Sourcesub[i].Y + 128;
                obj2.width = 128;
                obj2.height = 128;
                obj2.show = false;
                obj2.tint = temptint;

                //2nd obj
                obj3.spritePosition.X = Sourcesub[i].X + tempadd * 1;
                obj3.spritePosition.Y = Sourcesub[i].Y + 128;
                obj3.width = 128;
                obj3.height = 128;
                obj3.show = false;
                obj3.tint = temptint;
                //3rd obj
                obj4.spritePosition.X = Sourcesub[i].X + tempadd * 2;
                obj4.spritePosition.Y = Sourcesub[i].Y + 128;
                obj4.width = 128;
                obj4.height = 128;
                obj4.show = false;
                obj4.tint = temptint;
                //4th obj
                obj5.spritePosition.X = Sourcesub[i].X + tempadd * 3;
                obj5.spritePosition.Y = Sourcesub[i].Y + 128;
                obj5.width = 128;
                obj5.height = 128;
                obj5.show = false;
                obj5.tint = temptint;
                //5th obj
                obj6.spritePosition.X = Sourcesub[i].X + tempadd * 4;
                obj6.spritePosition.Y = Sourcesub[i].Y + 128;
                obj6.width = 128;
                obj6.height = 128;
                obj6.show = false;
                obj6.tint = temptint;

                //add the items
                mymenu.AddSub1(obj2);
                mymenu.AddSub2(obj3);
                mymenu.AddSub3(obj4);
                mymenu.AddSub4(obj5);
                mymenu.AddSub5(obj6);

            }

            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); // Set the rectangle parameters.
            mymenu.Frame = mainFrame;
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height; // Set the aspect ratio.
        }

        // "LoadModel()": Used to load a specific model with textures.
        private Model LoadModel(string assetName, out Texture2D[] textures)
        {
            Model newModel = Content.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel; // Returns the model.
        }

        // "UnloadContent()": UnloadContent will be called once per game and is the place to unload
        // all content.
        protected override void UnloadContent()
        {
        }

        // "Update()": Allows the game to run logic such as updating the world,
        // checking for collisions, gathering input, and playing audio.
        // gameTime Provides a snapshot of timing values.</param>
        Random testrandom = new Random();
        int output;
        protected override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState(); // Gets the current keyboard state.
            inputState.UpdateInput(cameraPosition); // Checks the keyboard input and executes the proper action.

            // If the Escape Key is pressed...
            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit(); // Exit program.
            }

            // Disables the music.
            if (keyState.IsKeyDown(Keys.LeftControl))
            {
                mymenu.songString = inputState.displayString;
                mymenu.songEnabled = true;
            }

            // Resets the camera position to default.
            if (keyState.IsKeyDown(Keys.LeftAlt))
            {
                cameraPosition = new Vector3(0, 0, 250);
            }

            // If the Enter Key is pressed...
            if (keyState.IsKeyDown(Keys.Enter))
            {
                // Enables the flag to load the stage and character models. 
                if (loadStart == true)
                {
                    inputState.soundEffects("Enter"); // Outputs the "Enter" select sound effect.
                    loading = true;
                }
            }

            // If the inputted key involves changes with the camera, update the camera.
            if (inputState.isCameraUpdate == true)
            {
                UpdateCamera(); // Update the camera position.
            }

            MouseState aMouse = Mouse.GetState(); // Gets the current mouse state.
            mymenu.checkTint(aMouse); // Do the tinting on the mouse.
            mPreviousMouseState = aMouse;  // Store the previous mouse state

            // Check model locations and update accordingly.
            output = testrandom.Next();
            Mario = objAI.autoMotion(Mario, output);
            output = testrandom.Next();
            DonkeyKong = objAI1.autoMotion(DonkeyKong, output);
            output = testrandom.Next();
            Kirby = objAI2.autoMotion(Kirby, output);
            output = testrandom.Next();
            Ness = objAI3.autoMotion(Ness, output);
            output = testrandom.Next();
            Fox = objAI4.autoMotion(Fox, output);
            output = testrandom.Next();
            Peach = objAI5.autoMotion(Peach, output);
            output = testrandom.Next();
            Mario1 = objAI.autoMotion(Mario1, output);
            output = testrandom.Next();
            Mario2 = objAI1.autoMotion(Mario2, output);
            output = testrandom.Next();
            Mario3 = objAI2.autoMotion(Mario3, output);
            output = testrandom.Next();

            base.Update(gameTime);
        }

        // "Draw()": This is called when the game should draw itself. gameTime Provides a snapshot of timing values.
        protected override void Draw(GameTime gameTime)
         {
           
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backColor, 1.0f, 0); // Clears the buffer and colors the background with the specified color.

            // Displays the title screen on launch and continues to do so until loadStart flag is false.
            if (loadStart == true)
            {
                // Displays the title screen.
                if (loading == false)
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // Start building the sprite.
                    spriteBatch.Draw(titleScreen, GraphicsDevice.Viewport.Bounds, Color.White); // Draws the title screen.
                    spriteBatch.End();
                }
                // Displays the loading screen.
                else if (loading == true)
                {
                    inputState.soundEffects("Loading"); // Outputs the loading select sound.
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // Start building the sprite.
                    spriteBatch.Draw(loadScreen, GraphicsDevice.Viewport.Bounds, Color.White); // Draws the title screen.
                    spriteBatch.End();
                    loadStart = false;
                }
            }

            // Loads the stage and models. Runs only once in the program. This can take a while to load, depending on the complexity of the models!
            else if ((loadStart == false) && (loadComplete == false))
            {
                inputState.soundEffects("Loading"); // Outputs the loading select sound.

                // Stadium stage textures.
                Skybox.model = LoadModel("Stages\\SSBB_Stadium\\Stadium", out Skybox.textures); // Loads the stage model. Depending on the complexity of the stage model, loading could take a while!
                
                // Stadium stage additional textures.
                Skybox1.model = LoadModel("Stages\\SSBB_Stadium\\StadiumSky", out Skybox1.textures); // Loads the additional stage model components.

                // Final Destination stage textures.
                FinalDest.model = LoadModel("Stages\\SSBB_FinalDestination\\Final_Destination", out FinalDest.textures); // Loads the additional stage model components.

                // Loads the character model into the program.
                Mario.model = LoadModel("Models\\Mario", out Mario.textures); // Loads the Mario model.
                DonkeyKong.model = LoadModel("Models\\DonkeyKong", out DonkeyKong.textures); // Loads the Donkey Kong model.
                Kirby.model = LoadModel("Models\\Kirby", out Kirby.textures); // Loads the Kirby model.
                Ness.model = LoadModel("Models\\Ness\\Ness", out Ness.textures); // Loads the Ness model.
                Peach.model = LoadModel("Models\\Peach", out Peach.textures); // Loads the Ness model.
                Fox.model = LoadModel("Models\\Fox", out Fox.textures); // Loads the Ness model.
                Mario1.model = LoadModel("Models\\MarioBlue", out Mario1.textures); // Loads the Mario1 model.
                Mario2.model = LoadModel("Models\\MarioWhite", out Mario2.textures); // Loads the Mario2 model.
                Mario3.model = LoadModel("Models\\MarioYellow", out Mario3.textures); // Loads the Mario3 model.
                
                inputState.soundEffects("Complete!"); // Outputs the "Complete!" sound, indicating that all models have loaded successfully.
                loadComplete = true;
            }

            else if (loadComplete == true)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // Start building the sprite.
                spriteBatch.Draw(currentbackground, GraphicsDevice.Viewport.Bounds, Color.White); // Draws the background.
                spriteBatch.End(); // End building the sprite.

                if (mymenu.stageSwitch == false)
                {
                    // Drawing the Stadium stage.
                    device.BlendState = BlendState.Opaque; // When enabled, stage appears opague.
                    device.DepthStencilState = DepthStencilState.Default; // When enabled, stage appears opague.
                    device.SamplerStates[0] = SamplerState.LinearWrap;
                    mMatrix.worldMatrix = Matrix.CreateScale(1.5f) * Matrix.CreateTranslation(Skybox.startPos); // Specify transformations for skybox.
                    drawModels.DrawModel(mMatrix, Skybox); // Draw the skybox.
                    device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                    // Drawing additional Stadium components.
                    device.BlendState = BlendState.Opaque; // When enabled, stage appears opague.
                    device.DepthStencilState = DepthStencilState.Default; // When enabled, stage appears opague.
                    device.SamplerStates[0] = SamplerState.LinearWrap;
                    mMatrix.worldMatrix = Matrix.CreateScale(1.5f) * Matrix.CreateTranslation(Skybox1.startPos); // Specify transformations for skybox.
                    drawModels.DrawModel(mMatrix, Skybox1); // Draw the skybox.
                    device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                    currentbackground = background; // Uses the sky as the background.
                }
                else if (mymenu.stageSwitch == true)
                {
                    // Drawing the Final Destination stage.
                    device.BlendState = BlendState.Opaque; // When enabled, stage appears opague.
                    device.DepthStencilState = DepthStencilState.DepthRead; // When enabled, stage appears transparent.
                    device.SamplerStates[0] = SamplerState.LinearWrap;
                    mMatrix.worldMatrix = Matrix.CreateScale(1.5f) * Matrix.CreateTranslation(FinalDest.startPos); // Specify transformations for skybox.
                    drawModels.DrawModel(mMatrix, FinalDest); // Draw the skybox.
                    device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                    currentbackground = fdbackground; // Uses the stars as the background.
                }

                // Drawing Mario model.
                device.BlendState = BlendState.Opaque; // When enabled, Mario appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Mario appears opague.

                // Stands still if false.
                if (mymenu.bounceSwitch == false)
                {
                    mMatrix.worldMatrix = Matrix.CreateTranslation(Mario.startPos); // Specify transformations.
                }
                // Bounces if true.
                else if (mymenu.bounceSwitch == true)
                {
                    mMatrix.worldMatrix = Matrix.CreateTranslation(Mario.startPos + Mario.direction); // Specify transformations.
                    //temp = Mario.startPos + Mario.direction;
                }
                
                drawModels.DrawModel(mMatrix, Mario); // Draw Mario.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.           
            
                // Drawing the Donkey Kong model.
                device.BlendState = BlendState.Opaque; // When enabled, DK appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, DK appears opague.
                
                // Stands still if false.
                if (mymenu.bounceSwitch == false)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(DonkeyKong.startPos); // Specify transformations for DK.
                }
                // Bounces if true.
                else if (mymenu.bounceSwitch == true)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.3f) * Matrix.CreateTranslation(DonkeyKong.startPos + DonkeyKong.direction); // Specify transformations for DK.
                    //temp = DonkeyKong.startPos + DonkeyKong.direction;
                }
                drawModels.DrawModel(mMatrix, DonkeyKong); // Draw DK.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                // Drawing the Kirby model.
                device.BlendState = BlendState.Opaque; // When enabled, Kirby appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Kirby appears opague.

                // Stands still if false.
                if (mymenu.bounceSwitch == false)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.4f) * Matrix.CreateTranslation(Kirby.startPos); // Specify transformations for Kirby.
                }
                // Bounces if true.
                else if (mymenu.bounceSwitch == true)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.4f) * Matrix.CreateTranslation(Kirby.startPos + Kirby.direction); // Specify transformations for Kirby.
                    //temp = Kirby.startPos + Kirby.direction;
                }
                drawModels.DrawModel(mMatrix, Kirby); // Draw Kirby.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                // Drawing the Ness model.
                device.BlendState = BlendState.Opaque; // When enabled, Ness appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Ness appears opague.

                // Stands still if false.
                if (mymenu.bounceSwitch == false)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(2) * Matrix.CreateRotationX(8) * Matrix.CreateRotationY(3) * Matrix.CreateTranslation(Ness.startPos); // Specify transformations for Ness.
                }
                // Bounces if true.
                else if (mymenu.bounceSwitch == true)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(2) * Matrix.CreateRotationX(8) * Matrix.CreateRotationY(3) * Matrix.CreateTranslation(Ness.startPos + Ness.direction); // Specify transformations for Ness.
                    // temp = Ness.startPos + Ness.direction;
                }
                drawModels.DrawModel(mMatrix, Ness); // Draw Ness.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                // Drawing the Peach model.
                device.BlendState = BlendState.Opaque; // When enabled, Peach appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Peach appears opague.

                // Stands still if false.
                if (mymenu.bounceSwitch == false)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(Peach.startPos); // Specify transformations for Peach.
                }
                // Bounces if true.
                else if (mymenu.bounceSwitch == true)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.04f) * Matrix.CreateTranslation(Peach.startPos + Peach.direction); // Specify transformations for Peach.
                    //temp = Peach.startPos + Peach.direction;
                }
                drawModels.DrawModel(mMatrix, Peach); // Draw Peach.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                // Drawing the Fox model.
                device.BlendState = BlendState.Opaque; // When enabled, Fox appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Fox appears opague.

                // Stands still if false.
                if (mymenu.bounceSwitch == false)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.4f) * Matrix.CreateTranslation(Fox.startPos); // Specify transformations for Fox.
                }
                // Bounces if true.
                else if (mymenu.bounceSwitch == true)
                {
                    mMatrix.worldMatrix = Matrix.CreateScale(0.4f) * Matrix.CreateTranslation(Fox.startPos + Fox.direction); // Specify transformations for Kirby.
                    //temp = Fox.startPos + Fox.direction;
                }
                drawModels.DrawModel(mMatrix, Fox); // Draw Fox.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.

                /*
                // Drawing alternate Mario 1 model.
                device.BlendState = BlendState.Opaque; // When enabled, Mario appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Mario appears opague.
                mMatrix.worldMatrix = Matrix.CreateTranslation(Mario1.startPos + Mario1.direction); // Specify transformations for Mario.
                drawModels.DrawModel(mMatrix, Mario1); // Draw Mario.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.
                //temp = Mario1.startPos + Mario1.direction;
                 
                // Drawing alternate Mario 2 model.
                device.BlendState = BlendState.Opaque; // When enabled, Mario appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Mario appears opague.
                mMatrix.worldMatrix = Matrix.CreateTranslation(Mario2.startPos + Mario2.direction); // Specify transformations for Mario.
                drawModels.DrawModel(mMatrix, Mario2); // Draw Mario.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.
                //temp = Mario2.startPos + Mario2.direction;

                // Drawing alternate Mario 3 model.
                device.BlendState = BlendState.Opaque; // When enabled, Mario appears opague.
                device.DepthStencilState = DepthStencilState.Default; // When enabled, Mario appears opague.
                mMatrix.worldMatrix = Matrix.CreateTranslation(Mario3.startPos + Mario3.direction); // Specify transformations for Mario.
                drawModels.DrawModel(mMatrix, Mario3); // Draw Mario.
                device.DepthStencilState = DepthStencilState.Default; // Graphics device attribute setup for models.
                //temp = Mario3.startPos + Mario3.direction;
                */

                // Draws the menu.
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // Start building the sprite.
                mymenu.draw(gameTime, spriteBatch);
                spriteBatch.End(); // End building the sprite.

                // If mymenu.songEnabled is true, display the song name string in-game.
                if (mymenu.songEnabled == true)
                {
                    Vector2 FontOrigin = Font1.MeasureString(mymenu.songString) / 2; // Finds the center of the string.
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // Start building the sprite.
                    spriteBatch.DrawString(Font1, mymenu.songString, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 1.0f); // Draws the string.
                    spriteBatch.End(); // End building the sprite.
                }
            }

            base.Draw(gameTime);
        }

        // "SetUpCamera()": Sets the camera position.
        public void SetUpCamera()
        {
            mMatrix.viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            mMatrix.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 10000.0f); // Sets the field of view. Can also specify aspet ratio and view distance here.
        }

        // "UpdateCamera()": Sets the camera position.
        public void UpdateCamera()
        {
            cameraPosition = inputState.cameraPosition;
            mMatrix.viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
        }

        // "initModel": Sets up the initial model for other model objects.
        private ModelObject initModel(int x, int z)
        {
            ModelObject thismodel = new ModelObject();
            thismodel.startPos = new Vector3(x, 0, z);
            thismodel.direction = Vector3.Zero;
            thismodel.gravity = new ball();
            //VINIT_VELOCITY
            thismodel.gravity.distance = new Vector3();
            thismodel.gravity.Vinit = 0.8f;
            thismodel.gravity.distance.Y = 0;
            thismodel.gravity.velocity = 0;
            thismodel.gravity.acc = -0.01f;
            thismodel.gravity.time = 0;
            thismodel.gravity.isfalling = false;
            thismodel.gravity.reverse = false;
            thismodel.gravity.hasBounced = false;
            thismodel.gravity.finishBouncing = false;

            return thismodel;
        }
    }
}

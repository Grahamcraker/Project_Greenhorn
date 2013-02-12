#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using KnightBear_TD_WindowsDesktop.Entities;
using KnightBear_TD_WindowsDesktop.Entities.Nightmares;
using KnightBear_TD_WindowsDesktop.Entities.Towers;
using KnightBear_TD_WindowsDesktop.Levels;
#endregion

namespace KnightBear_TD_WindowsDesktop
{
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class KnightBearGame : Game
    {
        GraphicsDevice device;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool isNewLevel;
        int screenWidth, screenHeight, levelIndex;

        Level currentLevel;

        /// <summary>
        /// Game begins here
        /// </summary>
        public KnightBearGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"../../../../../Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            device = graphics.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            Window.Title = "KnightBear Tower Defense";

            isNewLevel = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentLevel = new Level(Services, XmlLoader.GenerateLevelConfig(1));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            double currentMillis = gameTime.TotalGameTime.TotalMilliseconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ProcessInput();

            if (isNewLevel)
            {
                currentLevel = new Level(Services, XmlLoader.GenerateLevelConfig(levelIndex));
                isNewLevel = false;
#if DEBUG
                Console.WriteLine("Changing to level: " + levelIndex);
#endif
            }

            currentLevel.Update(gameTime, Keyboard.GetState(), Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            currentLevel.Draw(spriteBatch, screenWidth, screenHeight);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles responses to user input
        /// i.e. Keyboard, Mouse, Gamepad, Touch Events
        /// </summary>
        private void ProcessInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.D1))
            {
                levelIndex = 1;
                isNewLevel = true;
            }
            if (keyState.IsKeyDown(Keys.D2))
            {
                levelIndex = 2;
                isNewLevel = true;
            }
        }
    }
}

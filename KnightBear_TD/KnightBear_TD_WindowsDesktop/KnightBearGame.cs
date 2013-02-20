﻿//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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

        bool isNewLevel, isEditor;
        int screenWidth, screenHeight, levelIndex;

        Level level;
        MapEditor editor;

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
            levelIndex = 1;

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

            level = new Level(Services, XmlLoader.GenerateLevelConfig(1), screenWidth, screenHeight);
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
#if DEBUG
                Console.WriteLine("Changing to level: " + levelIndex);
#endif
                level = new Level(Services, XmlLoader.GenerateLevelConfig(levelIndex), screenWidth, screenHeight);
                isNewLevel = false;
            }

            if (isEditor)
            {
#if DEBUG
                Console.WriteLine("Changing to Map Editor");
#endif
                editor = new MapEditor(Services, screenWidth, screenHeight);
                isEditor = false;
            }
            else
            {
                level.Update(gameTime, Keyboard.GetState(), Mouse.GetState(), screenWidth, screenHeight);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            level.Draw(spriteBatch, screenWidth, screenHeight);
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

            if (keyState.IsKeyDown(Keys.L))
            {
                isEditor = false;
            }

            if (keyState.IsKeyDown(Keys.E))
            {
                isEditor = true;
            }
        }
    }
}

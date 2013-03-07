//-----------------------------------------------------------------------
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
using KnightBear_TD_WindowsDesktop.Menus;

namespace KnightBear_TD_WindowsDesktop
{
    /// <summary>
    /// This is the main type for the game
    /// </summary>
    public class KnightBearGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool isEditor;
        float gameScale;
        int screenWidth, screenHeight, levelIndex;

        GameState gameState;
        Level currentLevel;
        MapEditor editor;
        MenuManager menuManager;
        Texture2D background;
        XmlLoader loader;

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

            screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            Window.Title = "KnightBear Tower Defense";

            menuManager = MenuManager.Manager;
            loader = XmlLoader.Loader;

            levelIndex = 1;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MenuMain menu = new MenuMain();
            menuManager.AddMenu("main", menu);
            gameState = GameState.Menu;
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

            switch (gameState)
            {
                case GameState.Menu:
                    menuManager.Update(Keyboard.GetState(), Mouse.GetState(), gameScale);
                    break;
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

            switch (gameState)
            {
                case GameState.Menu:
                    menuManager.Draw(spriteBatch, gameScale);
                    break;
            }

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

    enum GameState
    {
        Menu,
        Load,
        Level,
        MapEditor
    }
}

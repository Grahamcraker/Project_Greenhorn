//-----------------------------------------------------------------------
// <copyright file="KnightBearGame.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using KnightBear_TD_Windows.Screens;
using KnightBear_TD_Windows.Screens.Menus;
#endregion

namespace KnightBear_TD_Windows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class KnightBearGame : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        /// <summary>
        /// Main entry point for the game. Creates the screen manager and sets the screen size.
        /// </summary>
        
        public KnightBearGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            // What is the root folder of our content?
            Content.RootDirectory = @"Content";
            
            // Create our ScreenManager and add it to the game's Components
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;

            AddInitialScreens();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds the background and main menu.
        /// </summary>
        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen());

            screenManager.AddScreen(new MainMenuScreen());
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="ScreenManager.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_Windows.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows
{
    /// <summary>
    /// Main class for handling screens. This class keeps
    /// a list of screens representing what is drawn on screen.
    /// Only one screen will be drawn unless a popup has appeared
    /// over top of another screen. This class also makes calls
    /// to have the screens update and draw. The screen manager
    /// will only allow the top screen to handle input from the player.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        /// <summary>
        /// True = The manager has loaded all its content.
        /// False = The manager hasn't loaded its content.
        /// </summary>
        bool isInitialized;
        /// <summary>
        /// Content manager for loading content.
        /// </summary>
        ContentManager content;
        /// <summary>
        /// The current state of the user inputs.
        /// </summary>
        InputState inputState;
        /// <summary>
        /// A list of all the current screens.
        /// </summary>
        List<BaseScreen> screens;
        /// <summary>
        /// A spritebatch used to make drawing calls.
        /// </summary>
        SpriteBatch spriteBatch;
        /// <summary>
        /// Spritefont containing font details for menu text.
        /// </summary>
        SpriteFont menuFont;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current spritebatch
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        /// <summary>
        /// Returns the font for menu text
        /// </summary>
        public SpriteFont MenuFont
        {
            get { return menuFont; }
        }
        #endregion

        #region Load
        /// <summary>
        /// Main constructor for the manager
        /// </summary>
        /// <param name="game"></param>
        public ScreenManager(Game game)
            : base(game)
        {
            content = game.Content;
            inputState = new InputState();
            screens = new List<BaseScreen>();

        }

        /// <summary>
        /// Initializes all non-content variables
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        /// <summary>
        /// Loads all initial content and activates starting screens
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menuFont = content.Load<SpriteFont>("Fonts/menuFont");

            foreach (BaseScreen screen in screens)
            {
                screen.Activate();
            }

            base.LoadContent();
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the game. This method is called once per frame.(i.e. 30 FPS = 30 times/second)
        /// The game will cycle through the screens, starting with the top screen. This screen will
        /// handle any user input and inform subsequent screens that they are covered and shouldn't
        /// draw. All screens will still be told to update themselves.
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Update(GameTime gameTime)
        {
            inputState.Update();

            List<BaseScreen> tempScreens = new List<BaseScreen>();

            // Make a copy of the screens in case 1 screen removes itself/another
            foreach (BaseScreen s in screens)
            {
                tempScreens.Add(s);
            }

            bool hasFocus = Game.IsActive;
            bool isCoveredByOtherScreen = false;

            while (tempScreens.Count > 0)
            {
                BaseScreen screen = tempScreens[tempScreens.Count - 1];

                tempScreens.Remove(screen);

                screen.Update(gameTime, hasFocus, isCoveredByOtherScreen);
                
                if (screen.State == ScreenState.Active)
                {
                    // This should be the first screen. Which should be what we want to see drawn on top
                    if (hasFocus)
                    {
                        screen.HandleInput(gameTime, inputState);

                        hasFocus = false;
                    }

                    // If this isn't a popup, other screens are covered by this screen
                    if (!screen.IsPopup)
                    {
                        isCoveredByOtherScreen = true;
                    }
                }
            }
        }

        /// <summary>
        /// Tells each non-hidden screen to draw itself
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (BaseScreen screen in screens)
            {
                if (screen.State == ScreenState.Hidden)
                {
                    continue;
                }

                screen.Draw();
            }

            spriteBatch.End();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Activeates the given screen and adds it to the list.
        /// This new screen will become the active screen.
        /// </summary>
        /// <param name="screen">The screen to be added</param>
        public void AddScreen(BaseScreen screen)
        {
            screen.Manager = this;

            if (isInitialized)
            {
                screen.Activate();
            }

            screens.Add(screen);
        }

        /// <summary>
        /// Returns a copy of the screens. We return only a
        /// copy as a screen should only be added/removed
        /// through AddScreen()/RemoveScreen().
        /// </summary>
        /// <returns>An array of screens</returns>
        public BaseScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        /// <summary>
        /// Removes a specific screen from the list. This
        /// method will immediately remove the screen and 
        /// not allow transitions to finish.
        /// </summary>
        /// <param name="screen">The screen to remove</param>
        public void RemoveScreen(BaseScreen screen)
        {
            screen.Unload();

            screens.Remove(screen);
        }
        #endregion
    }
}

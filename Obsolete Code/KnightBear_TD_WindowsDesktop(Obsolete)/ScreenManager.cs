using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
using System.IO.IsolatedStorage;
using KnightBear_TD_WindowsDesktop.Screens;

namespace KnightBear_TD_WindowsDesktop
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        SpriteBatch spriteBatch;
        SpriteFont defaultFont;
        Texture2D blankTexture;

        //InputState inputState;

        List<ScreenBase> screens;
        List<ScreenBase> tempScreens;
        #endregion

        #region Properties
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont DefaultFont
        {
            get { return defaultFont; }
        }

        public Texture2D BlankTexture
        {
            get { return blankTexture; }
        }
        #endregion

        #region Load
        public ScreenManager(Game game)
            : base(game)
        {

        }

        /// <summary>
        /// Initializes the screen manager.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads content. Called only once.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Unload Content. Called only once.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        #endregion

        #region Update
        /// <summary>
        /// Runs the update logic for all the game's screens.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the active screen(s)
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion
    }
}

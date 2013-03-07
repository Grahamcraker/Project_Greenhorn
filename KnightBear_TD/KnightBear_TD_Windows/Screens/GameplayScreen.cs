using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_Windows.Gameplay.Levels;
using KnightBear_TD_Windows.Screens.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows.Screens
{
    class GameplayScreen : BaseScreen
    {
        #region Fields
        /// <summary>
        /// Since the game loads a large amount of content,
        /// we want to create a manager just for the game.
        /// We can then unload all the content for the game
        /// much easier.
        /// </summary>
        ContentManager content;
        InputAction pauseAction;
        InputAction spawnNightmareAction;
        int screenWidth;
        int screenHeight;
        Level level;
        LevelConfig config;
        #endregion

        #region Properties
        #endregion

        #region Load
        public GameplayScreen(LevelConfig config)
        {
            this.config = config;
        }

        public override void Activate()
        {
            content = new ContentManager(Manager.Game.Services, Manager.Game.Content.RootDirectory);

            screenWidth = Manager.Game.GraphicsDevice.Viewport.Width;
            screenHeight = Manager.Game.GraphicsDevice.Viewport.Height;

            level = new Level(content, config, screenWidth, screenHeight);

            pauseAction = new InputAction(new Keys[] { Keys.P, Keys.Escape }, true);
            spawnNightmareAction = new InputAction(new Keys[] { Keys.N }, true);
        }
        #endregion

        #region Update
        public override void Draw()
        {
            level.Draw(Manager.SpriteBatch, screenWidth, screenHeight);
        }

        public override void Update(GameTime gameTime, bool hasFocus, bool isCovered)
        {
            base.Update(gameTime, hasFocus, isCovered);
            
            screenWidth = Manager.Game.GraphicsDevice.Viewport.Width;
            screenHeight = Manager.Game.GraphicsDevice.Viewport.Height;

            if (IsActive)
            {
                level.Update(gameTime, screenWidth, screenHeight);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState inputState)
        {
            if (pauseAction.Evaluate(inputState))
            {
                PauseMenuScreen screen = new PauseMenuScreen();
                screen.IsPopup = true;
                Manager.AddScreen(screen);
            }

            if (spawnNightmareAction.Evaluate(inputState))
            {
                level.CreateNightmare();
            }

            if (inputState.IsNewLeftMouseClick())
            {
                level.CreateTower(inputState.GetMousePosition());
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}

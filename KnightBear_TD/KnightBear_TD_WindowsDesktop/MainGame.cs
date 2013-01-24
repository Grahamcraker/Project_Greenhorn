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
#endregion

namespace KnightBear_TD_WindowsDesktop
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDevice device;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int UPDATE_SPAN = 500;
        double lastUpdateTime = 0;
        int screenWidth, screenHeight;

        Texture2D backgroundTexture;
        Texture2D laser;

        Level currentLevel;
        List<Nightmare> nightmares;
        List<Tower> towers;

        public MainGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            TimeSpan x = TargetElapsedTime;
            TargetElapsedTime = new TimeSpan(30);
            Window.Title = "KnightBear Tower Defense";

            currentLevel = new Level(1);
            nightmares = new List<Nightmare>();
            towers = new List<Tower>();

            device = graphics.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

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

            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("images/lvl2Background");
            laser = Content.Load<Texture2D>("images/laser");
            nightmares.Add(new Nightmare(Content.Load<Texture2D>("images/lime"), new Vector2(0, 350)));
            towers.Add(new Tower(Content.Load<Texture2D>("images/basicTowerIcon"), Content.Load<Texture2D>("images/laser"), new Vector2(150, 250)));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here
            //ProcessKeyboard();
            if (currentMillis - lastUpdateTime >= UPDATE_SPAN)
            {
                lastUpdateTime = currentMillis;
                MoveNightmares();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            DrawEntities();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawEntities()
        {
            foreach (Nightmare nm in nightmares)
            {
                spriteBatch.Draw(nm.Icon, nm.Position, null, Color.White, 0, new Vector2(nm.Icon.Width / 2, nm.Icon.Height / 2), nm.Scale, SpriteEffects.None, 0);
            }

            foreach (Tower tower in towers)
            {
                spriteBatch.Draw(tower.TowerIcon, tower.Position, null, Color.White, 0, new Vector2(0, 0), tower.Scale, SpriteEffects.None, 0);
            }
        }

        private void MoveNightmares()
        {
            foreach (Nightmare nm in nightmares)
            {
                if (nm.LocationIndex < currentLevel.NightmarePath.Count)
                    nm.UpdatePosition(currentLevel.NightmarePath[nm.LocationIndex]);
            }
        }

        private void ProcessKeyboard()
        {/*
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Left))
            {
                if (nightmare.Position.X <= 0)
                    nightmare.ResetPosition(0, nightmare.Position.Y);
                else
                    nightmare.UpdatePosition(-1, 0);
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                if (nightmare.Position.X + nightmare.Width >= 500)
                    nightmare.ResetPosition(500 - nightmare.Width, nightmare.Position.Y);
                else
                    nightmare.UpdatePosition(1, 0);
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                if (nightmare.Position.Y <= 0)
                    nightmare.ResetPosition(nightmare.Position.X, 0);
                else
                    nightmare.UpdatePosition(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                if (nightmare.Position.Y + nightmare.Height >= 500)
                    nightmare.ResetPosition( nightmare.Position.X, 500 - nightmare.Height);
                else
                    nightmare.UpdatePosition(0, 1);
            }
          */
        }
    }
}

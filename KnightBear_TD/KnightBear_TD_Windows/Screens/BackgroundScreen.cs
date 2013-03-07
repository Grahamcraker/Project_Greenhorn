using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Screens
{
    class BackgroundScreen : BaseScreen
    {
        private ContentManager content;
        private Texture2D background;

        public BackgroundScreen()
        {

        }

        public override void Update(GameTime gameTime, bool hasFocus, bool isCovered)
        {
            base.Update(gameTime, hasFocus, false);
        }

        public override void Activate()
        {
            if (content == null)
            {
                content = new ContentManager(Manager.Game.Services, Manager.Game.Content.RootDirectory);
            }

            background = content.Load<Texture2D>("Images/Converted/backgroundImage");
        }

        public override void Draw()
        {
            int width = Manager.Game.GraphicsDevice.Viewport.Width;
            int height = Manager.Game.GraphicsDevice.Viewport.Height;
            Manager.SpriteBatch.Draw(background, new Rectangle(0, 0, width, height), Color.White);
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="PauseMenuScreen.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class PauseMenuScreen : BaseMenuScreen
    {
        #region Fields
        Texture2D background;
        #endregion

        #region Load
        public PauseMenuScreen()
            : base("Pause Menu")
        {
            // Add Menu Items
            MenuItem resumeMenuItem = new MenuItem("Resume Game");
            MenuItem exitMenuItem = new MenuItem("Exit To Main Menu");

            // Hook up events
            resumeMenuItem.Selected += resumeMenuItem_Selected;
            exitMenuItem.Selected += exitMenuItem_Selected;

            // Add menu Items
            MenuItems.Add(resumeMenuItem);
            MenuItems.Add(exitMenuItem);
        }

        public override void Activate()
        {
            // TODO: Create background which will make the game seem faded out
            /*GraphicsDevice device = Manager.GraphicsDevice;
            int width = Manager.GraphicsDevice.Viewport.Width;
            int height = Manager.GraphicsDevice.Viewport.Height;
            background = new Texture2D(device, width, height);
            background.SetData(new Color[] { Color.Black });*/
        }
        #endregion

        #region Events
        void resumeMenuItem_Selected(object sender, EventArgs e)
        {
            
            ExitScreen();
        }

        void exitMenuItem_Selected(object sender, EventArgs e)
        {
            BaseScreen[] screens = Manager.GetScreens();

            foreach (BaseScreen s in screens)
            {
                Manager.RemoveScreen(s);
            }

            Manager.AddScreen(new BackgroundScreen());
            Manager.AddScreen(new MainMenuScreen());
        }
        #endregion
    }
}

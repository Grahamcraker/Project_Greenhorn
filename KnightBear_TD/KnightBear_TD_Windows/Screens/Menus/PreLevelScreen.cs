//-----------------------------------------------------------------------
// <copyright file="PreLevelScreen.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class PreLevelScreen : BaseMenuScreen
    {
        public PreLevelScreen()
            : base("Select A Level")
        {
            MenuItem level1MenuItem = new MenuItem("Level 1");
            MenuItem cancelMenuItem = new MenuItem("Return To Main Menu");

            // Hook up menu items to selection event
            // TODO: Create menu items for towers and levels
            level1MenuItem.Selected += level1MenuItem_Selected;
            cancelMenuItem.Selected += cancelMenuItem_Selected;

            MenuItems.Add(level1MenuItem);
            MenuItems.Add(cancelMenuItem);
        }

        void level1MenuItem_Selected(object sender, EventArgs e)
        {
            BaseScreen[] screens = Manager.GetScreens();

            foreach (BaseScreen s in screens)
            {
                Manager.RemoveScreen(s);
            }

            XmlController control = XmlController.Controller;
            
            Manager.AddScreen(new GameplayScreen(control.LoadLevelConfig(1, LevelType.Story)));
        }

        void cancelMenuItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }
    }
}

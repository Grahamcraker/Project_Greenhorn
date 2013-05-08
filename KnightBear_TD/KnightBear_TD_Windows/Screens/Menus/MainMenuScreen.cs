//-----------------------------------------------------------------------
// <copyright file="MainMenuScreen.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class MainMenuScreen : BaseMenuScreen
    {
        public MainMenuScreen()
            : base("Main Menu")
        {
            MenuItem playGameMenuItem = new MenuItem("Play Game");
            MenuItem editorMenuItem = new MenuItem("Map Editor");
            MenuItem optionsMenuItem = new MenuItem("Options");
            MenuItem exitGameMenuItem = new MenuItem("Exit");

            // Hook up menu items to selection event
            playGameMenuItem.Selected += playGameMenuItem_Selected;
            editorMenuItem.Selected += editorMenuItem_Selected;
            optionsMenuItem.Selected += optionsMenuItem_Selected;
            exitGameMenuItem.Selected += exitGameMenuItem_Selected;

            MenuItems.Add(playGameMenuItem);
            MenuItems.Add(editorMenuItem);
            MenuItems.Add(optionsMenuItem);
            MenuItems.Add(exitGameMenuItem);
        }

        /// <summary>
        /// Method for handling when Play Game is selected
        /// </summary>
        void playGameMenuItem_Selected(object sender, EventArgs e)
        {
            Manager.AddScreen(new PreLevelScreen());
        }

        /// <summary>
        /// Method for handling when Map Editor is selected
        /// </summary>
        void editorMenuItem_Selected(object sender, EventArgs e)
        {
            Manager.AddScreen(new MapEditorConfigMenuScreen());
        }

        /// <summary>
        /// Method for handling when Options is selected
        /// </summary>
        void optionsMenuItem_Selected(object sender, EventArgs e)
        {
            Manager.AddScreen(new OptionsMenuScreen());
        }

        /// <summary>
        /// Method for handling when Exit is selected
        /// </summary>
        void exitGameMenuItem_Selected(object sender, EventArgs e)
        {
            Manager.Game.Exit();
        }
    }
}

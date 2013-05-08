//-----------------------------------------------------------------------
// <copyright file="MapEditorConfigMenuScreen.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class MapEditorConfigMenuScreen : BaseMenuScreen
    {
        #region Fields
        InputAction increaseCountAction;
        InputAction decreaseCountAction;
        int verticalNodeCount;
        int horizontalNodeCount;
        readonly int verticalMin = 3;
        readonly int horizontalMin = 3;
        readonly int verticalMax = 8;
        readonly int horizontalMax = 8;
        #endregion

        #region Load
        public MapEditorConfigMenuScreen()
            : base("Map Editor Config")
        {
            verticalNodeCount = 3;
            horizontalNodeCount = 3;
            // Configure Increase/Decrease count actions
            increaseCountAction = new InputAction(new Keys[] { Keys.Right }, true);
            decreaseCountAction = new InputAction(new Keys[] { Keys.Left }, true);

            // Add Menu Items
            MenuItem verticalNodesMenuItem = new MenuItem("Vertical Nodes: 3");
            MenuItem horizontalNodesMenuItem = new MenuItem("Horizontal Nodes: 3");
            MenuItem confirmMenuItem = new MenuItem("Confirm");
            MenuItem cancelMenuItem = new MenuItem("Cancel");

            // Hook up events
            confirmMenuItem.Selected += confirmMenuItem_Selected;
            cancelMenuItem.Selected += cancelMenuItem_Selected;

            // Add menu Items
            MenuItems.Add(verticalNodesMenuItem);
            MenuItems.Add(horizontalNodesMenuItem);
            MenuItems.Add(confirmMenuItem);
            MenuItems.Add(cancelMenuItem);
        }
        #endregion

        #region Update
        public override void HandleInput(GameTime gameTime, InputState inputState)
        {
            base.HandleInput(gameTime, inputState);

            if (increaseCountAction.Evaluate(inputState))
            {
                switch (SelectedMenuItemIndex)
                {
                    case 0:
                        verticalNodeCount = verticalNodeCount + 1 > verticalMax ? verticalMax : verticalNodeCount + 1;
                        MenuItems[0].Text = String.Format("Vertical Nodes: {0}", verticalNodeCount);
                        break;
                    case 1:
                        horizontalNodeCount = horizontalNodeCount + 1 > horizontalMax ? horizontalMax : horizontalNodeCount + 1;
                        MenuItems[1].Text = String.Format("Horizontal Nodes: {0}", horizontalNodeCount);
                        break;
                }
            }

            if (decreaseCountAction.Evaluate(inputState))
            {
                switch (SelectedMenuItemIndex)
                {
                    case 0:
                        verticalNodeCount = verticalNodeCount - 1 < verticalMin ? verticalMin : verticalNodeCount - 1;
                        MenuItems[0].Text = String.Format("Vertical Nodes: {0}", verticalNodeCount);
                        break;
                    case 1:
                        horizontalNodeCount = horizontalNodeCount - 1 < horizontalMin ? horizontalMin : horizontalNodeCount - 1;
                        MenuItems[1].Text = String.Format("Horizontal Nodes: {0}", horizontalNodeCount);
                        break;
                }
            }
        }
        #endregion

        #region Events
        void confirmMenuItem_Selected(object sender, EventArgs e)
        {
            BaseScreen[] screens = Manager.GetScreens();

            foreach (BaseScreen s in screens)
            {
                if (s.GetType() == typeof(MapEditorScreen))
                {
                    Manager.RemoveScreen(s);
                }
            }

            MapEditorScreen screen = new MapEditorScreen(verticalNodeCount, horizontalNodeCount);

            Manager.AddScreen(screen);

            ExitScreen();
        }

        void cancelMenuItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }
        #endregion
    }
}

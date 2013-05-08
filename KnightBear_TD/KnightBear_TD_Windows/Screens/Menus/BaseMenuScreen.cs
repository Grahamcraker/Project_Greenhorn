//-----------------------------------------------------------------------
// <copyright file="BaseMenuScreen.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class BaseMenuScreen : BaseScreen
    {
        #region Fields
        InputAction menuUpAction;
        InputAction menuDownAction;
        InputAction menuSelectAction;
        InputAction menuCancelAction;
        int selectedMenuItemIndex;
        List<MenuItem> menuItems;
        string title;
        #endregion

        #region Properties
        public int SelectedMenuItemIndex
        {
            get { return selectedMenuItemIndex; }
        }
        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
        }
        #endregion

        #region Load
        public BaseMenuScreen(string title)
        {
            selectedMenuItemIndex = 0;

            this.title = title;

            // Create input actions
            menuUpAction = new InputAction(new Keys[] { Keys.Up }, true);
            menuDownAction = new InputAction(new Keys[] { Keys.Down }, true);
            menuSelectAction = new InputAction(new Keys[] { Keys.Enter, Keys.Space }, true);
            menuCancelAction = new InputAction(new Keys[] { Keys.Escape }, true);

            menuItems = new List<MenuItem>();
        }
        #endregion

        #region Update
        public override void Draw()
        {
            UpdateMenuItemLocations();

            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].Draw(this, i == selectedMenuItemIndex);
            }

            float x = Manager.GraphicsDevice.Viewport.Width / 2 - Manager.MenuFont.MeasureString(title).X / 2;
            float y = 10f;

            Vector2 titlePosition = new Vector2(x, y);

            Manager.SpriteBatch.DrawString(Manager.MenuFont, title, titlePosition, Color.Black);
        }

        public override void HandleInput(GameTime gameTime, InputState inputState)
        {
            if (menuUpAction.Evaluate(inputState))
            {
                selectedMenuItemIndex--;

                if (selectedMenuItemIndex < 0)
                {
                    selectedMenuItemIndex++;
                }
            }

            if (menuDownAction.Evaluate(inputState))
            {
                selectedMenuItemIndex++;

                if (selectedMenuItemIndex > menuItems.Count - 1)
                {
                    selectedMenuItemIndex--;
                }
            }

            if (menuSelectAction.Evaluate(inputState))
            {
                OnItemSelected();
            }

            if (menuCancelAction.Evaluate(inputState))
            {
                OnMenuCancelled();
            }
        }
        #endregion

        #region Methods
        protected void OnItemSelected()
        {
            menuItems[selectedMenuItemIndex].OnSelected();
        }

        protected virtual void OnMenuCancelled()
        {
            ExitScreen();
        }

        protected virtual void UpdateMenuItemLocations()
        {
            Vector2 position = new Vector2(0f, 100f);

            for (int i = 0; i < menuItems.Count; i++)
            {
                MenuItem item = menuItems[i];
                float itemWidth = Manager.MenuFont.MeasureString(item.Text).X / 2;
                position.X = Manager.GraphicsDevice.Viewport.Width / 2 - itemWidth;

                item.Position = position;

                position.Y += Manager.MenuFont.LineSpacing;
            }
        }
        #endregion
    }
}

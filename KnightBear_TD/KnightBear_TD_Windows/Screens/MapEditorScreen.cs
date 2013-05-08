//-----------------------------------------------------------------------
// <copyright file="MapEditorScreen.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_Windows.Editor;
using KnightBear_TD_Windows.Screens.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows.Screens
{
    class MapEditorScreen : BaseScreen
    {
        #region Fields
        InputAction menuAction;
        ContentManager content;
        MapEditor editor;
        readonly int vNodeCount;
        readonly int hNodeCount;
        #endregion

        #region Properties
        public MapEditor Editor
        {
            get { return editor; }
        }
        #endregion

        #region Load
        public MapEditorScreen(int vNodeCount, int hNodeCount)
        {
            this.vNodeCount = vNodeCount;
            this.hNodeCount = hNodeCount;
        }

        public override void Activate()
        {
            menuAction = new InputAction(new Keys[] { Keys.Escape }, true);
            content = new ContentManager(Manager.Game.Services, Manager.Game.Content.RootDirectory);

            int w = Manager.GraphicsDevice.Viewport.Width;
            int h = Manager.GraphicsDevice.Viewport.Height;

            editor = new MapEditor(content, w, h, vNodeCount, hNodeCount);
        }

        public override void Draw()
        {
            editor.Draw(Manager.SpriteBatch);
        }

        public override void HandleInput(GameTime gameTime, InputState inputState)
        {
            if (menuAction.Evaluate(inputState))
            {
                Manager.AddScreen(new MapEditorMenuScreen());
            }

            if (inputState.IsNewLeftMouseClick())
            {
                editor.CycleNode(inputState.GetMousePosition());
            }
        }

        public override void Update(GameTime gameTime, bool hasFocus, bool isCovered)
        {
            base.Update(gameTime, hasFocus, isCovered);
            
            int w = Manager.GraphicsDevice.Viewport.Width;
            int h = Manager.GraphicsDevice.Viewport.Height;
            editor.Update(w, h);
        }
        #endregion
    }
}

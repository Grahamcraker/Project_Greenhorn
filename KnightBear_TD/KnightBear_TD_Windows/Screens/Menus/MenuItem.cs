//-----------------------------------------------------------------------
// <copyright file="MenuItem.cs" company="Leim Productions">
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
    class MenuItem
    {
        #region Fields
        private string text;
        private Vector2 position;
        #endregion

        #region Properties
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        #region Events
        public event EventHandler Selected;

        protected internal virtual void OnSelected()
        {
            if (Selected != null)
            {
                Selected(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Load
        public MenuItem(string text)
        {
            this.text = text;
        }
        #endregion

        #region Update
        public void Update(bool isSelected)
        {

        }

        public void Draw(BaseMenuScreen menu, bool isSelected)
        {
            Color textColor = isSelected ? Color.Yellow : Color.White;

            SpriteFont font = menu.Manager.MenuFont;
            SpriteBatch spriteBatch = menu.Manager.SpriteBatch;
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, textColor);
        }
        #endregion
    }
}

//-----------------------------------------------------------------------
// <copyright file="ScreenState.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KnightBear_TD_Windows.Screens
{
    public enum ScreenState
    {
        Active,
        Hidden
    }

    public abstract class BaseScreen
    {
        #region Fields
        ScreenManager manager;
        ScreenState state;
        bool isPopup;
        bool hasFocus;
        #endregion

        #region Properties
        /// <summary>
        /// True = This is the active screen.
        /// False = This screen is either covered or disabled during a popup.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return hasFocus && state == ScreenState.Active;
            }
        }

        public ScreenManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        public ScreenState State
        {
            get { return state; }
        }

        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }
        #endregion

        #region Load/Update
        public BaseScreen() { }

        public virtual void Draw() { }

        public virtual void Update(GameTime gameTime, bool hasFocus, bool isCovered)
        {
            this.hasFocus = hasFocus;
            state = isCovered ? ScreenState.Hidden : ScreenState.Active;
        }
        #endregion

        #region Methods
        public virtual void Activate() { }

        public void ExitScreen()
        {
            manager.RemoveScreen(this);
        }

        public virtual void HandleInput(GameTime gameTime, InputState inputState) { }

        public virtual void Unload() { }
        #endregion
    }
}

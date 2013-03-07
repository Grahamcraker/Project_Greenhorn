using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KnightBear_TD_WindowsDesktop.Screens
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden
    }
    public abstract class ScreenBase
    {
        #region Fields
        ScreenState state;
        ScreenManager manager;
        #endregion

        #region Properties
        /// <summary>
        /// The current state of the screen
        /// </summary>
        public ScreenState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// The ScreenManager this screen belongs to
        /// </summary>
        public ScreenManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }
        #endregion

        #region Load
        /// <summary>
        /// Called when screen is first created
        /// </summary>
        public virtual void Activate() { }
        #endregion

        #region Update
        public virtual void Draw(GameTime gameTime) { }

        public virtual void HandleInput() { }

        public virtual void Update(GameTime gameTime) { }
        #endregion

        #region Close
        public void CloseScreen() { }

        public virtual void Deactivate() { }

        public virtual void Unload() { }
        #endregion
    }
}

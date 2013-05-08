//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Gameplay.Entities.Player
{
    /// <summary>
    /// Represents the player. The player is visually
    /// seen as the hero bear on screen.
    /// </summary>
    class Player : Sprite
    {
        #region Fields
        private Ability attack;
        private Ability defend;
        private int health;
        private int wallet;
        #endregion

        #region Properties
        public int Wallet
        {
            get { return wallet; }
            set { wallet = value; }
        }
        #endregion

        #region Load/Update
        public Player(Texture2D texture, Vector2 position, int wallet)
            : base(texture, position)
        {
            this.wallet = wallet;
        }
        #endregion
    }
}

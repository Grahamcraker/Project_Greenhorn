//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_WindowsDesktop.GameMode;
using Microsoft.Xna.Framework;

namespace KnightBear_TD_WindowsDesktop.Entities.Player
{
    /// <summary>
    /// Represents the player. The player is visually
    /// seen as the hero bear on screen.
    /// </summary>
    class Player : GameObject
    {
        #region Member Variables
        private Ability attack;
        private Ability defend;
        private int health;
        private int wallet;
        #endregion

        #region Properties
        public Ability Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public Ability Defend
        {
            get { return defend; }
            set { defend = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Wallet
        {
            get { return wallet; }
            set { wallet = value; }
        }
        #endregion

        #region Load/Update
        public Player(int health, int wallet, Ability attack, Ability defend, Vector2 position)
        {
            Health = health;
            Wallet = wallet;
            Attack = attack;
            Defend = defend;
            Position = position;
        }

        public void Update()
        {

        }
        #endregion
    }
}

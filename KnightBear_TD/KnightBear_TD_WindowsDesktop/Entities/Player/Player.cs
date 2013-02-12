using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KnightBear_TD_WindowsDesktop.Entities
{
    /// <summary>
    /// Represents the player. The player is represented
    /// on screen by the hero bear.
    /// </summary>
    class Player : Entity
    {
        #region Member Variables
        private int health;
        private int wallet;
        private Ability attack;
        private Ability defend;
        private Vector2 position;
        #endregion

        #region Properties
        public int Health
        {
            get { return health; }
        }

        public int Wallet
        {
            get { return wallet; }
            set { wallet = value; }
        }

        public Ability Attack
        {
            get { return attack; }
        }

        public Ability Defend
        {
            get { return defend; }
        }
        #endregion

        public void Update()
        {

        }
    }
}

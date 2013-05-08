//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_Windows.Gameplay.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Gameplay.Entities.Nightmares
{
    class OldNightmare : Sprite
    {
        #region Constants
        private readonly int StandardMoveSpeed = 600;
        #endregion

        #region Fields
        private Ability nightmareAbility;
        private bool hasReachedNode;
        private double lastMove;
        private int currencyValue;
        private int health;
        private int moveSpeed;
        private int nodeIndex;
        #endregion

        #region Properties
        public Ability NightmareAbility
        {
            get { return nightmareAbility; }
            set { nightmareAbility = value; }
        }
        
        public bool HasReachedNode
        {
            get { return hasReachedNode; }
            set { hasReachedNode = value; }
        }

        public double LastMove
        {
            get { return lastMove;}
            set { lastMove = value; }
        }

        /// <summary>
        /// Amount of currency gained if nightmare is killed
        /// </summary>
        public int CurrencyValue
        {
            get { return currencyValue; }
            set { currencyValue = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// The speed multiplier at which the nightmare moves. Standard multiplier is 1
        /// Ex.
        /// Standard speed = 6 moves/second
        /// Nightmare MoveSpeed = .5
        /// Nightmare speed becomes 12 moves/second
        /// </summary>
        public int MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        public int NodeIndex
        {
            get { return nodeIndex; }
            set { nodeIndex = value; }
        }
        #endregion

        #region Load/Update
        public OldNightmare(Texture2D texture, Vector2 position, float scale, int health)
            : base(texture, position)
        {
        }

        public void Update(GameTime gameTime, Vector2 currentTarget)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastMove > StandardMoveSpeed * moveSpeed)
            {
                position += direction;
                if (position == currentTarget)
                {
                    hasReachedNode = true;
                }
            }
        }
        #endregion

    }
}

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
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Entities.Towers
{
    class Tower : GameObject
    {
        #region Member Variables
        private Ability towerAbility;
        private bool canAttack;
        private double lastAttack;
        private float range;
        private int targetIndex;
        #endregion

        #region Properties
        public Ability TowerAbility
        {
            get { return towerAbility; }
            set { towerAbility = value; }
        }

        public bool CanAttack
        {
            get { return canAttack; }
            set { canAttack = value; }
        }

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        /// <summary>
        /// Index of the nightmare currently targeted.
        /// -1 if tower has no target.
        /// </summary>
        public int TargetIndex
        {
            get { return targetIndex; }
            set { targetIndex = value; }
        }
        #endregion

        #region Load/Update
        public Tower(Vector2 position, Texture2D texture, float scale, int range, Ability attack)
        {
            Position = position;
            ObjectTexture = texture;
            Scale = scale;
            Range = range;
            TowerAbility = attack;
            Origin = new Vector2(0, 0);
            targetIndex = -1;
            CanAttack = true;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastAttack > towerAbility.Cooldown)
            {
                canAttack = true;
            }
        }
        #endregion

        #region Methods
        public void PerformAttack(GameTime gameTime)
        {
            CanAttack = false;
            lastAttack = gameTime.TotalGameTime.TotalMilliseconds;
        }
        #endregion
    }
}

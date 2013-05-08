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
using KnightBear_TD_Windows.Gameplay.Entities.Nightmares;

namespace KnightBear_TD_Windows.Gameplay.Entities.Towers
{
    public class Tower : Sprite
    {
        #region Fields
        private Ability towerAbility;
        private bool canAttack;
        private double lastAttack;
        private int range;
        private Nightmare target;
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

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        public Nightmare Target
        {
            get { return target; }
            set { target = value; }
        }
        #endregion

        #region Load/Update
        public Tower(Texture2D texture, Vector2 position, int range, Ability attack)
            : base(texture, position)
        {
            Range = range;
            TowerAbility = attack;
            CanAttack = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

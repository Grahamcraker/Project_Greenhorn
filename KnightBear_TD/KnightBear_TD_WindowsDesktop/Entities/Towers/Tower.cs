using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Entities.Towers
{
    class Tower : Entity
    {
        #region Member Variables
        private Ability attack;
        private bool canAttack;
        private double lastAttack;
        private float range;
        private int targetIndex;
        #endregion

        #region Properties
        public Ability Attack
        {
            get { return attack; }
            set { attack = value; }
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
            EntityTexture = texture;
            Scale = scale;
            Range = range;
            Attack = attack;
            targetIndex = -1;
            CanAttack = true;
        }

        public void Update(GameTime time)
        {
            if (time.TotalGameTime.TotalMilliseconds - lastAttack > attack.Cooldown)
            {
                canAttack = true;
            }
        }
        #endregion

        #region Methods
        public void PerformAttack(GameTime time)
        {
            CanAttack = false;
            lastAttack = time.TotalGameTime.TotalMilliseconds;
        }
        #endregion
    }
}

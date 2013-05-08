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
    class Projectile : Sprite
    {
        #region Fields
        private Ability projectileAbility;
        private double lastMove;
        private Nightmare target;
        #endregion

        #region Properties
        public Ability ProjectileAbility
        {
            get { return projectileAbility; }
            set { projectileAbility = value; }
        }

        public double LastMove
        {
            get { return lastMove; }
            set { lastMove = value; }
        }

        public Nightmare Target
        {
            get { return target; }
        }
        #endregion

        #region Load/Update
        public Projectile(Texture2D texture, Vector2 position, Nightmare target, Ability projectileAbility)
            : base(texture, position)
        {
            this.target = target;
            ProjectileAbility = projectileAbility;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameTime.TotalGameTime.TotalMilliseconds - lastMove > projectileAbility.MoveSpeed)
            {
                UpdatePosition(target.Center);
            }
        }
        #endregion

        #region Methods
        private void UpdatePosition(Vector2 target)
        {
            // Difference of x/y coordiantes between shot and target
            float diffX, diffY;

            diffX = target.X - position.X;
            diffY = target.Y - position.Y;
            rotation = (float)Math.Atan2(diffY, diffX);

            Vector2 forward = new Vector2(1, 0);
            Matrix rotMatrix = Matrix.CreateRotationZ(rotation);
            Vector2 shotDirection = Vector2.Transform(forward, rotMatrix);

            position += shotDirection;
        }
        #endregion
    }
}

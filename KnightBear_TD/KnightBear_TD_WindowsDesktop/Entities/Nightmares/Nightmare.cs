using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_WindowsDesktop.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Entities.Nightmares
{
    class Nightmare : Entity
    {
        #region Constants
        private readonly int StandardMoveSpeed = 600;
        #endregion

        #region Member Variables
        private Ability attack;
        private bool hasReachedNode;
        private double lastMove;
        private int currencyValue;
        private int health;
        private int moveSpeed;
        private int nodeIndex;
        #endregion

        #region Properties
        public Ability Attack
        {
            get { return attack; }
            set { attack = value; }
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
        public Nightmare(Vector2 position, Vector2 target, Texture2D texture, float scale, int nodeIndex, int health)
        {
            Position = position;
            EntityTexture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Scale = scale;
            NodeIndex = nodeIndex;
            Health = health;
            hasReachedNode = false;
            UpdateRotation(target);
        }

        public void Update(GameTime gameTime, Vector2 currentTarget)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastMove > StandardMoveSpeed * moveSpeed)
            {
                Position += Direction;
                if (Position == currentTarget)
                {
                    hasReachedNode = true;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the nightmares direction and angle
        /// </summary>
        /// <param name="target"></param>
        public void UpdateRotation(Vector2 target)
        {
            // Difference of x/y coordiantes between shot and target
            float diffX, diffY;

            diffX = target.X - Position.X;
            diffY = target.Y - Position.Y;
            Angle = (float)Math.Atan2(diffY, diffX);

            Forward = new Vector2(1, 0);
            Matrix rotMatrix = Matrix.CreateRotationZ(Angle);
            Direction = Vector2.Transform(Forward, rotMatrix);
        }

        /// <summary>
        /// Deals damage and applies special effects
        /// </summary>
        /// <param name="pAbility">Ability used against nightmare</param>
        /// <returns>True = Nightmare was killed    False = Nightmare still lives</returns>
        public bool DealDamage(Ability pAbility)
        {
            health -= pAbility.Power;

            if (health < 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Gameplay.Entities.Nightmares
{
    public class Nightmare : Sprite
    {
        #region Fields
        private bool isAlive;
        private decimal healthPercent;
        private float speed;
        private int bounty;
        private int currentHealth;
        private int maxHealth;
        private Queue<Vector2> waypoints;
        #endregion

        #region Properties
        public bool IsAlive
        {
            get { return isAlive; }
        }

        public float Health
        {
            get { return currentHealth; }
        }

        public int Bounty
        {
            get { return bounty; }
        }

        public int WaypointCount
        {
            get { return waypoints.Count(); }
        }
        #endregion

        #region Load/Update
        public Nightmare(Texture2D texture, Vector2 start, int health, int bounty, float speed, Queue<Vector2> waypoints)
            : base(texture, start)
        {
            isAlive = true;
            currentHealth = health;
            maxHealth = health;
            this.bounty = bounty;
            this.speed = speed;

            this.waypoints = waypoints;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Enter code to move nightmare
            if (position == waypoints.Peek())
            {
                waypoints.Dequeue();
            }

            if (waypoints.Count < 1)
            {
                isAlive = false;
            }
            else
            {
                UpdatePosition();
            }
            
            if (currentHealth <= 0)
            {
                isAlive = false;
            }

            healthPercent = currentHealth / maxHealth * 100;

            base.Update(gameTime);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deals damage and applies special effects
        /// </summary>
        /// <param name="pAbility">Ability used against nightmare</param>
        public void DealDamage(Ability pAbility)
        {
            currentHealth -= pAbility.AbilityPower;

            if (currentHealth <= 0)
            {
                isAlive = false;
            }
        }

        /// <summary>
        /// Updates the nightmares direction and angle
        /// </summary>
        /// <param name="target"></param>
        private void UpdatePosition()
        {
            Vector2 target = waypoints.Peek();

            // Difference of x/y coordiantes between shot and target
            float diffX, diffY;

            diffX = target.X - position.X;
            diffY = target.Y - position.Y;
            rotation = (float)Math.Atan2(diffY, diffX);

            forward = new Vector2(1, 0);
            Matrix rotMatrix = Matrix.CreateRotationZ(rotation);
            direction = Vector2.Transform(forward, rotMatrix);
        }
        #endregion
    }
}

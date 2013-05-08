using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Gameplay
{
    /// <summary>
    /// Contains all information about a GameObject's ability
    /// </summary>
    public struct Ability
    {
        public int Cooldown;
        public int AbilityPower;
        public int MoveSpeed;
        public float SpecialPower;
        public AbilityType Type;

        public Ability(int cooldown, int power, int moveSpeed, AbilityType type, float specialPower)
        {
            Cooldown = cooldown;
            AbilityPower = power;
            MoveSpeed = moveSpeed;
            SpecialPower = specialPower;
            Type = type;
        }
    }

    /// <summary>
    /// Type of ability. Used for special effects.
    /// </summary>
    public enum AbilityType
    {
        Basic,
        Slow
    }

    /// <summary>
    /// Base class for all GameMode objects
    /// </summary>
    public abstract class Sprite
    {
        #region Fields
        protected float rotation;
        protected Texture2D texture;
        protected Vector2 center, direction, forward, origin, position, velocity;
        #endregion

        #region Properties
        /// <summary>
        /// Returns a rectangle that surrounds the GameObject.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                int x = Convert.ToInt32(Math.Round(position.X - origin.X));
                int y = Convert.ToInt32(Math.Round(position.Y - origin.Y));
                int w = Convert.ToInt32(texture.Width);
                int h = Convert.ToInt32(texture.Height);

                return new Rectangle(x, y, w, h);
            }
        }

        /// <summary>
        /// Returns the coordinates for the center of the sprite. Adjusts for scale.
        /// </summary>
        public Vector2 Center
        {
            get { return center; }
        }

        public Vector2 Origin
        {
            get { return origin; }
        }

        public Vector2 Position
        {
            get { return position; }
        }
        #endregion

        #region Load/Update
        /// <summary>
        /// Default GameObject constructor.
        /// </summary>
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            center = position + origin;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            position += direction;
            this.center = position + origin;
        }
        #endregion
    }
}

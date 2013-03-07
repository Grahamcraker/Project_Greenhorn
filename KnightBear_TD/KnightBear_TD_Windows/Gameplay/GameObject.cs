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
    public class GameObject
    {
        /// <summary>
        /// The angle, in radians, at which the texture should be drawn.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// How much the image should be scaled up/down.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Returns a rectangle that surrounds the GameObject.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                int x = Convert.ToInt32(Math.Round(Position.X - (Origin.X * Scale)));
                int y = Convert.ToInt32(Math.Round(Position.Y - (Origin.Y * Scale)));
                int w = Convert.ToInt32(ObjectTexture.Width * Scale);
                int h = Convert.ToInt32(ObjectTexture.Height * Scale);

                return new Rectangle(x, y, w, h);
            }
        }

        /// <summary>
        /// The Texture2D associated with this GameObject.
        /// </summary>
        public Texture2D ObjectTexture { get; set; }

        /// <summary>
        /// The direction this GameObject will move on the screen.
        /// </summary>
        /// <remarks>
        /// The idea of a direction is a little difficult at first glance.
        /// The purpose of a 2D Vector is to add the current position to this
        /// value. The resulting coordinate will be the new position.
        /// Ex. If I have a direction of (1,0) and add it to my current position,
        /// (10,10) the new position will be (11,10), resulting in an increased
        /// position on the X-axis.
        /// </remarks>
        public Vector2 Direction { get; set; }

        /// <summary>
        /// The direction of the "Forward" direction of this GameObject's texture.
        /// </summary>
        /// <remarks>
        /// This property determines what is the forward direction
        /// of a texture. i.e. If a texture of a nightmare is looking up
        /// by default, the Forward property would equal (0,-1), representing
        /// an up direction.
        /// </remarks>
        public Vector2 Forward { get; set; }

        /// <summary>
        /// Point on the texture that should be drawn at this GameObject's Position.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Coordinate of this GameObject's position on the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Default GameObject constructor.
        /// </summary>
        public GameObject() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ObjectTexture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }

        public virtual void Update(float scale)
        {
            Scale = scale;
        }
    }
}

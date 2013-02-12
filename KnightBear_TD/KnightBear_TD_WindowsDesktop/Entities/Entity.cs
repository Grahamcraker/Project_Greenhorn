using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_WindowsDesktop.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Entities
{
    public struct Ability
    {
        public int Cooldown;
        public int Power;
        public int MoveSpeed;
        public float SpecialPower;
        public AbilityType Type;

        public Ability(int cooldown, int power, int moveSpeed, AbilityType type, float specialPower)
        {
            Cooldown = cooldown;
            Power = power;
            MoveSpeed = moveSpeed;
            SpecialPower = specialPower;
            Type = type;
        }
    }

    public enum AbilityType
    {
        BASIC,
        SLOW
    }

    /// <summary>
    /// Base class all entities inherit from. Contains
    /// animation and update methods.
    /// </summary>
    abstract class Entity : IHasTexture
    {
        #region Member Variables
        private float angle;
        private float scale;
        private int width;
        private int height;
        private Vector2 direction;
        private Vector2 forward;
        private Vector2 origin;
        private Vector2 position;
        private Texture2D entityTexture;
        private Dictionary<string, List<Texture2D>> animations;
        #endregion

        #region Properties
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Vector2 Forward
        {
            get { return forward; }
            set { forward = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D EntityTexture
        {
            get { return entityTexture; }
            set { entityTexture = value; }
        }
        #endregion

        public Entity() { }

        public void AddAnimation(string name, List<Texture2D> animation)
        {
            animations.Add(name, animation);
        }

        public List<Texture2D> GetAnimation(string animationName)
        {
            return animations[animationName];
        }
    }
}

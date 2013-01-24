using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Entities
{
    class Nightmare
    {
        public int LocationIndex { get; private set; }
        public float Scale { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public int Damage { get; private set; }
        public int AttackSpeed { get; private set; }
        public string Name { get; private set; }
        public Texture2D Icon { get; private set; }
        public Vector2 Position;

        public Nightmare(Texture2D icon, Vector2 position)
        {
            LocationIndex = 0;
            Scale = 80.0f / icon.Width;
            Width = icon.Width * Scale;
            Height = icon.Height * Scale;
            Icon = icon;
            Position = position;
        }

        public void ResetPosition(float newX, float newY)
        {
            Position.X = newX;
            Position.Y = newY;
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            Position = newPosition;
            LocationIndex += 1;
        }
    }
}

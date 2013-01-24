using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Entities
{
    class Tower
    {
        public float Scale { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public int Damage { get; private set; }
        public int AttackSpeed { get; private set; }
        public string Name { get; private set; }
        public Texture2D TowerIcon { get; private set; }
        public Texture2D WeaponIcon { get; private set; }
        public Vector2 Position;

        public Tower(Texture2D towerIcon, Texture2D weaponIcon, Vector2 position)
        {
            TowerIcon = towerIcon;
            WeaponIcon = weaponIcon;
            Position = position;
            Scale = 50.0f / towerIcon.Width;
            Width = towerIcon.Width * Scale;
            Height = towerIcon.Height * Scale;
        }
    }
}

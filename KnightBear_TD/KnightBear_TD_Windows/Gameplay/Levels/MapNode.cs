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

namespace KnightBear_TD_Windows.Gameplay.Levels
{
    public class MapNode
    {
        #region Fields
        Texture2D texture;
        NodeType type;
        MapNode[] neighbors;
        Vector2 position;
        #endregion

        #region Properties
        public NodeType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Returns a rectangle that surrounds the GameObject.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                int x = Convert.ToInt32(Math.Round(position.X));
                int y = Convert.ToInt32(Math.Round(position.Y));
                int w = Convert.ToInt32(texture.Width);
                int h = Convert.ToInt32(texture.Height);

                return new Rectangle(x, y, w, h);
            }
        }

        public Vector2 Center
        {
            get { return position + new Vector2(25, 25); }
        }

        public Vector2 Position
        {
            get { return position; }
        }
        #endregion

        #region Load/Update
        public MapNode(Texture2D texture, Vector2 position, NodeType type)
        {
            this.texture = texture;
            this.position = position;
            this.type = type;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        }
        #endregion
    }

    public enum NodeType
    {
        Buildable,
        NonBuildable,
        Path,
        PathEnd,
        PathStart
    }
}

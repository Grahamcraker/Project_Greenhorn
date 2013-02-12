using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_WindowsDesktop.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Levels
{
    class MapNode : IHasTexture
    {
        #region Member Variables
        private float angle;
        private float scale;
        private int width;
        private int height;
        private NodeType type;
        private Texture2D nodeTexture;
        private Vector2 direction;
        private Vector2 forward;
        private Vector2 origin;
        private Vector2 position;
        #endregion

        #region Properties
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

        public NodeType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Texture2D NodeTexture
        {
            get { return nodeTexture; }
            set { nodeTexture = value; }
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
        #endregion

        #region Load/Update
        public MapNode(NodeType type, Texture2D texture)
        {
            Type = type;
            NodeTexture = texture;
        }
        #endregion
    }

    enum NodeType
    {
        NONBUILDABLE,
        BUILDABLE,
        NIGHTMAREPATH
    }
}

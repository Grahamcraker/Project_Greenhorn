using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Levels
{
    class MapNode
    {
        private NodeType type;
        private Texture2D nodeTexture;
        private Vector2 position;
        private float scale;

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
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public MapNode(NodeType type, Texture2D texture)
        {
            Type = type;
            NodeTexture = texture;
        }
    }

    enum NodeType
    {
        NONBUILDABLE,
        BUILDABLE,
        NIGHTMAREPATH
    }
}

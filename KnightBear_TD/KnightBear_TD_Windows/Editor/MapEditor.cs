//-----------------------------------------------------------------------
// <copyright file="MapEditor.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_Windows.Gameplay.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Editor
{
    class MapEditor
    {
        #region Fields
        private int nodeWidth;
        private int nodeHeight;
        private readonly int verticalNodeCount;
        private readonly int horizontalNodeCount;
        private Map map;
        private Texture2D buildableTexture;
        private Texture2D nonBuildableTexture;
        private Texture2D pathTexture;
        #endregion

        #region Load
        public MapEditor(ContentManager content, int mapWidth, int mapHeight, int vNodeCount, int hNodeCount)
        {
            Dictionary<NodeType, Texture2D> textures = new Dictionary<NodeType,Texture2D>();

            verticalNodeCount = vNodeCount;
            horizontalNodeCount = hNodeCount;

            nodeWidth = mapWidth / horizontalNodeCount;
            nodeHeight = mapHeight / verticalNodeCount;

            // Load the textures for the different node types
            textures.Add(NodeType.Buildable, content.Load<Texture2D>("images/bgBuildable"));
            textures.Add(NodeType.NonBuildable, content.Load<Texture2D>("images/bgNonBuildable"));
            textures.Add(NodeType.Path, content.Load<Texture2D>("images/bgNightmarePath"));

            int[,] layout = CreateLayout(vNodeCount, hNodeCount);

            //map = new Map(layout, textures);
        }
        #endregion

        #region Update
        public void Update(int mapWidth, int mapHeight)
        {
            nodeWidth = mapWidth / horizontalNodeCount;
            nodeHeight = mapHeight / verticalNodeCount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
        }
        #endregion

        #region Methods
        private int[,] CreateLayout(int vCount, int hCount)
        {
            int[,] layout = new int[hCount, vCount];

            for (int i = 0; i < hCount; i++)
            {
                for (int j = 0; j < vCount; j++)
                {
                    layout[i, j] = 0;
                }
            }

            return layout;
        }

        public void CycleNode(Vector2 clickPosition)
        {
            // TODO: Implement code to cycle the nodetype
            //Rectangle mouseRec = new Rectangle((int)clickPosition.X, (int)clickPosition.Y, 1, 1);

            //foreach (MapNode node in mapNodes)
            //{
            //    if (mouseRec.Intersects(node.Bounds))
            //    {
            //        switch (node.Type)
            //        {
            //            case NodeType.NonBuildable:
            //                node.Type = NodeType.Buildable;
            //                break;
            //            case NodeType.Buildable:
            //                node.Type = NodeType.Path;
            //                break;
            //            case NodeType.Path:
            //                node.Type = NodeType.NonBuildable;
            //                break;
            //        }

            //        break;
            //    }
            //}
        }
        #endregion
    }
}

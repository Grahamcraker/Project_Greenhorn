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
    class MapNode : GameObject
    {
        #region Fields
        private NodeType type;
        #endregion

        #region Properties
        public NodeType Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion

        #region Load/Update
        public MapNode(NodeType type, Texture2D texture)
        {
            Type = type;
            ObjectTexture = texture;
        }
        #endregion
    }

    enum NodeType
    {
        NonBuildable,
        Buildable,
        NightmarePath
    }
}

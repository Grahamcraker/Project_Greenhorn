//-----------------------------------------------------------------------
// <copyright file="XmlOperator.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using KnightBear_TD_Windows.Gameplay.Levels;

public enum LevelType
{
    Bonus,
    Custom,
    Story
}

namespace KnightBear_TD_Windows
{
    /// <summary>
    /// This class manages all XML loading. This is a singleton.
    /// </summary>
    public class XmlController
    {
        #region Singleton Code
        private static XmlController controller;

        public static XmlController Controller
        {
            get
            {
                if (controller == null)
                {
                    controller = new XmlController();
                }

                return controller;
            }
        }

        /// <summary>
        /// Private constructor only called when initally creating singleton
        /// </summary>
        private XmlController()
        {

        }
        #endregion

        #region Fields
        readonly string rootConfigLocation = @"..\..\..\Config Files\";
        XmlDocument doc;
        #endregion

        #region Methods
        /// <summary>
        /// Prepares a new level node to be saved
        /// </summary>
        /// <param name="levelNumber">What level is this</param>
        /// <param name="config"></param>
        /// <returns></returns>
        private XmlNode ConfigureNode(int levelNumber, LevelConfig config)
        {
            // TODO: Fix this
            List<XmlNode> nodes = new List<XmlNode>();
            string buildableIds = String.Empty;
            string pathIds = String.Empty;
            XmlNode subNode, walletNode;
            
            // Initialize the new level XmlNode
            XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "level", null);
            XmlAttribute att = doc.CreateAttribute("number");
            att.Value = levelNumber.ToString();
            newNode.Attributes.Append(att);

            
            subNode = doc.CreateNode(XmlNodeType.Element, "buildable_texture", null);
            subNode.InnerText = config.NodeTextures[NodeType.Buildable];
            newNode.AppendChild(subNode);

            subNode = doc.CreateNode(XmlNodeType.Element, "nonbuildable_texture", null);
            subNode.InnerText = config.NodeTextures[NodeType.NonBuildable];
            newNode.AppendChild(subNode);

            subNode = doc.CreateNode(XmlNodeType.Element, "path_texture", null);
            subNode.InnerText = config.NodeTextures[NodeType.Path];
            newNode.AppendChild(subNode);

            walletNode = doc.CreateNode(XmlNodeType.Element, "wallet_start_amount", null);
            walletNode.InnerText = config.WalletStartAmount.ToString();
            newNode.AppendChild(walletNode);

            return newNode;
        }

        private List<string[]> GetWaypoints(string[] paths)
        {
            List<string[]> layout = new List<string[]>();

            foreach (string path in paths)
            {
                string[] points = path.Split(',');

                layout.Add(points);
            }

            return layout;
        }

        /// <summary>
        /// Loads the configuration for the given level.
        /// </summary>
        /// <param name="levelNumber">Which level to load</param>
        /// <returns>The requested level config</returns>
        public LevelConfig LoadLevelConfig(int levelNumber, LevelType type)
        {
            Dictionary<NodeType, string> nodeTextures = new Dictionary<NodeType, string>();
            doc = new XmlDocument();
            int[,] mapLayout;
            List<string[]> waypointLayout;
            LevelConfig config;
            string fileLocation = rootConfigLocation + "Levels.xml";
            string baseNodeName = String.Empty;

            switch (type)
            {
                case LevelType.Bonus:
                    baseNodeName = String.Format("levels/bonus/level[@number='{0}']", levelNumber);
                    break;
                case LevelType.Custom:
                    baseNodeName = String.Format("levels/custom/level[@number='{0}']", levelNumber);
                    break;
                default:
                    baseNodeName = String.Format("levels/story/level[@number='{0}']", levelNumber);
                    break;
            }

            doc.Load(fileLocation);
            XmlNode baseNode = doc.SelectSingleNode(baseNodeName);
            
            // Parse the map layout
            string[] rows = baseNode.SelectSingleNode("map").InnerText.Split(';');

            int mapHeight = rows.Length;
            int mapWidth = rows[0].Split(',').Length;
            mapLayout = new int[mapWidth, mapHeight];

            for (int i = 0; i < rows.Length; i++)
            {
                string[] nodes = rows[i].Split(',');

                for (int j = 0; j < nodes.Length; j++)
                {
                    mapLayout[i, j] = Convert.ToInt32(nodes[j]);
                }
            }

            // Load Waypoints
            waypointLayout = GetWaypoints(baseNode.SelectSingleNode("waypoints").InnerText.Split(';'));

            // Load wallet starting amount
            int startingWallet = Convert.ToInt32(baseNode.SelectSingleNode("wallet_start_amount").InnerText);

            // Load Textures
            nodeTextures.Add(NodeType.Buildable, baseNode.SelectSingleNode("buildable_texture").InnerText);
            nodeTextures.Add(NodeType.NonBuildable, baseNode.SelectSingleNode("nonbuildable_texture").InnerText);
            nodeTextures.Add(NodeType.Path, baseNode.SelectSingleNode("path_texture").InnerText);

            config = new LevelConfig( mapLayout
                                    , waypointLayout
                                    , startingWallet
                                    , nodeTextures
                                    );

            return config;
        }

        /// <summary>
        /// Saves a level configuration
        /// </summary>
        /// <param name="levelNumber">Which level is being saved</param>
        /// <param name="config">The level's configuration</param>
        /// <param name="type">What type of level is being saved</param>
        public void SaveLevelConfig(int levelNumber, LevelConfig config, LevelType type)
        {
            bool isNodeCreated = false;
            int nodeIndex = levelNumber;
            string currentNodeName = String.Format("level[@number='{0}']", levelNumber);
            string baseNodeName;
            string fileLocation = rootConfigLocation + "Levels.xml";
            doc = new XmlDocument();
            XmlNode baseNode, prevNode = null;

            //Create our new node
            XmlNode newNode = ConfigureNode(levelNumber, config);
            doc.Load(fileLocation);

            switch (type)
            {
                case LevelType.Bonus:
                    baseNodeName = String.Format("levels/bonus/level[@number='{0}']", levelNumber);
                    break;
                case LevelType.Custom:
                    baseNodeName = String.Format("levels/custom/level[@number='{0}']", levelNumber);
                    break;
                default:
                    baseNodeName = String.Format("levels/story/level[@number='{0}']", levelNumber);
                    break;
            }

            baseNode = doc.SelectSingleNode(baseNodeName);

            // Check if any levels exist
            if (baseNode.SelectNodes("level").Count == 0)
            {
                // No levels have been created yet so we don't care about order
                baseNode.AppendChild(newNode);
                doc.Save(fileLocation);
                return;
            }
            
            // Check if this level already exists
            if (baseNode.SelectSingleNode(currentNodeName) != null)
            {
                // Delete the original
                baseNode.RemoveChild(doc.SelectSingleNode(currentNodeName));
            }

            while (!isNodeCreated)
            {
                nodeIndex--;

                currentNodeName = String.Format("level[@number='{0}]'", nodeIndex);
                prevNode = baseNode.SelectSingleNode(currentNodeName);

                if (prevNode != null)
                {
                    baseNode.InsertAfter(newNode, prevNode);
                    isNodeCreated = true;
                }
            }

            doc.Save(fileLocation);
        }
        #endregion
    }
}

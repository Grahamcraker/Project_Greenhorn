//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using KnightBear_TD_Windows.Gameplay.Levels;

namespace KnightBear_TD_Windows
{
    /// <summary>
    /// This class manages all XML loading. This is a singleton.
    /// </summary>
    public class XmlLoader
    {
        #region Singleton Code
        private static XmlLoader loader;

        public static XmlLoader Loader
        {
            get
            {
                if (loader == null)
                {
                    loader = new XmlLoader();
                }

                return loader;
            }
        }

        /// <summary>
        /// Private constructor only called when initally creating singleton
        /// </summary>
        private XmlLoader()
        {

        }
        #endregion

        #region Fields
        readonly string levelConfigFile = @"..\..\..\GamePlay\Levels\Levels.xml";
        #endregion

        #region Methods
        /// <summary>
        /// Loads the configuration for the given level.
        /// </summary>
        /// <param name="level">Which level to load</param>
        /// <returns>The requested level config</returns>
        public LevelConfig GenerateLevelConfig(int level)
        {
            Dictionary<string, string> textures = new Dictionary<string,string>();
            XmlDocument doc = new XmlDocument();
            List<int> build = new List<int>();
            List<int> path = new List<int>();
            LevelConfig config;

            doc.Load(levelConfigFile);
            XmlNode baseNode = doc.SelectSingleNode(String.Format("/levels/level{0}", level));

            string[] list = baseNode.SelectSingleNode("nodes/build/ids").InnerText.Split(new char[] { ',' });
            foreach (string str in list)
            {
                build.Add(Convert.ToInt32(str));
            }

            list = baseNode.SelectSingleNode("nodes/path/ids").InnerText.Split(new char[] { ',' });
            foreach (string str in list)
            {
                path.Add(Convert.ToInt32(str));
            }

            textures.Add("NONBUILD", baseNode.SelectSingleNode("nodes/nonbuild/texture").InnerText);
            textures.Add("BUILD", baseNode.SelectSingleNode("nodes/build/texture").InnerText);
            textures.Add("PATH", baseNode.SelectSingleNode("nodes/path/texture").InnerText);

            config = new LevelConfig(
                                      Convert.ToInt32(baseNode.SelectSingleNode("nodes/horizontalNodeCount").InnerText)
                                    , Convert.ToInt32(baseNode.SelectSingleNode("nodes/verticalNodeCount").InnerText)
                                    , Convert.ToInt32(baseNode.SelectSingleNode("walletStart").InnerText)
                                    , build
                                    , path
                                    , textures
                                    );

            return config;
        }
        #endregion
    }
}

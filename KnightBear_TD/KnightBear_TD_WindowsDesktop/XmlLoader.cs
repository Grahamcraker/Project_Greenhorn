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
using KnightBear_TD_WindowsDesktop.Levels;

namespace KnightBear_TD_WindowsDesktop
{
    static class XmlLoader
    {
        static string levelConfigFile = @"..\..\..\Levels\Levels.xml";

        public static LevelConfig GenerateLevelConfig(int level)
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
    }
}

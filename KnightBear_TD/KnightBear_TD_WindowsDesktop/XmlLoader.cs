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
            XmlNode baseNode = doc.SelectSingleNode(String.Format("/levels/level{0}/nodes", level));

            string[] list = baseNode.SelectSingleNode("build/ids").InnerText.Split(new char[] { ',' });
            foreach (string str in list)
            {
                build.Add(Convert.ToInt32(str));
            }

            list = baseNode.SelectSingleNode("path/ids").InnerText.Split(new char[] { ',' });
            foreach (string str in list)
            {
                path.Add(Convert.ToInt32(str));
            }

            textures.Add("NONBUILD", baseNode.SelectSingleNode("nonbuild/texture").InnerText);
            textures.Add("BUILD", baseNode.SelectSingleNode("build/texture").InnerText);
            textures.Add("PATH", baseNode.SelectSingleNode("path/texture").InnerText);
            


            config = new LevelConfig(
                                      Convert.ToInt32(baseNode.SelectSingleNode("horizontalNodeCount").InnerText)
                                    , Convert.ToInt32(baseNode.SelectSingleNode("verticalNodeCount").InnerText)
                                    , build
                                    , path
                                    , textures
                                    );

            return config;
        }
    }
}

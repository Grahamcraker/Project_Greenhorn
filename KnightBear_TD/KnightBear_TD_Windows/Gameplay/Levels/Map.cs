using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_Windows.Gameplay.Levels
{
    public class Map
    {
        #region Fields
        private MapNode[,] nodeLayout;
        private List<Queue<Vector2>> waypoints;
        private Dictionary<NodeType, Texture2D> textures;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the number of nodes the map is High.
        /// </summary>
        /// <remarks>
        /// This value is determined by checking the length of the map array
        /// along the x-axis. Remember that the x/y axes are switched within
        /// a bi-dimensional array.
        /// </remarks>
        public int Height
        {
            get { return nodeLayout.GetLength(0); }
        }

        /// <summary>
        /// Returns the number of nodes the map is wide.
        /// </summary>
        /// <remarks>
        /// This value is determined by checking the length of the map array
        /// along the y-axis. Remember that the x/y axes are switched within
        /// a bi-dimensional array.
        /// </remarks>
        public int Width
        {
            get { return nodeLayout.GetLength(1); }
        }
        #endregion

        #region Load/Update
        public Map(int[,] layout, Dictionary<NodeType, Texture2D> nodeTextures, List<string[]> waypointList)
        {
            waypoints = new List<Queue<Vector2>>();
            this.textures = nodeTextures;
            InitNodes(layout);

            if (waypointList != null)
            {
                InitWaypoints(waypointList);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    nodeLayout[x, y].Draw(spriteBatch);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks if the given position intersects a map node.
        /// </summary>
        /// <param name="position">Location of the action</param>
        /// <returns>The MapNode that intersects the given position. Null if no intersection exists.</returns>
        public MapNode CheckCollision(Vector2 position)
        {
            Rectangle targetRec = new Rectangle((int)position.X, (int)position.Y, 1, 1);

            foreach (MapNode node in nodeLayout)
            {
                // Collision occurs on this node
                if (targetRec.Intersects(node.Bounds))
                {
                    return node;
                }
            }

            return null;
        }

        public MapNode GetPlayerPosition()
        {
            foreach (MapNode node in nodeLayout)
            {
                if (node.Type == NodeType.PathEnd)
                {
                    return node;
                }
            }

            return null;
        }

        public MapNode GetNode(int x, int y)
        {
            return nodeLayout[x, y];
        }

        public Texture2D GetTexture(NodeType type)
        {
            return textures[type];
        }

        /// <summary>
        /// Returns a queue of all the waypoints used for nightmare travel.
        /// </summary>
        /// <param name="pathIndex">Which path is being used</param>
        public Queue<Vector2> GetWaypoints(int pathIndex)
        {
            Queue<Vector2> points = new Queue<Vector2>();

            foreach (Vector2 point in waypoints[pathIndex])
            {
                points.Enqueue(point);
            }

            return points;
        }

        /// <summary>
        /// Populates the MapNode[,] layout with the given config.
        /// </summary>
        /// <param name="layout">The layout configuration for the level</param>
        private void InitNodes(int[,] layout)
        {
            Point[] neighborIndexes;
            MapNode node;
            NodeType type;
            Texture2D texture;
            Vector2 position;
            int width = layout.GetLength(1);
            int height = layout.GetLength(0);
            
            nodeLayout = new MapNode[height,width];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (layout[x, y])
                    {
                        case 0:
                            type = NodeType.Buildable;
                            break;
                        case 2:
                            type = NodeType.Path;
                            break;
                        case 3:
                            type = NodeType.PathEnd;
                            break;
                        case 4:
                            type = NodeType.PathStart;
                            break;
                        default:
                            type = NodeType.NonBuildable;
                            break;
                    }

                    if (type == NodeType.PathEnd || type == NodeType.PathStart)
                    {
                        texture = textures[NodeType.Path];
                    }
                    else
                    {
                        texture = textures[type];
                    }

                    position = new Vector2(y * 50, x * 50);
                    Console.WriteLine(position);

                    nodeLayout[x, y] = new MapNode(texture, position, type);
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    node = nodeLayout[x, y];

                    neighborIndexes = new Point[]
                    {
                        new Point(x, y - 1),
                        new Point(x + 1, y),
                        new Point(x, y + 1),
                        new Point(x - 1, y)
                    };

                    // References neighboring nodes in the following order:
                    // Top, Right, Down, Left
                    for (int i = 0; i < neighborIndexes.Length; i++)
                    {
                        Point neighbor = neighborIndexes[i];

                        if (neighbor.X < 0 || neighbor.X > width - 1 ||
                            neighbor.Y < 0 || neighbor.Y > height - 1)
                        {
                            continue;
                        }

                        node.neighbors[i] = nodeLayout[x, y];
                    }
                }
            }
        }

        /// <summary>
        /// Creates queues containing waypoints for each nightmare path.
        /// </summary>
        /// <param name="waypointList">List of nightmare paths</param>
        /// <remarks>
        /// This system is a little complex. This method will receive a list
        /// of string arrays. Each array represents a path. Each string within an array
        /// represents a single waypoint. Each waypoint must be changed from a location
        /// within the nodeLayout([x, y]) to a Vector2 representing the actual waypoint
        /// position.
        /// </remarks>
        private void InitWaypoints(List<string[]> waypointList)
        {
            Queue<Vector2> waypointQueue;

            foreach (string[] path in waypointList)
            {
                waypointQueue = new Queue<Vector2>();

                foreach (string waypoint in path)
                {
                    string[] node = waypoint.Split('.');

                    int x = Convert.ToInt32(node[0]);
                    int y = Convert.ToInt32(node[1]);

                    // Flip X/Y to get the correct node.
                    waypointQueue.Enqueue(nodeLayout[y, x].Center);
                }

                waypoints.Add(waypointQueue);
            }
        }
        #endregion
    }
}

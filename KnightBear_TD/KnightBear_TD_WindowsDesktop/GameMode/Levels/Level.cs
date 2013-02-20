//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_WindowsDesktop.Entities;
using KnightBear_TD_WindowsDesktop.Entities.Nightmares;
using KnightBear_TD_WindowsDesktop.Entities.Player;
using KnightBear_TD_WindowsDesktop.Entities.Towers;
using KnightBear_TD_WindowsDesktop.GameMode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_WindowsDesktop.Levels
{
    /// <summary>
    /// Contains all the configuration needed to run a level
    /// </summary>
    struct LevelConfig
    {
        /// <summary>
        /// Height of each node on the map.
        /// </summary>
        public int NodeHeight { get; set; }
        /// <summary>
        /// Width of each node on the map.
        /// </summary>
        public int NodeWidth { get; set; }
        /// <summary>
        /// Number of nodes the level has horizontally.
        /// </summary>
        public int HorizontalNodeCount { get; set; }
        /// <summary>
        /// Number of nodes the level has vertically.
        /// </summary>
        public int VerticalNodeCount { get; set; }
        /// <summary>
        /// Starting amount for Player's Wallet.
        /// </summary>
        public int WalletStartAmount { get; set; }
        /// <summary>
        /// Comma separated list of BUILDABLE node indexes
        /// </summary>
        public List<int> BuildNodes { get; set; }
        /// <summary>
        /// Comma separated list of NIGHTMAREPATH node indexes
        /// </summary>
        public List<int> PathNodes { get; set; }
        /// <summary>
        /// Dictionary containing towers available for this level.
        /// Key = Slot #
        /// Value = Tower
        /// </summary>
        public Dictionary<int, Tower> Towers { get; set; }
        /// <summary>
        /// Dictionary containing textures used by level
        /// Key = In-game texture name
        /// Value = Texture filename
        /// </summary>
        public Dictionary<string, string> Textures { get; set; }

        public LevelConfig(int hCount, int vCount, int walletStart, List<int> build, List<int> path, Dictionary<string, string> textures)
            : this()
        {
            HorizontalNodeCount = hCount;
            VerticalNodeCount = vCount;
            WalletStartAmount = walletStart;
            BuildNodes = build;
            PathNodes = path;
            Textures = textures;
        }
    }

    class Level
    {
        #region Constants
        /// <summary>
        /// Represents no collision occuring between a projectile and its target
        /// </summary>
        private readonly Vector2 NoCollision = new Vector2(-1, -1);
        /// <summary>
        /// Represents a tower having no target
        /// </summary>
        private readonly int NoTarget = -1;
        #endregion

        #region Member Variables
        /// <summary>
        /// True = Player is allowed to place a tower   False = Player cannot place a tower
        /// </summary>
        private bool canPlaceTower;
        /// <summary>
        /// True = Nightmare can be spawned   False = Nightmare cannot be spawned.
        /// </summary>
        /// <remarks>This should become obsolete once automatic spawning occurs</remarks>
        private bool canPlaceNightmare;
        /// <summary>
        /// ContentManager for the level
        /// </summary>
        private ContentManager content;
        /// <summary>
        /// Dictionary containing all the loaded textures
        /// </summary>
        private Dictionary<string, Texture2D> textures;
        /// <summary>
        /// List containing the IDs(in mapNodes list) of buildable nodes.
        /// </summary>
        private List<int> buildNodes;
        /// <summary>
        /// List containing the IDs(in mapNodes list) of nightmare path nodes.
        /// </summary>
        private List<int> pathNodes;
        /// <summary>
        /// List of all mapnodes for the level
        /// </summary>
        private List<MapNode> mapNodes;
        /// <summary>
        /// List of all nightmares
        /// </summary>
        private List<Nightmare> nightmares;
        /// <summary>
        /// List of all towers
        /// </summary>
        private List<Tower> towers;
        /// <summary>
        /// List of all projectiles
        /// </summary>
        private List<Projectile> projectiles;
        /// <summary>
        /// Configuration for the level
        /// </summary>
        private LevelConfig config;
        /// <summary>
        /// The current player
        /// </summary>
        private Player player;
        /// <summary>
        /// The center of a map node
        /// </summary>
        private Vector2 nodeCenter;
        #endregion

        #region Load
        /// <summary>
        /// Primary constructor for Level
        /// </summary>
        /// <param name="service">ServiceProvider for the main game</param>
        /// <param name="config"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public Level(IServiceProvider service, LevelConfig config, int screenWidth, int screenHeight)
        {
            this.config = config;
            this.config.NodeWidth = screenWidth / config.HorizontalNodeCount;
            this.config.NodeHeight = screenHeight / config.VerticalNodeCount;
            nodeCenter = new Vector2(this.config.NodeWidth / 2, this.config.NodeHeight / 2);
            this.buildNodes = config.BuildNodes;
            this.pathNodes = config.PathNodes;
            mapNodes = new List<MapNode>();
            nightmares = new List<Nightmare>();
            towers = new List<Tower>();
            projectiles = new List<Projectile>();
            textures = new Dictionary<string,Texture2D>();
            content = new ContentManager(service, "../../../../../Content");
            LoadTextures();
            InitNodes();
            InitNightmares();
            Vector2 playerPosition = mapNodes[pathNodes.Count - 1].Position;
            player = new Player(100, config.WalletStartAmount, new Ability(), new Ability(), playerPosition);//TODO: Add attack/defend abilities
        }

        /// <summary>
        /// Currently does nothing. Placeholder used to create nightmares according to
        /// level configuration options.
        /// </summary>
        private void InitNightmares()
        {
            // TODO: Write code to create nightmares for this level.
        }

        /// <summary>
        /// Initializes map nodes according to level configuration. All nodes are
        /// initially created as NONBUILDABLE nodes, then changed to the appropriate node type.
        /// </summary>
        private void InitNodes()
        {
            int nodeCount = config.HorizontalNodeCount * config.VerticalNodeCount;
            for (var counter = 0; counter < nodeCount; counter++)
            {
                mapNodes.Add(new MapNode(NodeType.NONBUILDABLE, textures["NONBUILD"]));
            }

            foreach (var id in buildNodes)
            {
                mapNodes[id].Type = NodeType.BUILDABLE;
                mapNodes[id].ObjectTexture = textures["BUILD"];
            }

            foreach (var id in pathNodes)
            {
                mapNodes[id].Type = NodeType.NIGHTMAREPATH;
                mapNodes[id].ObjectTexture = textures["PATH"];
            }

            MapNode currentNode;
            int index = 0;

            for (var v = 0; v < config.VerticalNodeCount; v++)
            {
                for (var h = 0; h < config.HorizontalNodeCount; h++)
                {
                    currentNode = mapNodes[index];
                    currentNode.Position = new Vector2(h * config.NodeWidth, v * config.NodeHeight);
                    currentNode.Origin = new Vector2(currentNode.ObjectTexture.Width / 2, currentNode.ObjectTexture.Height / 2);
                    currentNode.Scale = (float)config.NodeWidth / currentNode.ObjectTexture.Width;
                    index++;
                }
            }
        }

        /// <summary>
        /// Loads all the textures needed for the level
        /// </summary>
        private void LoadTextures()
        {
            textures.Add("BUILD", content.Load<Texture2D>("images/converted/" + config.Textures["BUILD"]));
            textures.Add("PATH", content.Load<Texture2D>("images/converted/" + config.Textures["PATH"]));
            textures.Add("NONBUILD", content.Load<Texture2D>("images/converted/" + config.Textures["NONBUILD"]));
            textures.Add("BACKGROUND", content.Load<Texture2D>("images/converted/backgroundImage"));
            textures.Add("NIGHTMARE", content.Load<Texture2D>("images/converted/lime"));
            textures.Add("TOWER", content.Load<Texture2D>("images/converted/basicTowerIcon"));
            textures.Add("PROJECTILE", content.Load<Texture2D>("images/converted/laser"));
        }
        #endregion

        #region Update
        /// <summary>
        /// Draws all the backgrounds and entities to the screen
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch which will perform all Draw calls</param>
        /// <param name="screenWidth">Width of the screen</param>
        /// <param name="screenHeight">Height of the screen</param>
        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            //spriteBatch.Draw(textures["BACKGROUND"], new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            foreach (MapNode node in mapNodes)
            {
                spriteBatch.Draw(node.ObjectTexture
                                , node.Position
                                , null
                                , Color.White
                                , 0
                                , new Vector2(0, 0)
                                , node.Scale
                                , SpriteEffects.None
                                , 0
                                );
            }

            foreach (Nightmare n in nightmares)
            {
                spriteBatch.Draw(n.ObjectTexture, n.Position, null, Color.White, n.Angle, n.Origin, n.Scale, SpriteEffects.None, 0);
            }

            foreach (Tower t in towers)
            {
                spriteBatch.Draw(t.ObjectTexture, t.Position, null, Color.White, t.Angle, t.Origin, t.Scale, SpriteEffects.None, 0);
            }
            
            foreach (Projectile p in projectiles)
            {
                spriteBatch.Draw(p.ObjectTexture, p.Position, null, Color.White, p.Angle, p.Origin, p.Scale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Calls updates on all game variables
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        /// <param name="keyState">Current Keyboard state</param>
        /// <param name="mouseState">Current Mouse state</param>
        /// <param name="screenWidth">Width of the screen</param>
        /// <param name="screenHeight">Geight of the screen</param>
        public void Update(GameTime gameTime, KeyboardState keyState, MouseState mouseState, int screenWidth, int screenHeight)
        {
            config.NodeWidth = screenWidth / config.HorizontalNodeCount;
            config.NodeHeight = screenHeight / config.VerticalNodeCount;
            ProcessInput(keyState, mouseState);
            UpdateEntities(gameTime);
            UpdatePlayer();
            DetectCollisions();
        }

        /// <summary>
        /// Updates all game entities
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        private void UpdateEntities(GameTime gameTime)
        {
            // Update projectiles
            foreach (Projectile p in projectiles)
            {
                p.Update(gameTime, nightmares[p.TargetIndex].Position);
            }

            // Update towers
            foreach (Tower tw in towers)
            {
                tw.Update(gameTime);

                // Tower can attack and a target is within range
                if (tw.CanAttack && CheckTarget(tw))
                {
                    Vector2 start = tw.Position + nodeCenter;
                    tw.PerformAttack(gameTime);
                    projectiles.Add(new Projectile(textures["PROJECTILE"], start, 0.2f, tw.TargetIndex, tw.TowerAbility));
                }
            }

            //Update Nightmares
            for (int i = nightmares.Count - 1; i >= 0; i--)
            {
                Nightmare nm = nightmares[i];
                Vector2 target = new Vector2(config.NodeWidth / 2, config.NodeHeight / 2) + mapNodes[nm.NodeIndex].Position;
                nm.Update(gameTime, target);

                if (nm.HasReachedNode)
                {
                    // Check if the nightmare has reached the final node
                    if (nm.NodeIndex == pathNodes[pathNodes.Count - 1])
                    {
                        DestroyNightmare(i);
                        nightmares.Remove(nm);
                    }
                    else
                    {
                        Vector2 newTarget = mapNodes[pathNodes[pathNodes.IndexOf(nm.NodeIndex) + 1]].Position + nodeCenter;

                        nm.NodeIndex = pathNodes[pathNodes.IndexOf(nm.NodeIndex) + 1];
                        nm.UpdateRotation(newTarget);
                        nm.HasReachedNode = false;
                    }
                }
            }

            //Update player
            player.Update();
        }

        /// <summary>
        /// Updates the player
        /// </summary>
        private void UpdatePlayer()
        {
            // TODO: Implement unlocking abilities after cooldown has been reached
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks the following(in order):
        /// 1)Is a nightmare in range
        /// 2)Has the nightmare travelled farther than the current target
        /// </summary>
        /// <param name="tw"></param>
        /// <returns>True = A target has been selected    False = No target was selected</returns>
        private bool CheckTarget(Tower tw)
        {
            Nightmare nm;
            tw.TargetIndex = NoTarget;

            for (int index = 0; index < nightmares.Count; index++ )
            {
                nm = nightmares[index];
                Vector2 twCenter = tw.Position + nodeCenter;

                // Check that target is within range
                if (Vector2.Distance(twCenter, nm.Position) < tw.Range)
                {
                    // If no target is selected, target first nightmare in range
                    if (tw.TargetIndex == NoTarget)
                    {
                        tw.TargetIndex = index;
                    }
                    else
                    {
                        // What node is this nightmare traveling towards
                        int newTarget = pathNodes.IndexOf(nm.NodeIndex);
                        // What node is the current target nightmare traveling towards
                        int currentTarget = pathNodes.IndexOf(nightmares[tw.TargetIndex].NodeIndex);

                        // If this nightmare has travelled further than the currently targeted nightmare
                        if (newTarget > currentTarget)
                        {
                            tw.TargetIndex = index;
                        }
                    }
                }
            }

            if (tw.TargetIndex == NoTarget)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Creates a new nightmare at the start node of the level
        /// </summary>
        private void CreateNightmare()
        {
            MapNode startNode = mapNodes[pathNodes[0]];
            int targetIndex = pathNodes[1];

            Texture2D nmTexture = textures["NIGHTMARE"];
            float scale = (float)(config.NodeWidth - 15) / nmTexture.Width;
            Vector2 start = startNode.Position + nodeCenter; // TODO: Implement intelligent way of determining starting coordinates
            Vector2 target = mapNodes[targetIndex].Position + nodeCenter;
            nightmares.Add(new Nightmare(start, target, nmTexture, scale, targetIndex, 100));
        }

        /// <summary>
        /// Creates a tower at the selected position
        /// </summary>
        /// <param name="position">Position of the node</param>
        private void CreateTower(Vector2 position)
        {
            float scale = (float)config.NodeWidth / textures["TOWER"].Width;
            Ability attack = new Ability(1000, 10, 600, AbilityType.BASIC, 0);
            towers.Add(new Tower(position, textures["TOWER"], scale, 200, attack));
        }

        /// <summary>
        /// Performs the following:
        /// 1)Removes all projectiles targeting the given nightmare.
        /// 2)All towers targeting this nightmare are set to NoTarget
        /// 3)Adds currency value of the nightmare to the player's wallet
        /// 4)Removes the nightmare from the level
        /// </summary>
        /// <param name="nightmareIndex"></param>
        private void DestroyNightmare(int nightmareIndex)
        {
            foreach (Tower t in towers)
            {
                // All towers targeting this nightmare should reset their target
                if (t.TargetIndex == nightmareIndex)
                {
                    t.TargetIndex = NoTarget;
                }
            }

            // All projectiles targeting this nightmare should be removed
            for (int index = projectiles.Count - 1; index >= 0; index--)
            {
                // If projectile is targeting this nightmare
                if (projectiles[index].TargetIndex == nightmareIndex)
                {
                    projectiles.RemoveAt(index);
                }
                // Projectile index should be updated as a lower index nightmare is being removed
                else if (projectiles[index].TargetIndex > nightmareIndex)
                {
                    projectiles[index].TargetIndex -= 1;
                }
            }

            player.Wallet += nightmares[nightmareIndex].CurrencyValue;
            nightmares.RemoveAt(nightmareIndex);
        }
        
        /// <summary>
        /// Detects if there are any collisions between projectiles and their targets
        /// </summary>
        private void DetectCollisions()
        {
            Nightmare nm;
            Projectile p;

            for (int index = nightmares.Count - 1; index >= 0; index--)
            {
                nm = nightmares[index];

                List<Projectile> pList = GetProjectiles(index);

                for (int pIndex = pList.Count - 1; pIndex >= 0; pIndex--)
                {
                    p = pList[pIndex];

                    // If there's a collision
                    if (nm.BoundingRectangle.Intersects(p.BoundingRectangle))
                    {
                        if (ProcessCollision(nm.Position, p))
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines all the projectiles targeting the given nightmare
        /// </summary>
        /// <param name="nmIndex">Index of the nightmare</param>
        /// <returns>A list containing all projectiles</returns>
        private List<Projectile> GetProjectiles(int nmIndex)
        {
            List<Projectile> pList = new List<Projectile>();

            foreach (Projectile p in projectiles)
            {
                if (p.TargetIndex == nmIndex)
                {
                    pList.Add(p);
                }
            }

            return pList;
        }

        /// <summary>
        /// Deals damage to the nightmare and determines if the damage destroys the nightmare
        /// </summary>
        /// <param name="position">Location the collisions occured</param>
        /// <param name="p">Projectile that caused the damage</param>
        /// <returns>True = Nightmare is destroyed   False = Nightmare is still alive</returns>
        private bool ProcessCollision(Vector2 position, Projectile p)
        {
            // TODO: Implement explosions or other collision effects
            // If the nightmare is dead
            if (nightmares[p.TargetIndex].DealDamage(p.ProjectileAbility))
            {
                DestroyNightmare(p.TargetIndex);
                return true;
            }
            // Projectile has hit. Remove it
            else
            {
                projectiles.Remove(p);
                return false;
            }
        }

        /// <summary>
        /// Processes user input
        /// </summary>
        /// <param name="keyState">Current state of the keyboard</param>
        /// <param name="mouseState">Current state of the mouse</param>
        private void ProcessInput(KeyboardState keyState, MouseState mouseState)
        {
            if (keyState.IsKeyDown(Keys.N) && canPlaceNightmare)
            {
                CreateNightmare();
                canPlaceNightmare = false;
            }
            
            if (keyState.IsKeyUp(Keys.N))
            {
                canPlaceNightmare = true;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (canPlaceTower)
                {
                    bool isPosAllowed = true;
                    Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                    Rectangle mouseRec = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
                    Rectangle nodeRec;
                    foreach (MapNode node in mapNodes)
                    {
                        nodeRec = new Rectangle((int)node.Position.X, (int)node.Position.Y, config.NodeWidth, config.NodeHeight);

                        // Click occurs on this node
                        if (nodeRec.Intersects(mouseRec))
                        {
                            // Node must be buildable
                            if (node.Type == NodeType.BUILDABLE)
                            {
                                // Check if any towers are already built on this node
                                int count = (from t in towers
                                            where t.Position == node.Position
                                            select t).Count();

                                // A tower already exists. Drop this click
                                if (count > 0)
                                {
                                    isPosAllowed = false;
                                }


                                if (isPosAllowed)
                                {
                                    CreateTower(node.Position);
                                }
                            }

                            break;
                        }
                    }

                    canPlaceTower = false;
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                canPlaceTower = true;
            }
        }
        #endregion
    }
}

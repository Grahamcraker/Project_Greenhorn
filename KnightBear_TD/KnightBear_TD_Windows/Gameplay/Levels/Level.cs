//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_Windows.Gameplay.Entities;
using KnightBear_TD_Windows.Gameplay.Entities.Nightmares;
using KnightBear_TD_Windows.Gameplay.Entities.Player;
using KnightBear_TD_Windows.Gameplay.Entities.Towers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows.Gameplay.Levels
{
    /// <summary>
    /// Contains all the configuration needed to run a level
    /// </summary>
    public struct LevelConfig
    {
        /// <summary>
        /// A bi-dimensional array containing the map layout
        /// </summary>
        public int[,] Layout { get; set; }
        public List<string[]> Waypoints { get; set; }
        /// <summary>
        /// Number of nodes the level has vertically.
        /// </summary>
        public int MapHeight
        {
            get { return Layout.GetLength(0); }
        }
        /// <summary>
        /// Number of nodes the level has horizontally.
        /// </summary>
        public int MapWidth
        {
            get { return Layout.GetLength(1); }
        }
        /// <summary>
        /// Starting amount for Player's Wallet.
        /// </summary>
        public int WalletStartAmount { get; set; }
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
        public Dictionary<NodeType, string> NodeTextures { get; set; }

        public LevelConfig(int[,] layout, List<string[]> waypoints, int walletStart, Dictionary<NodeType, string> textures)
            : this()
        {
            Layout = layout;
            Waypoints = waypoints;
            WalletStartAmount = walletStart;
            NodeTextures = textures;
        }
    }

    public class Level
    {
        #region Constants
        /// <summary>
        /// Represents no collision occuring between a projectile and its target
        /// </summary>
        private readonly Vector2 NoCollision = new Vector2(-1, -1);
        #endregion

        #region Fields
        // TODO: Create a more dynamic assignment of textures
        Texture2D nightmareTexture, towerTexture, projectileTexture, playerTexture;
        
        private ContentManager contentManager;
        private Map map;
        private List<Nightmare> nightmares;
        private List<Tower> towers;
        private List<Projectile> projectiles;
        private LevelConfig config;
        private Player player;
        #endregion

        #region Load
        /// <summary>
        /// Primary constructor for Level
        /// </summary>
        /// <param name="service">ServiceProvider for the main game</param>
        /// <param name="config"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public Level(ContentManager content, LevelConfig config)
        {
            this.contentManager = content;

            this.config = config;

            InitLevel();

            player = new Player(playerTexture, map.GetPlayerPosition().Center, config.WalletStartAmount);
        }

        private void InitLevel()
        {
            nightmares = new List<Nightmare>();
            towers = new List<Tower>();
            projectiles = new List<Projectile>();
            map = new Map(config.Layout, LoadTextures(), config.Waypoints);
            // TODO: Enter code to create waypoints
        }
        
        /// <summary>
        /// Loads all the textures needed for the level
        /// </summary>
        private Dictionary<NodeType, Texture2D> LoadTextures()
        {
            Dictionary<NodeType, Texture2D> textures = new Dictionary<NodeType, Texture2D>();

            textures.Add(NodeType.Buildable, contentManager.Load<Texture2D>("images/" + config.NodeTextures[NodeType.Buildable]));
            textures.Add(NodeType.NonBuildable, contentManager.Load<Texture2D>("images/" + config.NodeTextures[NodeType.NonBuildable]));
            textures.Add(NodeType.Path, contentManager.Load<Texture2D>("images/" + config.NodeTextures[NodeType.Path]));

            nightmareTexture = contentManager.Load<Texture2D>("images/lime");
            towerTexture = contentManager.Load<Texture2D>("images/basicTowerIcon");
            projectileTexture = contentManager.Load<Texture2D>("images/laser");
            playerTexture = contentManager.Load<Texture2D>("images/player");

            return textures;
        }
        #endregion

        #region Update
        /// <summary>
        /// Draws all the backgrounds and entities to the screen
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch which will perform all Draw calls</param>
        /// <param name="screenWidth">Width of the screen</param>
        /// <param name="screenHeight">Height of the screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);

            foreach (Tower t in towers)
            {
                t.Draw(spriteBatch);
            }

            foreach (Nightmare n in nightmares)
            {
                n.Draw(spriteBatch);
            }
            
            foreach (Projectile p in projectiles)
            {
                p.Draw(spriteBatch);
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
        public void Update(GameTime gameTime)
        {
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
                // TODO: Enter code to move projectiles
                p.Update(gameTime);
            }

            // Update towers
            foreach (Tower tw in towers)
            {
                tw.Update(gameTime);

                // Tower can attack and a target is within range
                if (tw.CanAttack && CheckTarget(tw))
                {
                    Vector2 startPosition = tw.Center;
                    tw.PerformAttack(gameTime);
                    projectiles.Add(new Projectile(projectileTexture, startPosition, tw.Target, new Ability()));
                }
            }

            //Update Nightmares
            for (int i = nightmares.Count - 1; i >= 0; i--)
            {
                Nightmare nm = nightmares[i];

                if (nm.IsAlive)
                {
                    nm.Update(gameTime);
                }
                else
                {
                    nightmares.Remove(nm);
                }
            }

            //Update player
            player.Update(gameTime);
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
            tw.Target = null;

            for (int i = nightmares.Count - 1; i >= 0; i--)
            {
                nm = nightmares[i];
                Vector2 twCenter = tw.Center;

                // Check that target is within range
                if (Vector2.Distance(twCenter, nm.Center) < tw.Range)
                {
                    // If no target is selected, target first nightmare in range
                    if (tw.Target == null)
                    {
                        tw.Target = nm;
                    }
                    else
                    {
                        if (tw.Target.WaypointCount < nm.WaypointCount)
                        {
                            tw.Target = nm;
                        }
                    }
                }
            }

            if (tw.Target == null)
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
        public void CreateNightmare()
        {
            Queue<Vector2> waypoints = map.GetWaypoints(0);
            Vector2 position = waypoints.Dequeue();
            nightmares.Add(new Nightmare(nightmareTexture, position, 100, 15, 1, waypoints));
        }

        /// <summary>
        /// Creates a tower at the selected position
        /// </summary>
        /// <param name="position">Position of the node</param>
        public void CreateTower(Vector2 position)
        {
            MapNode selectedNode = map.CheckCollision(position);

            if (selectedNode != null)
            {
                if (selectedNode.Type == NodeType.Buildable)
                {
                    int count = (from t in towers
                                 where t.Center == selectedNode.Center
                                 select t).Count();

                    // No towers currently exist on this node
                    if (count < 1)
                    {
                        Ability attack = new Ability(1000, 10, 600, AbilityType.Basic, 0);
                        towers.Add(new Tower(towerTexture, selectedNode.Center, 200, attack));
                    }
                }
            }
        }

        /// <summary>
        /// Performs the following:
        /// 1)Removes all projectiles targeting the given nightmare.
        /// 2)All towers targeting this nightmare are set to NoTarget
        /// 3)Adds currency value of the nightmare to the player's wallet
        /// 4)Removes the nightmare from the level
        /// </summary>
        /// <param name="nightmareIndex"></param>
        private void DestroyNightmare(Nightmare nightmare)
        {
            Projectile p;

            foreach (Tower t in towers)
            {
                // All towers targeting this nightmare should reset their target
                if (t.Target == nightmare)
                {
                    t.Target = null;
                }
            }

            // All projectiles targeting this nightmare should be removed
            for (int index = projectiles.Count - 1; index >= 0; index--)
            {
                p = projectiles[index];

                // If projectile is targeting this nightmare
                if (p.Target == nightmare)
                {
                    projectiles.Remove(p);
                }
            }

            player.Wallet += nightmare.Bounty;
            nightmares.Remove(nightmare);
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

                List<Projectile> pList = GetProjectiles(nm);

                for (int pIndex = pList.Count - 1; pIndex >= 0; pIndex--)
                {
                    p = pList[pIndex];

                    // If there's a collision
                    if (nm.Bounds.Intersects(p.Bounds))
                    {
                        ProcessCollision(nm.Center, p);
                    }
                }
            }
        }

        /// <summary>
        /// Determines all the projectiles targeting the given nightmare
        /// </summary>
        /// <param name="nmIndex">Index of the nightmare</param>
        /// <returns>A list containing all projectiles</returns>
        private List<Projectile> GetProjectiles(Nightmare nm)
        {
            List<Projectile> pList = new List<Projectile>();

            foreach (Projectile p in projectiles)
            {
                if (p.Target == nm)
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
        private void ProcessCollision(Vector2 position, Projectile p)
        {
            // TODO: Implement explosions or other collision effects
            p.Target.DealDamage(p.ProjectileAbility);

            projectiles.Remove(p);
        }
        #endregion
    }
}

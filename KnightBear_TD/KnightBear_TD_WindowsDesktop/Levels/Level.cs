using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnightBear_TD_WindowsDesktop.Entities;
using KnightBear_TD_WindowsDesktop.Entities.Nightmares;
using KnightBear_TD_WindowsDesktop.Entities.Towers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_WindowsDesktop.Levels
{
    struct LevelConfig
    {
        public int NodeHeight { get; set; }
        public int NodeWidth { get; set; }
        public float NodeScale { get; set; }
        public int HorizontalNodeCount { get; set; }
        public int VerticalNodeCount { get; set; }
        public List<int> BuildNodes { get; set; }
        public List<int> PathNodes { get; set; }
        public List<Tower> Towers { get; set; }
        public Dictionary<string, string> Textures { get; set; }

        public LevelConfig(int hCount, int vCount, List<int> build, List<int> path, Dictionary<string, string> textures)
            : this()
        {
            HorizontalNodeCount = hCount;
            VerticalNodeCount = vCount;
            BuildNodes = build;
            PathNodes = path;
            Textures = textures;
        }
    }

    class Level
    {
        #region Constants
        private readonly Vector2 NoCollision = new Vector2(-1, -1);
        private readonly int NoTarget = -1;
        #endregion

        #region Member Variables
        private bool canPlaceTower;
        private ContentManager content;
        private Dictionary<string, Texture2D> textures;
        private List<MapNode> mapNodes;
        private List<Nightmare> nightmares;
        private List<Tower> towers;
        private List<Projectile> projectiles;
        private LevelConfig config;
        private Player player;
        private Vector2 nodeCenter;
        #endregion

        #region Load
        public Level(IServiceProvider service, LevelConfig config, int screenWidth, int screenHeight)
        {
            player = new Player();
            mapNodes = new List<MapNode>();
            nightmares = new List<Nightmare>();
            towers = new List<Tower>();
            projectiles = new List<Projectile>();
            textures = new Dictionary<string,Texture2D>();
            content = new ContentManager(service, "../../../../../Content");
            this.config = config;
            this.config.NodeWidth = screenWidth / config.HorizontalNodeCount;
            this.config.NodeHeight = screenHeight / config.VerticalNodeCount;
            nodeCenter = new Vector2(this.config.NodeWidth / 2, this.config.NodeHeight / 2);
            LoadTextures();
            InitNodes();
            InitNightmares();
        }

        private void InitNightmares()
        {
            // Populate with data from configuration
            /*MapNode start = mapNodes[config.PathNodes[0]];
            float startX = start.Position.X + config.NodeWidth / 2;
            float startY = start.Position.Y + config.NodeHeight / 2;
            nightmares.Add(new Nightmare(new Vector2(startX, startY), textures["NIGHTMARE"]));*/
        }

        private void InitNodes()
        {
            int nodeCount = config.HorizontalNodeCount * config.VerticalNodeCount;
            for (var counter = 0; counter < nodeCount; counter++)
            {
                mapNodes.Add(new MapNode(NodeType.NONBUILDABLE, textures["NONBUILD"]));
            }

            foreach (var id in config.BuildNodes)
            {
                mapNodes[id].Type = NodeType.BUILDABLE;
                mapNodes[id].NodeTexture = textures["BUILD"];
            }

            foreach (var id in config.PathNodes)
            {
                mapNodes[id].Type = NodeType.NIGHTMAREPATH;
                mapNodes[id].NodeTexture = textures["PATH"];
            }

            MapNode currentNode;
            int index = 0;

            for (var v = 0; v < config.VerticalNodeCount; v++)
            {
                for (var h = 0; h < config.HorizontalNodeCount; h++)
                {
                    currentNode = mapNodes[index];
                    currentNode.Position = new Vector2(h * config.NodeWidth, v * config.NodeHeight);
                    currentNode.Origin = new Vector2(currentNode.NodeTexture.Width / 2, currentNode.NodeTexture.Height / 2);
                    currentNode.Scale = (float)config.NodeWidth / currentNode.NodeTexture.Width;
                    index++;
                }
            }
        }

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
        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            //spriteBatch.Draw(textures["BACKGROUND"], new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            foreach (MapNode node in mapNodes)
            {
                spriteBatch.Draw( node.NodeTexture
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
                spriteBatch.Draw(n.EntityTexture, n.Position, null, Color.White, n.Angle, n.Origin, n.Scale, SpriteEffects.None, 0);
            }

            foreach (Tower t in towers)
            {
                spriteBatch.Draw(t.EntityTexture, t.Position, null, Color.White, t.Angle, t.Origin, t.Scale, SpriteEffects.None, 0);
            }

            foreach (Projectile p in projectiles)
            {
                spriteBatch.Draw(p.EntityTexture, p.Position, null, Color.White, p.Angle, p.Origin, p.Scale, SpriteEffects.None, 0);
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyState, MouseState mouseState, int screenWidth, int screenHeight)
        {
            config.NodeWidth = screenWidth / config.HorizontalNodeCount;
            config.NodeHeight = screenHeight / config.VerticalNodeCount;
            ProcessInput(keyState, mouseState);
            UpdateEntities(gameTime);
            UpdatePlayer();
            CheckCollisions();
        }

        private void UpdateEntities(GameTime gameTime)
        {
            // Update projectiles
            foreach (Projectile p in projectiles)
            {
                if (p.CanMove)
                {
                    p.Update(gameTime, nightmares[p.TargetIndex].Position);
                    p.CanMove = false;
                }
            }

            // Update towers
            foreach (Tower tw in towers)
            {
                tw.Update(gameTime);

                // Tower can attack and a target is within range
                if (tw.CanAttack && CheckTarget(tw))
                {
                    tw.PerformAttack(gameTime);
                    projectiles.Add(new Projectile(textures["PROJECTILE"], tw.Position, 1.0f, tw.TargetIndex, tw.Attack));
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
                    if (nm.NodeIndex != config.PathNodes[config.PathNodes.Count - 1])
                    {
                        Vector2 newTarget = mapNodes[config.PathNodes[config.PathNodes.IndexOf(nm.NodeIndex) + 1]].Position + nodeCenter;

                        nm.NodeIndex = config.PathNodes[config.PathNodes.IndexOf(nm.NodeIndex) + 1];
                        nm.UpdateRotation(newTarget);
                        nm.HasReachedNode = false;
                    }
                    else
                    {
                        nightmares.Remove(nm);
                    }
                }
            }

            player.Update();
        }

        private void UpdatePlayer()
        {
            // TODO: Implement unlocking abilities after cooldown has been reached
        }
        #endregion

        #region Methods
        private void CheckCollisions()
        {
            Vector2 collisionPosition;
            Nightmare nm;
            Projectile p;
            for (int index = projectiles.Count -1; index >=0; index--)
            {
                p = projectiles[index];
                nm = nightmares[p.TargetIndex];
                Color[,] pTexture = TextureToArray(p.EntityTexture);
                Color[,] nTexture = TextureToArray(nm.EntityTexture);

                Matrix pMatrix =
                    Matrix.CreateTranslation(p.Origin.X, p.Origin.Y, 0) *
                    Matrix.CreateScale(p.Scale) *
                    Matrix.CreateRotationZ(p.Angle) *
                    Matrix.CreateTranslation(p.Position.X, p.Position.Y, 0);

                Matrix nMatrix =
                    Matrix.CreateTranslation(nm.Origin.X, nm.Origin.Y, 0) *
                    Matrix.CreateRotationZ(nm.Angle) *
                    Matrix.CreateScale(nm.Scale) *
                    Matrix.CreateTranslation(nm.Position.X, nm.Position.Y, 0);

                collisionPosition = GetCollision(pTexture, pMatrix, nTexture, nMatrix);

                if (collisionPosition != NoCollision)
                {
                    ProcessCollision(collisionPosition, p);
                }
            }
        }

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
            bool isTargetSelected = false;

            for (int index = 0; index < nightmares.Count; index++)
            {
                nm = nightmares[index];

                // The target is within range
                if (Vector2.Distance(tw.Position, nm.Position) < tw.Range)
                {
                    if (tw.TargetIndex == NoTarget)
                    {
                        tw.TargetIndex = index;
                        isTargetSelected = true;
                    }
                    // Further down the path than the current target
                    else
                    {
                        int newTarget = config.PathNodes.IndexOf(nm.NodeIndex);
                        int currentTarget = config.PathNodes.IndexOf(nightmares[tw.TargetIndex].NodeIndex);
                        if (newTarget > currentTarget)
                        {
                            tw.TargetIndex = index;
                            isTargetSelected = true;
                        }
                    }
                }
            }

            return isTargetSelected;
        }

        private void CreateNightmare()
        {
            MapNode startNode = mapNodes[config.PathNodes[0]];
            int targetIndex = config.PathNodes[1];

            Texture2D nmTexture = textures["NIGHTMARE"];
            float scale = (float)(config.NodeWidth - 15) / nmTexture.Width;
            Vector2 start = startNode.Position + nodeCenter; // TODO: Implement intelligent way of determining starting coordinates
            Vector2 target = mapNodes[targetIndex].Position + nodeCenter;
            nightmares.Add(new Nightmare(start, target, nmTexture, scale, targetIndex, 100));
        }

        private void CreateTower(Vector2 position)
        {
            float scale = (float)config.NodeWidth / textures["TOWER"].Width;
            Ability attack = new Ability(60000, 10, 10000, AbilityType.BASIC, 0);
            towers.Add(new Tower(position, textures["TOWER"], scale, 200, attack));
        }

        /// <summary>
        /// Checks if a collision occurs between a projectile and a nightmare
        /// </summary>
        /// <param name="pTexture">Projectile Texture</param>
        /// <param name="pMatrix">Projectile Matrix</param>
        /// <param name="nTexture">Nightmare Texture</param>
        /// <param name="nMatrix">Nightmare Matrix</param>
        /// <returns>Coordinate of the collision.  (-1,-1) if no collision detected</returns>
        private Vector2 GetCollision(Color[,] pTexture, Matrix pMatrix, Color[,] nTexture, Matrix nMatrix)
        {
            // Allows us to transform a pixel location between textures
            Matrix transformMatrix = pMatrix * Matrix.Invert(nMatrix);
            int pWidth = pTexture.GetLength(0);
            int pHeight = pTexture.GetLength(1);
            int nWidth = nTexture.GetLength(0);
            int nHeight = nTexture.GetLength(1);

            // Cycle through all the pixels of the projectile
            for (int x = 0; x < pWidth; x++)
            {
                for (int y = 0; y < pHeight; y++)
                {
                    Vector2 pPosition = new Vector2(x, y);
                    Vector2 nPosition = Vector2.Transform(pPosition, transformMatrix);

                    int x2 = (int)nPosition.X;
                    int y2 = (int)nPosition.Y;
                    // Check if nightmare coordinate doesn't collide at all
                    if ((x2 >= 0) && (x2 < nWidth))
                    {
                        if ((y2 >= 0) && (y2 < nHeight))
                        {
                            if (pTexture[x, y].A > 0)
                            {
                                if (nTexture[x2, y2].A > 0)
                                {
                                    return Vector2.Transform(pPosition, pMatrix);
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        private void ProcessCollision(Vector2 position, Projectile p)
        {
            // TODO: Implement explosions or other collision effects
            // If the nightmare is dead
            if (nightmares[p.TargetIndex].DealDamage(p.ProjectileAbility))
            {
                player.Wallet += nightmares[p.TargetIndex].CurrencyValue;
                foreach (Tower t in towers)
                {
                    // All towers targeting this nightmare should reset their target
                    if (t.TargetIndex == p.TargetIndex)
                    {
                        t.TargetIndex = NoTarget;
                    }
                }

                // All projectiles targeting this nightmare should disappear
                for (int index = projectiles.Count - 1; index >= 0; index++)
                {
                    if (projectiles[index].TargetIndex == p.TargetIndex)
                    {
                        projectiles.RemoveAt(index);
                    }
                }

                nightmares.RemoveAt(p.TargetIndex);
            }
            // Projectile has hit. Remove it
            else
            {
                projectiles.Remove(p);
            }
        }

        private void ProcessInput(KeyboardState keyState, MouseState mouseState)
        {
            if (keyState.IsKeyDown(Keys.N))
            {
                CreateNightmare();
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
                        if (nodeRec.Intersects(mouseRec))
                        {
                            foreach (Tower t in towers)
                            {
                                // If a tower already exists on that node
                                if (t.Position == node.Position)
                                {
                                    isPosAllowed = false;
                                    break;
                                }
                            }

                            if (isPosAllowed)
                            {
                                CreateTower(node.Position);
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

        /// <summary>
        /// Converts a 2D Texture to a 2-dimensional array
        /// </summary>
        /// <param name="texture">Texture to be converted</param>
        /// <returns>2D array of colors</returns>
        private Color[,] TextureToArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            Color[,] colors2D = new Color[texture.Width, texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * texture.Width];
                }
            }

            return colors2D;
        }
        #endregion
    }
}

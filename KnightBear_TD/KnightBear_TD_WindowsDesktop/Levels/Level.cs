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
        public int NodeWidth { get; set; }
        public int NodeHeight { get; set; }
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
        private readonly float NoTarget = -1;
        #endregion

        #region Member Variables
        private bool canPlaceTower;
        private ContentManager content;
        private List<MapNode> mapNodes;
        private List<Nightmare> nightmares;
        private List<Tower> towers;
        private List<Projectile> projectiles;
        private LevelConfig config;
        private Player player;
        private Dictionary<string, Texture2D> textures;
        #endregion

        #region Load
        public Level(IServiceProvider service, LevelConfig config)
        {
            player = new Player();
            mapNodes = new List<MapNode>();
            nightmares = new List<Nightmare>();
            towers = new List<Tower>();
            projectiles = new List<Projectile>();
            textures = new Dictionary<string,Texture2D>();
            content = new ContentManager(service, "../../../../../Content");
            this.config = config;
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

            foreach (Projectile p in projectiles)
            {
                spriteBatch.Draw(p.EntityTexture, p.Position, null, Color.White, p.Angle, p.Origin, p.Scale, SpriteEffects.None, 0);
            }

            foreach (Tower t in towers)
            {
                spriteBatch.Draw(t.EntityTexture, t.Position, null, Color.White, t.Angle, t.Origin, t.Scale, SpriteEffects.None, 0);
            }

            foreach (Nightmare n in nightmares)
            {
                spriteBatch.Draw(n.EntityTexture, n.Position, null, Color.White, n.Angle, n.Origin, n.Scale, SpriteEffects.None, 0);
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
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
                p.Update(nightmares[p.TargetIndex].Position);
            }

            // Update towers
            foreach (Tower tw in towers)
            {
                tw.Update(gameTime);

                // Tower has a target
                if (tw.TargetIndex != NoTarget)
                {
                    // Tower can attack and target is within range
                    if (tw.CanAttack && CheckTarget(tw))
                    {
                        tw.PerformAttack(gameTime);
                        projectiles.Add(new Projectile(textures["PROJECTILE"], tw.Position, .02f));
                    }
                }
            }

            //Update Nightmares
            foreach (Nightmare nm in nightmares)
            {
                nm.Update(gameTime, mapNodes[nm.NodeIndex]);

                if (nm.HasReachedNode)
                {
                    // Check if the nightmare has reached the final node
                    if (nm.NodeIndex != config.PathNodes[config.PathNodes.Count - 1])
                    {
                        nm.NodeIndex = config.PathNodes[config.PathNodes.IndexOf(nm.NodeIndex) + 1];
                        nm.HasReachedNode = false;
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
            foreach (Projectile p in projectiles)
            {
                Nightmare nm = nightmares[p.TargetIndex];
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
        /// 1)Is the nightmare in range
        /// 2)Has the nightmare travelled farther than the current target
        /// </summary>
        /// <param name="tw"></param>
        /// <returns>True = A target has been selected    False = No target was selected</returns>
        private bool CheckTarget(Tower tw)
        {
            Nightmare nm;
            int targetNode = -1;
            bool isTargetSelected = false;
            for (int index = 0; index < nightmares.Count; index++)
            {
                nm = nightmares[index];

                // The target is within range
                if (Vector2.Distance(tw.Position, nm.Position) < tw.Range)
                {
                    // Further down the path than the current target
                    if (nm.NodeIndex > targetNode)
                    {
                        tw.TargetIndex = index;
                        isTargetSelected = true;
                    }
                }
            }

            return isTargetSelected;
        }

        private void CreateTower(Vector2 position)
        {
            float scale = textures["TOWER"].Width / config.NodeWidth;
            towers.Add(new Tower(position, textures["TOWER"], scale));
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
            if (nightmares[p.TargetIndex].DealDamage(p.ProjectileAbility))
            {
                player.Wallet += nightmares[p.TargetIndex].CurrencyValue;
                nightmares.RemoveAt(p.TargetIndex);
            }

            projectiles.Remove(p);
        }

        private void ProcessInput(KeyboardState keyState, MouseState mouseState)
        {
            if (keyState.IsKeyDown(Keys.N))
            {
                float scale = textures["NIGHTMARE"].Width / (config.NodeWidth + 10);
                MapNode node = mapNodes[config.PathNodes[0]];
                Vector2 start = new Vector2(node.Position.X + config.NodeWidth / 2, node.Position.Y + config.NodeHeight / 2);
                nightmares.Add(new Nightmare(start, textures["NIGHTMARE"], scale));
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                bool isPosAllowed = true;
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                Rectangle mouseRec = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
                Rectangle nodeRec;
                foreach(MapNode node in mapNodes)
                {
                    nodeRec = new Rectangle((int)node.Position.X, (int)node.Position.Y, config.NodeWidth, config.NodeHeight);
                    if (nodeRec.Intersects(mouseRec))
                    {
                        foreach (Tower t in towers)
                        {
                            // If a tower already exists on that node
                            if (t.Position == mousePos)
                            {
                                isPosAllowed = false;
                                break;
                            }
                        }

                        if (isPosAllowed)
                        {
                            CreateTower(mousePos);
                        }
                    }
                }

                canPlaceTower = false;
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

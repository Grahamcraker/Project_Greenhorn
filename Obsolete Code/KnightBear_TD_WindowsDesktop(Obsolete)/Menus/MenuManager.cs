using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_WindowsDesktop.Menus
{
    /// <summary>
    /// This class manages all menus for the game. This is a singleton.
    /// </summary>
    public class MenuManager
    {
        #region Member Variables
        private static MenuManager manager;

        private Dictionary<string, Menu> Menus;
        private Menu activeMenu;
        private Stack<Menu> previousMenus;
        #endregion

        #region Properties
        public Menu ActiveMenu
        {
            get { return activeMenu; }
        }

        /// <summary>
        /// Returns the singleton instance of the MenuManager class
        /// </summary>
        public static MenuManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = new MenuManager();
                }

                return manager;
            }
        }
        #endregion

        private MenuManager()
        {
            Menus = new Dictionary<string, Menu>();
            previousMenus = new Stack<Menu>();
        }

        public bool IsActive { get; set; }

        /// <summary>
        /// Updates the active menu
        /// </summary>
        public void Update(KeyboardState keyState, MouseState mouse, float scale)
        {
            activeMenu.Update(keyState, mouse, scale);
        }

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            activeMenu.Draw(spriteBatch);
        }

        public void AddMenu(string name, Menu menu)
        {
            Menus.Add(name, menu);

            activeMenu = menu;
        }

        public void GoToMenu(string menuName)
        {
            previousMenus.Push(Menus[menuName]);
        }

        /// <summary>
        /// Closes the current menu and returns to the previous menu.
        /// </summary>
        public void Close()
        {
            if (previousMenus.Count > 0)
            {
                activeMenu = previousMenus.Pop();
            }
            else
            {
                Exit();
            }
        }

        /// <summary>
        /// Closes all menus.
        /// </summary>
        public void Exit()
        {
            activeMenu = null;
            IsActive = false;
        }
    }
}

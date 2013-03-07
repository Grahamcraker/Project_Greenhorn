using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KnightBear_TD_WindowsDesktop.Menus
{
    public class Menu
    {
        bool canClick;
        List<Button> buttons;

        public List<Button> Buttons { get; set; }

        #region Load/Update
        public Menu()
        {
            buttons = new List<Button>();
        }

        public void Update(KeyboardState keyState, MouseState mouse, float scale)
        {
            ProcessInput(keyState, mouse);

            foreach (Button btn in buttons)
            {
                btn.Update(scale);

                if (btn.IsClicked)
                {
                    ProcessClick(buttons.IndexOf(btn));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button btn in buttons)
            {
                btn.Draw(spriteBatch);
            }
        }
        #endregion

        #region Methods
        protected void AddButton(Button btn)
        {
            buttons.Add(btn);
        }

        private void ProcessInput(KeyboardState keyState, MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed && canClick)
            {
                foreach (Button btn in buttons)
                {
                    Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);
                    if (mouseRec.Intersects(btn.Bounds))
                    {
                        btn.IsClicked = true;
                        break;
                    }
                }

                canClick = false;
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                canClick = true;

                foreach (Button btn in buttons)
                {
                    btn.IsClicked = false;
                }
            }
        }

        public virtual void ProcessClick(int index) { }
        #endregion
    }
}

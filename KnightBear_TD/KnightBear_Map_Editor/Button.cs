using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_Map_Editor
{
    abstract class Button
    {
        public Vector2 Position { get; set; }
        public Texture2D Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public static List<Button> Buttons;

        public Button()
        {
            Buttons = new List<Button>();
        }

        /// <summary>
        /// Used to handle the button being clicked
        /// </summary>
        public abstract void OnClick();

        /// <summary>
        /// Returns the index of the clicked button
        /// </summary>
        /// <param name="location">Coordinates of point being clicked</param>
        /// <returns>0-based index or -1 if no button was clicked</returns>
        public static int GetButtonIndex(Vector2 location)
        {
            Rectangle rec = new Rectangle();
            foreach (var btn in Buttons)
            {
                rec = new Rectangle((int)btn.Position.X, (int)btn.Position.Y, btn.Width, btn.Height);

                if (rec.Contains((int)location.X, (int)location.Y))
                {
                    return Buttons.IndexOf(btn);
                }
            }

            return -1;
        }
    }
}

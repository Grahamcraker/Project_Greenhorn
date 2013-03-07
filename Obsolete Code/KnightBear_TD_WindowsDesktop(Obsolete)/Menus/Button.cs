using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Menus
{
    public class Button : GameObject
    {
        public bool IsClicked { get; set; }

        private Texture2D clickedTexture;
        private Texture2D nonClickedTexture;

        public Button(Texture2D nonClickedTexture, Texture2D clickedTexture, Vector2 position)
        {
            this.nonClickedTexture = nonClickedTexture;
            this.clickedTexture = clickedTexture;
            ObjectTexture = nonClickedTexture;
            Position = position;
        }

        public override void Update(float scale)
        {
            base.Update(scale);

            if (IsClicked)
            {
                ObjectTexture = clickedTexture;
            }
            else
            {
                ObjectTexture = nonClickedTexture;
            }
        }
    }
}

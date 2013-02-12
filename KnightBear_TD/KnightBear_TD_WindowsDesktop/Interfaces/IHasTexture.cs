using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KnightBear_TD_WindowsDesktop.Interfaces
{
    interface IHasTexture
    {
        /// <summary>
        /// The angle(Radians) at which the texture is drawn.
        /// </summary>
        float Angle { get; set; }
        /// <summary>
        /// The scaling variable used to adjust texture to a proper size
        /// </summary>
        float Scale { get; set; }
        /// <summary>
        /// Width of the rectangle the texture is drawn to
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Height of the rectangle the texture is drawn to
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// The direction the texture will move, if it moves.
        /// </summary>
        Vector2 Direction { get; set; }
        /// <summary>
        /// The direction the texture is facing by default.
        /// Examples:
        /// An "Up Arrow" texture would have (0,-1).
        /// A "Down Arrow" texture would have (0,1). 
        /// A "Right Arrow" texture would have (1,0).
        /// A "Left Arrow" texture would have (-1,0).
        /// </summary>
        Vector2 Forward { get; set; }
        /// <summary>
        /// The center point of the texture
        /// </summary>
        Vector2 Origin { get; set; }
        /// <summary>
        /// The current (x,y) coordinates of the texture
        /// </summary>
        Vector2 Position { get; set; }
    }
}

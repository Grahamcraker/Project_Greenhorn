using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows
{
    /// <summary>
    /// This represents the current state of the user inputs.
    /// </summary>
    public class InputState
    {
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;

        MouseState currentMouseState;
        MouseState previousMouseState;

        /// <summary>
        /// Constructor. Initializes input states
        /// </summary>
        public InputState()
        {
            currentKeyState = new KeyboardState();
            previousKeyState = new KeyboardState();
            currentMouseState = new MouseState();
            previousMouseState = new MouseState();
        }

        /// <summary>
        /// Gets the current state of inputs
        /// </summary>
        public void Update()
        {
            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;

            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            // Use the following line to implement touch pad functionality
            // TouchCollection touchState = TouchPanel.GetState();
        }

        /// <summary>
        /// Returns the current position of the mouse
        /// </summary>
        public Vector2 GetMousePosition()
        {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        /// <summary>
        /// Determines if the given key is pressed. This will not check if the key
        /// press is unique.
        /// </summary>
        /// <param name="key">Which key to check</param>
        /// <returns>True = Key is pressed. False = Key is not pressed.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines if the given key is pressed. This will check if the key
        /// press is unique.
        /// </summary>
        /// <param name="key">Which key to check</param>
        /// <returns>
        /// True = Key is pressed.
        /// False = Key is not pressed or isn't a unique press.
        /// </returns>
        public bool IsNewKeyPress(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key);
        }

        /// <summary>
        /// Determines if the left mouse button is clicked. This will not check if
        /// the button press is unique.
        /// </summary>
        /// <returns>True = Button is pressed. False = Button is not pressed.</returns>
        public bool IsLeftMouseClicked()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Determines if the left mouse button is clicked. This will check if
        /// the button press is unique.
        /// </summary>
        /// <returns>
        /// True = Button is pressed and unique.
        /// False = Button is not pressed or isn't a unique press.
        /// </returns>
        public bool IsNewLeftMouseClick()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }
    }
}

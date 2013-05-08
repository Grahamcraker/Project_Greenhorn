//-----------------------------------------------------------------------
// <copyright file="InputAction.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace KnightBear_TD_Windows.Screens
{
    class InputAction
    {
        /// <summary>
        /// An array containing all the keys that cause this action to occur
        /// </summary>
        private readonly Keys[] keys;
        private readonly bool isNewPressOnly;

        public InputAction(Keys[] keys, bool isNewPressOnly)
        {
            this.keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];
            this.isNewPressOnly = isNewPressOnly;
        }

        public bool Evaluate(InputState state)
        {
            foreach (Keys key in keys)
            {
                if (isNewPressOnly)
                {
                    if (state.IsNewKeyPress(key))
                    {
                        return true;
                    }
                }
                else
                {
                    if (state.IsKeyPressed(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

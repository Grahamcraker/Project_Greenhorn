//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="Leim Productions">
//     Copyright (c) Leim Productions Inc.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace KnightBear_TD_WindowsDesktop
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static KnightBearGame game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new KnightBearGame();
            game.Run();
        }
    }
}

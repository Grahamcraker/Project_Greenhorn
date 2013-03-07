#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace KnightBear_TD_Windows
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

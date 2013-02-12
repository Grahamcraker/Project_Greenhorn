#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace KnightBear_Map_Editor
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static MapEditor game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new MapEditor();
            game.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class OptionsMenuScreen : BaseMenuScreen
    {
        public OptionsMenuScreen()
            : base("Options")
        {
            MenuItem cancelMenuItem = new MenuItem("Cancel");

            // Hook up menu items to selection event
            // TODO: Create menu items for the options menu
            cancelMenuItem.Selected += cancelMenuItem_Selected;

            MenuItems.Add(cancelMenuItem);
        }

        /// <summary>
        /// Method for handling when Cancel is selected
        /// </summary>
        void cancelMenuItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }
    }
}

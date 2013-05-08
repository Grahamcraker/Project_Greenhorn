using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnightBear_TD_Windows.Screens.Menus
{
    class MapEditorMenuScreen : BaseMenuScreen
    {
        public MapEditorMenuScreen()
            : base("Map Options")
        {
            // Add Menu Items
            MenuItem saveMenuItem = new MenuItem("Save Map");
            MenuItem resumeMenuItem = new MenuItem("Resume");
            MenuItem exitMenuItem = new MenuItem("Exit To Main Menu");

            // Hook up events
            saveMenuItem.Selected += saveMenuItem_Selected;
            resumeMenuItem.Selected += resumeMenuItem_Selected;
            exitMenuItem.Selected += exitMenuItem_Selected;

            // Add menu Items
            MenuItems.Add(saveMenuItem);
            MenuItems.Add(resumeMenuItem);
            MenuItems.Add(exitMenuItem);
        }

        void saveMenuItem_Selected(object sender, EventArgs e)
        {
            // TODO: Enter code to check that the path is valid

        }

        void resumeMenuItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }

        void exitMenuItem_Selected(object sender, EventArgs e)
        {
            BaseScreen[] screens = Manager.GetScreens();

            foreach (BaseScreen s in screens)
            {
                Manager.RemoveScreen(s);
            }

            Manager.AddScreen(new BackgroundScreen());
            Manager.AddScreen(new MainMenuScreen());
        }
    }
}

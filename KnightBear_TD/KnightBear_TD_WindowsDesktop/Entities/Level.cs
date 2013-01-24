using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KnightBear_TD_WindowsDesktop.Entities
{
    class Level
    {
        public List<Vector2> NightmarePath { get; private set; }

        public Level(int level)
        {
            NightmarePath = new List<Vector2>();

            switch (level)
            {
                case 1:
                    PrepareLevel1();
                    break;
            }
        }

        private void PrepareLevel1()
        {
            Vector2 START = new Vector2(0, 350);
            Vector2 END = new Vector2(250, 0);
            Vector2 current = START;
            while(current.Y != END.Y)
            {
                Console.WriteLine(current.ToString());
                NightmarePath.Add(current);

                if (current.X < 250)
                    current.X += 5;
                else
                    current.Y -= 5;
            }
        }
    }
}

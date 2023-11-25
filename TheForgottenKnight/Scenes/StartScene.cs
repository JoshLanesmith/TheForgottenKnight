using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.Scenes
{
    public class StartScene : GameScene
    {
        private MenuComponent menu;

        public StartScene(Game game) : base(game)
        {
            string[] menuItems = { "Start Game", "Help", "High Score", "Credit", "Quit" };

            Menu = new MenuComponent(game, menuItems);
            Components.Add(Menu);
        }

        public MenuComponent Menu { get => menu; set => menu = value; }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
            Game1 g = (Game1)game;
         
            string[] menuItems = { "Start Game", "Help", "High Score", "Credit", "Quit" };
            
            ClickableString.OnClick[] menuActions =
            {
                () => { g.GoToActionScene(); },
                () => { g.GoToHelpScene(); },
                () => { g.GoToHighscoreScene(); },
                () => { },
                () => { g.ExitGame(); }
            };

            Menu = new MenuComponent(game, this, menuItems, menuActions);
           
            Components.Add(Menu);
		}

        public MenuComponent Menu { get => menu; set => menu = value; }

        public override void Draw(GameTime gameTime)
        {

            Shared.sb.Begin();
            Shared.sb.Draw(Shared.menuBgImage, Shared.displayPosShift, new Rectangle(0, 0, Shared.menuBgImage.Width, Shared.menuBgImage.Height),
                Color.White, 0.0f, Vector2.Zero, Shared.gameDisplaySize.X / Shared.menuBgImage.Width, SpriteEffects.None, 0);
            Shared.sb.End();
            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.Scenes
{
    public class HelpScene : GameScene
    {
        private Texture2D tex;

        public HelpScene(Game game) : base(game)
        {
            tex = game.Content.Load<Texture2D>("images/HelpScene2");
        }

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();

            Shared.sb.Draw(tex, Vector2.Zero, Color.White);

            Shared.sb.End();

            base.Draw(gameTime);
        }

    }
}

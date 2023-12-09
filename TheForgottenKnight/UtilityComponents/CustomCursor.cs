using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForgottenKnight.Scenes;

namespace TheForgottenKnight
{
    public class CustomCursor : DrawableGameComponent
    {
        private Vector2 mousePoint;
        private Texture2D tex;

        public CustomCursor(Game game) : base(game)
        {
            tex = game.Content.Load<Texture2D>("images/FantasyCursor");
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            Point mousePoint = new Point(ms.X, ms.Y);

            this.mousePoint = new Vector2(mousePoint.X, mousePoint.Y);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();
            Shared.sb.Draw(tex, mousePoint, new Rectangle(0, 0, tex.Width, tex.Height), Color.White,
                0.0f, Vector2.Zero, 64f / (float)tex.Height, SpriteEffects.None, 0);
            Shared.sb.End();
            base.Draw(gameTime);
        }
    }
}

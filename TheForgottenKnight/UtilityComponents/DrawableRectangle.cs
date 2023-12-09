using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
    public class DrawableRectangle : DrawableGameComponent
    {
        private Texture2D tex;
        private int width;
        private int height;
        private Vector2 position;
        private Color color;
        private Color[] transparentColor;
        private Byte transparencyAmount;
        private Rectangle panel;


        public DrawableRectangle(Game game, int width, int height, Vector2 position, Color color, byte transparencyAmount) : base(game)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            this.color = color;
            this.transparentColor = new Color[1];
            this.transparencyAmount = transparencyAmount;

            tex = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            this.transparentColor[0] = Color.FromNonPremultiplied(255, 255, 255 , transparencyAmount);
            tex.SetData<Color>(this.transparentColor);

            panel = new Rectangle((int)position.X, (int)position.Y, width, height);
            panel.Inflate(15, 15); //pad by 5 pixels
        }

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();
            Shared.sb.Draw(tex, panel, color);
            Shared.sb.End();
            base.Draw(gameTime);
        }
    }
}

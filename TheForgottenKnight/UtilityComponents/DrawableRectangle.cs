/* DrawableRectangle.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.12.05: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheForgottenKnight
{
    /// <summary>
    /// Drawable rectangle to draw background panels
    /// </summary>
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

        /// <summary>
        /// Create a rectangle with a transparency factor
        /// </summary>
        /// <param name="game">The game context for the ButtonComponent</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="position">Position of the top left corner</param>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="transparencyAmount">Transparency of the rectangle</param>
        public DrawableRectangle(Game game, int width, int height, Vector2 position, Color color, byte transparencyAmount) : base(game)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            this.color = color;
            this.transparentColor = new Color[1];
            this.transparencyAmount = transparencyAmount;

            tex = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            // Apply the transparency for the color
            this.transparentColor[0] = Color.FromNonPremultiplied(255, 255, 255 , transparencyAmount);
            tex.SetData<Color>(this.transparentColor);

            panel = new Rectangle((int)position.X, (int)position.Y, width, height);

            // Pad the rectangle by 15 pixels
            panel.Inflate(15, 15);
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

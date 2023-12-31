﻿/* Paralax.cs
 * The Forgotten Knight
 *	Revision History
 *			Miles Purvis, 2023.10.08: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheForgottenKnight
{
    /// <summary>
    /// Parallax component handles moveable background 
    /// </summary>
    public class Parallax : DrawableGameComponent
	{
		private Texture2D tex;
		private Rectangle srcRec;
		private Vector2 speed;
		private Vector2 milesPosition;
		private Vector2 joshPosition;
		private Vector2 imagePosition;
		private Vector2 titlePosition;
		private Vector2 pos1, pos2;

		/// <summary>
		/// Initializes a new instance of the Parallax class.
		/// </summary>
		/// <param name="game">The Game object associated with this component.</param>
		/// <param name="tex">The texture used for the parallax effect.</param>
		/// <param name="srcRec">The source rectangle defining the portion of the texture to draw.</param>
		/// <param name="speed">The speed at which the background moves.</param>
		/// <param name="pos">The initial position of the background images.</param>
		public Parallax(Game game, Texture2D tex, Rectangle srcRec, Vector2 speed, Vector2 pos) : base(game)
		{

			this.tex = tex;
			this.srcRec = srcRec;
			this.speed = speed;

			this.pos1 = pos;
			this.pos2 = new Vector2(pos.X + srcRec.Width, pos.Y);
			float screenWidth = GraphicsDevice.Viewport.Width;
			float screenHeight = GraphicsDevice.Viewport.Height;

			imagePosition = new Vector2((screenWidth - Shared.scrollPnlImageSmall.Width) / 2, (screenHeight - Shared.scrollPnlImageSmall.Height) / 2);
			milesPosition = new Vector2(imagePosition.X + 200, imagePosition.Y + 55);
			joshPosition = new Vector2(imagePosition.X + 200, imagePosition.Y + 80);
			titlePosition = new Vector2(imagePosition.X + 200, imagePosition.Y + 10);
		}

		/// <summary>
		/// Draws the parallax background and additional text.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		/// /// <summary>
		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			Shared.sb.Draw(tex, pos1, srcRec, Color.White);
			Shared.sb.Draw(tex, pos2, srcRec, Color.White);
			Shared.sb.Draw(Shared.scrollPnlImageSmall, imagePosition, Color.White);
			Shared.sb.DrawString(Shared.highlightFont, "Created By: ", titlePosition, Color.Black);
			Shared.sb.DrawString(Shared.highlightFont, "Miles Purvis", milesPosition, Color.Black);
			Shared.sb.DrawString(Shared.highlightFont, "Josh Lanesmith", joshPosition, Color.Black);
			Shared.sb.End();
			base.Draw(gameTime);
		}

		
		/// Updates the position of the background images based on the speed.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Update(GameTime gameTime)
		{
			pos1 -= speed;
			pos2 -= speed;

			if (pos1.X <= -srcRec.Width)
			{
				pos1.X = pos2.X + srcRec.Width;
			}

			if (pos2.X <= -srcRec.Width)
			{
				pos2.X = pos1.X + srcRec.Width;
			}

			base.Update(gameTime);
		}
	}
}

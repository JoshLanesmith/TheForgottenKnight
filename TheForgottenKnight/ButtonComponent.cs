using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	public enum ButtonStatus
	{
		Neutral,
		Hover,
		Clicked
	}

	/// <summary>
	/// Button compontent to display a button and execute a specified action
	/// </summary>
	public class ButtonComponent : DrawableGameComponent
	{
		/// <summary>
		/// OnClick delegate to allow the function of the button to be defined through the constructor
		/// </summary>
		public delegate void OnClick();


		private ButtonStatus buttonStatus;
		private Vector2 position;
		private Rectangle boundaries;
		private Texture2D tex;
		private OnClick onClick;

		/// <summary>
		/// Create a button with a specified position texture and OnClick acction
		/// </summary>
		/// <param name="game">The game context for the ButtonComponent</param>
		/// <param name="position">The postion of the button's top left corner</param>
		/// <param name="tex">The image used for the button</param>
		/// <param name="onClick">The OnClick fuction to be executed</param>
		public ButtonComponent(Game game, Vector2 position, Texture2D tex, OnClick onClick) : base(game)
		{
			this.tex = tex;
			this.onClick = onClick;

			this.position = position + Shared.displayPosShift;
			
			boundaries = new Rectangle((int)this.position.X, (int)this.position.Y, tex.Width, tex.Height);

			buttonStatus = ButtonStatus.Neutral;
		}

		public override void Update(GameTime gameTime)
		{
			MouseState ms = Mouse.GetState();
			Point mousePoint = new Point(ms.X, ms.Y);

			// Check if the button is hovered over by the mouse and clicked
			if (boundaries.Contains(mousePoint))
			{
				buttonStatus = ButtonStatus.Hover;

				if (ms.LeftButton == ButtonState.Pressed && buttonStatus== ButtonStatus.Hover)
				{
					buttonStatus = ButtonStatus.Clicked;
					onClick();
				}
			}


			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();

			Shared.sb.Draw(tex, boundaries, Color.White);

			Shared.sb.End();


			base.Draw(gameTime);
		}


	}
}

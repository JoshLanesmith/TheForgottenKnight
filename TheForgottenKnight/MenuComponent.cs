using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	/// <summary>
	/// MenuComonent used to draw the menu items and toggle the selected menu items
	/// </summary>
	public class MenuComponent : DrawableGameComponent
	{
		//Menu Components
		private SpriteFont regularFont, hilightFont, titleFont;
		private List<string> menuItems;
		private string titleString = "The Forgotten Knight";
		private Texture2D bgImage;
		private SoundEffect menuHoverSFX;
		private SoundState menuMusic;

		public int SelectedIndex { get; set; }
		private Vector2 position;
		private Color regularColor = Color.Black;
		private Color hilightColor = Color.Brown;

		private KeyboardState oldState;

		/// <summary>
		/// Generate menu items with an array of strings for the menu items' text
		/// Loads soundEffects to play while traversing the menu
		/// </summary>
		/// <param name="game">The game context for the nenu</param>
		/// <param name="menus">Array of strings for the menu items</param>
		public MenuComponent(Game game, string[] menus) : base(game)
		{
			this.regularFont = Shared.regularFont;
			this.hilightFont = Shared.hilightFont;
			this.titleFont = Shared.titleFont;
			this.bgImage = Shared.menuBgImage;

			menuHoverSFX = game.Content.Load<SoundEffect>("sfx/start-menu-sfx/menuHover");

			menuItems = menus.ToList();
			position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
		}

		public override void Update(GameTime gameTime)
		{
			KeyboardState ks = Keyboard.GetState();

			// Toggle down through the menu items 
			if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
			{
				menuHoverSFX.Play();
				SelectedIndex++;

				// If currently on the last menu item then loop to the top item
				if (SelectedIndex == menuItems.Count)
				{
					SelectedIndex = 0;
				}
			}

			// Toggle up through the menu items 
			if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
			{
				menuHoverSFX.Play();
				SelectedIndex--;

				// If currently on the first menu item then loop to the bottom item
				if (SelectedIndex == -1)
				{
					SelectedIndex = menuItems.Count() - 1;
				}
			}


			oldState = ks;
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{

			//Values for Draw Position
			#region Values
			int screenWidth = GraphicsDevice.Viewport.Width;
			int screenHeight = GraphicsDevice.Viewport.Height;
			int totalTextHeight = menuItems.Count() * hilightFont.LineSpacing;
			int offsetY = 5;
			int titleOffsetY = 220;
			int titleOffsetX = 165;
			int startY = (screenHeight - totalTextHeight) / 2 - offsetY;
			int startX = (screenWidth - (int)hilightFont.MeasureString(menuItems[0]).X) / 2;
			#endregion

			//Position Vectors
			Vector2 tempPos = new Vector2(startX, startY);
			Vector2 titlePos = new Vector2(startX - titleOffsetX, titleOffsetY);

			Shared.sb.Begin();
			Vector2 vPos = new Vector2((GraphicsDevice.Viewport.Width - bgImage.Width) / 2, (GraphicsDevice.Viewport.Height - bgImage.Height) / 2);
			Shared.sb.Draw(bgImage, vPos, Color.White);

			Shared.sb.DrawString(titleFont, titleString, titlePos, hilightColor);

			for (int i = 0; i < menuItems.Count(); i++)
			{
				if (i == SelectedIndex)
				{
					// Draw the selected item with the hilighted font and color
					Shared.sb.DrawString(hilightFont, menuItems[i], tempPos, hilightColor);
					tempPos.Y += hilightFont.LineSpacing;
				}
				else
				{
					// Draw all other items with the regular font and color
					Shared.sb.DrawString(regularFont, menuItems[i], tempPos, regularColor);
					tempPos.Y += hilightFont.LineSpacing;
				}

			}

			Shared.sb.End();

			base.Draw(gameTime);
		}
	}
}

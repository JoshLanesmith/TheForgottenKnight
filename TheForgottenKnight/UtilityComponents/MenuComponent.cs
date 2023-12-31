﻿/* MenuComponent.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using TheForgottenKnight.Scenes;

namespace TheForgottenKnight
{
    /// <summary>
    /// MenuComonent used to draw the menu items and toggle the selected menu items
    /// </summary>
    public class MenuComponent : DrawableGameComponent
	{
		//Menu Components
		private GameScene scene;
		private SpriteFont regularFont, highlightFont, titleFont;
		private List<string> menuStrings;
		private string titleString = "The Forgotten Knight";
		private Texture2D bgImage;
		private SoundEffect menuHoverSFX;
		private SoundState menuMusic;
		private DrawableRectangle backPanel;

		public int SelectedIndex { get; set; }
		private Vector2 position;
		private Vector2 titlePos;
		private Color regularColor = Color.Black;
		private Color highlightColor = new Color(135, 18, 18);

		private List<ClickableString> menuItems;


		private KeyboardState oldState;
        private Point oldMousePoint;

        /// <summary>
        /// Generate menu items with an array of strings for the menu items' text
        /// Loads soundEffects to play while traversing the menu
        /// </summary>
        /// <param name="game">The game context for the menu</param>
		/// <param name="scene">The game scene context for the menu</param>
        /// <param name="menuStrings">Array of strings for the menu items</param>
		/// <param name="menuActions">Array of actions for the menu items</param>
        public MenuComponent(Game game, GameScene scene, string[] menuStrings, ClickableString.OnClick[] menuActions) : base(game)
		{
			Game1 g = (Game1)game;

			this.scene = scene;
			this.regularFont = Shared.regularFont;
			this.highlightFont = Shared.highlightFont;
			this.titleFont = Shared.titleFont;
			this.bgImage = Shared.menuBgImage;

			menuHoverSFX = game.Content.Load<SoundEffect>("sfx/start-menu-sfx/menuHover");

			menuItems = new List<ClickableString>();
			this.menuStrings = menuStrings.ToList();
			position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);


            // Values for Draw Position
            #region Values
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;
            int totalMenuHeight = this.menuStrings.Count() * highlightFont.LineSpacing;
            int offsetY = 5;
            int titleOffsetY = 60;
			int titleWidth = (int)titleFont.MeasureString(titleString).X;
            int startY = (screenHeight - totalMenuHeight) / 2 - offsetY;
            int startX = (screenWidth - (int)regularFont.MeasureString(this.menuStrings[0]).X) / 2;
            #endregion

            // Position Vectors
            Vector2 tempPos = new Vector2(startX, startY);
            titlePos = new Vector2((screenWidth - titleWidth) / 2, startY  - titleOffsetY);

			backPanel = new DrawableRectangle(game, titleWidth, titleOffsetY + totalMenuHeight, titlePos, new Color(120, 120, 120), 150);
			scene.Components.Add(backPanel);

			// Instantiate each clickable string and add them to the scene components
            for (int i = 0; i < this.menuStrings.Count(); i++)
            {

				ClickableString menuItem = new ClickableString(game, regularFont, highlightFont, this.menuStrings[i], tempPos, regularColor, highlightColor, menuActions[i]);
				menuItems.Add(menuItem);
				
				scene.Components.Add(menuItem);
                tempPos.Y += highlightFont.LineSpacing;

            }

			menuItems[0].ToggleSelected();

		}

		public override void Update(GameTime gameTime)
		{
			KeyboardState ks = Keyboard.GetState();

			// Toggle down through the menu items 
			if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
			{
				menuHoverSFX.Play();
				menuItems[SelectedIndex].ToggleSelected();
				SelectedIndex++;

				// If currently on the last menu item then loop to the top item
				if (SelectedIndex == menuStrings.Count)
				{
					SelectedIndex = 0;
				}
				menuItems[SelectedIndex].ToggleSelected();
			}

			// Toggle up through the menu items 
			if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
			{
				menuHoverSFX.Play();
				menuItems[SelectedIndex].ToggleSelected();
				SelectedIndex--;

				// If currently on the first menu item then loop to the bottom item
				if (SelectedIndex == -1)
				{
					SelectedIndex = menuStrings.Count() - 1;
				}
				menuItems[SelectedIndex].ToggleSelected();
			}

            MouseState ms = Mouse.GetState();
            Point mousePoint = new Point(ms.X, ms.Y);

			// Update the selecte menu item when the mouse moves and it is hovering over one of the menu items
			if (mousePoint != oldMousePoint)
			{
				oldMousePoint = mousePoint;
				foreach (ClickableString menuItem in menuItems)
				{
					if (menuItem.Bounds.Contains(mousePoint) && menuItems[SelectedIndex] != menuItem)
					{
						// Toggle the previously selected menu item
						menuItems[SelectedIndex].ToggleSelected();

						// Toggle the currently selected menu item and update the hover status
						menuItem.ToggleSelected();
						menuItem.ButtonStatus = ButtonStatus.Hover;
						menuHoverSFX.Play();

						// Update the selected index to that of the currently selected menu item
						SelectedIndex = menuItems.IndexOf(menuItem);
					}
				} 
			}

            oldState = ks;
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			Shared.sb.Begin();

			Shared.sb.DrawString(titleFont, titleString, titlePos, highlightColor);

			Shared.sb.End();

		}
	}
}

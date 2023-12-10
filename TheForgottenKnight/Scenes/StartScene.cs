/* StartScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.26: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheForgottenKnight.Scenes
{
    /// <summary>
    /// Represents the starting scene of the game, including the main menu.
    /// </summary>
    public class StartScene : GameScene
	{
		private MenuComponent menu;
		private CustomCursor cursor;

		/// <summary>
		/// Initializes a new instance of the StartScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		public StartScene(Game game) : base(game)
		{
			Game1 g = (Game1)game;

			string[] menuItems = { "Start Game", "Help", "High Score", "Credit", "Quit" };

			ClickableString.OnClick[] menuActions =
			{
				() => { g.GoToActionScene(); },
				() => { g.GoToHelpScene(); },
				() => { g.GoToHighscoreScene(); },
				() => { g.GoToCreditScene(); },
				() => { g.ExitGame(); }
			};

			Menu = new MenuComponent(game, this, menuItems, menuActions);
			Components.Add(Menu);

			cursor = new CustomCursor(game);
			Components.Add(cursor);
		}

		/// <summary>
		/// Gets or sets the menu component associated with the start scene.
		/// </summary>
		public MenuComponent Menu { get => menu; set => menu = value; }

		/// <summary>
		/// Draws the start scene, displaying the background and associated components.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheForgottenKnight.Scenes
{
	public class EndScene : GameScene
	{
		private Player player;
		private Score newScore;
		private StringInputManager stringInputManager;
		private HighScoreManager highScoreManager;
		private ButtonComponent saveButton;
		private ButtonComponent cancelButton;
		private Vector2 topBannerPosition;
		private Vector2 tablePosition;
		private Vector2 button1Position;
		private Vector2 button2Position;
		private Color regularColor = Color.Black;

		public EndScene(Game game, Player player) : base(game)
		{
			this.player = player;
			newScore = new Score();
			stringInputManager = new StringInputManager(game);
			Components.Add(stringInputManager);
            topBannerPosition = new Vector2(Shared.gameDisplaySize.X - Shared.scrollPnlImage.Width - 20, 70);
			tablePosition = new Vector2(Shared.gameDisplaySize.X - Shared.scrollPnlImage.Width - 20, topBannerPosition.Y + Shared.scrollPnlImageSmall.Height + 40);
			highScoreManager = new HighScoreManager(game, newScore, tablePosition);
			Components.Add(highScoreManager);

			Texture2D cancelButtonTex = Game.Content.Load<Texture2D>("images/cancel_red");

			button1Position = tablePosition + new Vector2(Shared.scrollPnlImage.Width / 2 - cancelButtonTex.Width - 10, Shared.scrollPnlImage.Height + 20);
			button2Position = tablePosition + new Vector2(Shared.scrollPnlImage.Width / 2 + 10, Shared.scrollPnlImage.Height + 20);

			cancelButton = new ButtonComponent(game, button1Position, cancelButtonTex, () => {

				Game1 g = (Game1)game;
				g.ResetGame();
			});
			Components.Add(cancelButton);
		}

		public override void Update(GameTime gameTime)
		{
			Game1 g = (Game1)Game;
			if (Enabled && g.CurrentScene != this)
			{
				g.CurrentScene = this;

				newScore.LevelsCompleted = player.LevelsCompleted; 
				newScore.TimeSpent = player.TimeSpent;

				if (highScoreManager.NewScoreIsHighScore(newScore))
				{
					Texture2D saveButtonTex = Game.Content.Load<Texture2D>("images/save_red");
					saveButton = new ButtonComponent(Game, button2Position, saveButtonTex, () => {

						highScoreManager.SaveHighScores();

						Game1 g = (Game1)Game;
						g.ResetGame();

					});
					Components.Add(saveButton);
				}
			}

			base.Update(gameTime);

			newScore.PlayerName = stringInputManager.InputText;
		}

		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			Shared.sb.Draw(Shared.gameWonBgImage, Shared.displayPosShift, new Rectangle(0, 0, Shared.gameWonBgImage.Width, Shared.gameWonBgImage.Height),
                Color.White, 0.0f, Vector2.Zero, Shared.gameDisplaySize.X / Shared.gameWonBgImage.Width, SpriteEffects.None, 0);

			Shared.sb.Draw(Shared.scrollPnlImageSmall, topBannerPosition + Shared.displayPosShift, new Rectangle(0, 0, Shared.scrollPnlImageSmall.Width, Shared.scrollPnlImageSmall.Height),
				Color.White);

            Shared.sb.DrawString(Shared.smallFont, $"Your Score", topBannerPosition + Shared.displayPosShift + new Vector2(50, 30), regularColor);
			Shared.sb.DrawString(Shared.smallFont, $"Levels Completed: {player.LevelsCompleted}",
                topBannerPosition + new Vector2(50, 30 + Shared.regularFont.LineSpacing) + Shared.displayPosShift, regularColor);
			Shared.sb.DrawString(Shared.smallFont, $"Time Spent in Completed Levels: {player.TimeSpent:0.##} seconds",
                topBannerPosition + new Vector2(50, 30 + Shared.regularFont.LineSpacing * 2) + Shared.displayPosShift, regularColor);


			Shared.sb.End();

			base.Draw(gameTime);
		}
	}
}

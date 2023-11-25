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
		private Color regularColor = Color.White;

		public EndScene(Game game, Player player) : base(game)
		{
			this.player = player;
			newScore = new Score();
			stringInputManager = new StringInputManager(game);
			Components.Add(stringInputManager);
			Vector2 tablePosition = new Vector2(100, 300);
			highScoreManager = new HighScoreManager(game, newScore, tablePosition);
			Components.Add(highScoreManager);

			Texture2D cancelButtonTex = Game.Content.Load<Texture2D>("images/cancel_red");
			cancelButton = new ButtonComponent(game, new Vector2(0, 10), cancelButtonTex, () => {

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
					saveButton = new ButtonComponent(Game, new Vector2(saveButtonTex.Width + 50, 10), saveButtonTex, () => {

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
			Shared.sb.DrawString(Shared.regularFont, $"Your Score", new Vector2(100, 100) + Shared.displayPosShift, regularColor);
			Shared.sb.DrawString(Shared.regularFont, $"Levels Completed: {player.LevelsCompleted}", new Vector2(100, 100 + Shared.regularFont.LineSpacing) + Shared.displayPosShift, regularColor);
			Shared.sb.DrawString(Shared.regularFont, $"Time Spent in Completed Levels: {player.TimeSpent:0.##} seconds", new Vector2(100, 100 + Shared.regularFont.LineSpacing * 2) + Shared.displayPosShift, regularColor);


			Shared.sb.End();

			base.Draw(gameTime);
		}
	}
}

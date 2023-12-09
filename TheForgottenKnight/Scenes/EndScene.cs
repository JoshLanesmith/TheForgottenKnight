/* EndScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
	/// <summary>
	/// Represents the end game scene in the game, displaying scores, providing options to save scores, exit the game, or restart.
	/// </summary>
	public class EndScene : GameScene
	{
		private Player player;
		private Score newScore;
		private StringInputManager stringInputManager;
		private HighScoreManager highScoreManager;
		private ButtonComponent saveButton;
		private ButtonComponent cancelButton;
		private ButtonComponent exitButton;
		private Vector2 topBannerPosition;
		private Vector2 tablePosition;
		private Vector2 button1Position;
		private Vector2 button2Position;
		private Vector2 button3Position;
		private Color regularColor = Color.Black;
		private SoundEffect saveClick;
		private SoundEffect cancelClick;
        private CustomCursor cursor;
		private float scrollPanelScalingFactor;


		/// <summary>
		/// Initializes a new instance of the EndGameScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		/// <param name="player">The player object associated with the end game scene.</param>
		/// <param name="newScore">The new score to be displayed and potentially saved.</param>
		/// <param name="stringInputManager">The manager for handling string input for the player's name.</param>
		public EndScene(Game game, Player player) : base(game)
        {
            Game1 g = (Game1)game;
            this.player = player;
			newScore = new Score();
			stringInputManager = new StringInputManager(game);
			Components.Add(stringInputManager);
            topBannerPosition = new Vector2(Shared.gameDisplaySize.X - Shared.scrollPnlImage.Width - 20, 70);
			tablePosition = new Vector2(Shared.gameDisplaySize.X - Shared.scrollPnlImage.Width - 20, topBannerPosition.Y + Shared.scrollPnlImageSmall.Height + 40);
			highScoreManager = new HighScoreManager(game, newScore, tablePosition);
			Components.Add(highScoreManager);

			scrollPanelScalingFactor = Shared.gameDisplaySize.X / Shared.gameWonBgImage.Width;


            Texture2D cancelButtonTex = Game.Content.Load<Texture2D>("images/buttons/cancelButton");
			Dictionary<ButtonStatus, Texture2D> cancelButtonTextures = new Dictionary<ButtonStatus, Texture2D>()
            {
				{ButtonStatus.Neutral, Game.Content.Load<Texture2D>("images/buttons/cancelButton") },
				{ButtonStatus.Hover, Game.Content.Load<Texture2D>("images/buttons/cancelButton1h") },
				{ButtonStatus.Clicked, Game.Content.Load<Texture2D>("images/buttons/cancelButton1d") }
			};

            Dictionary<ButtonStatus, Texture2D> saveButtonTextures = new Dictionary<ButtonStatus, Texture2D>()
            {
                {ButtonStatus.Neutral, Game.Content.Load<Texture2D>("images/buttons/saveButton") },
                {ButtonStatus.Hover, Game.Content.Load<Texture2D>("images/buttons/saveButton1h") },
                {ButtonStatus.Clicked, Game.Content.Load<Texture2D>("images/buttons/saveButton1d") },
                {ButtonStatus.Disabled, Game.Content.Load<Texture2D>("images/buttons/saveButtonDisabled") }
            };

            Dictionary<ButtonStatus, Texture2D> exitButtonTextures = new Dictionary<ButtonStatus, Texture2D>()
            {
                {ButtonStatus.Neutral, Game.Content.Load<Texture2D>("images/buttons/exitButton") },
                {ButtonStatus.Hover, Game.Content.Load<Texture2D>("images/buttons/exitButton1h") },
                {ButtonStatus.Clicked, Game.Content.Load<Texture2D>("images/buttons/exitButton1d") }
            };

            cancelClick = Game.Content.Load<SoundEffect>("sfx/end-menu-sfx/cancel");
            saveClick = Game.Content.Load<SoundEffect>("sfx/end-menu-sfx/save");

            int numberOfButtons = 3;
			int buttonGap = 20;
			int buttonWidth = (Shared.scrollPnlImage.Width - (buttonGap * (numberOfButtons - 1))) /3;
			float btnScalingFactor = scrollPanelScalingFactor * ((float)buttonWidth / cancelButtonTextures[ButtonStatus.Neutral].Width);


            button1Position = tablePosition + new Vector2(0, Shared.scrollPnlImage.Height + 20);
			button2Position = button1Position + new Vector2(buttonWidth + buttonGap, 0);
			button3Position = button2Position + new Vector2(buttonWidth + buttonGap, 0);

			cancelButton = new ButtonComponent(game, button1Position, cancelButtonTextures, buttonWidth, btnScalingFactor, () => {
				cancelClick.Play();
				g.ResetGame();
			});
			Components.Add(cancelButton);

            saveButton = new ButtonComponent(game, button2Position, saveButtonTextures, buttonWidth, btnScalingFactor, () =>
			{
                saveClick.Play();
                highScoreManager.SaveHighScores();
				g.ResetGame();
			});
            Components.Add(saveButton);

            exitButton = new ButtonComponent(game, button3Position, exitButtonTextures, buttonWidth, btnScalingFactor, () =>
			{
                cancelClick.Play();
				g.ExitGame();
            });
            Components.Add(exitButton);

            cursor = new CustomCursor(game);
            Components.Add(cursor);
        }

		/// <summary>
		/// Updates the end game scene, checking for a new high score, enabling/disabling save button, and handling player input.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Update(GameTime gameTime)
		{
			Game1 g = (Game1)Game;
			if (Enabled && g.CurrentScene != this)
			{
				g.CurrentScene = this;

				newScore.LevelsCompleted = player.LevelsCompleted; 
				newScore.TimeSpent = player.TimeSpent;

				if (!highScoreManager.NewScoreIsHighScore(newScore))
				{
					saveButton.ButtonStatus = ButtonStatus.Disabled;
				}
			}

			base.Update(gameTime);

			newScore.PlayerName = stringInputManager.InputText;
		}

		/// <summary>
		/// Draws the end game scene, displaying background images, score information, and buttons for various actions.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			Shared.sb.Draw(Shared.gameWonBgImage, Shared.displayPosShift, new Rectangle(0, 0, Shared.gameWonBgImage.Width, Shared.gameWonBgImage.Height),
                Color.White, 0.0f, Vector2.Zero, scrollPanelScalingFactor, SpriteEffects.None, 0);
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

/* HighScoreScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.26: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheForgottenKnight.Scenes
{
    /// <summary>
    /// Represents a scene dedicated to displaying high scores.
    /// </summary>
    public class HighScoreScene : GameScene
	{
		private HighScoreManager highScoreManager;
		/// <summary>
		/// Initializes a new instance of the HighScoreScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		public HighScoreScene(Game game) : base(game)
		{
			
			Vector2 tablePosition = new Vector2(Shared.gameDisplaySize.X / 2 - Shared.scrollPnlImage.Width / 2, Shared.gameDisplaySize.Y / 2 - Shared.scrollPnlImage.Height / 2);
			highScoreManager = new HighScoreManager(game, tablePosition);
			Components.Add(highScoreManager);
		
		}

        public override void Show()
        {
			highScoreManager.LoadHighScores();
            base.Show();
        }

        /// <summary>
        /// Draws the high score scene, displaying the background and high score manager.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game's timing state.</param>
        public override void Draw(GameTime gameTime)
        {
			Shared.sb.Begin();
            Shared.sb.Draw(Shared.highscoreBgImage, Shared.displayPosShift, new Rectangle(0, 0, Shared.highscoreBgImage.Width, Shared.highscoreBgImage.Height),
                Color.White, 0.0f, Vector2.Zero, Shared.gameDisplaySize.X / Shared.highscoreBgImage.Width, SpriteEffects.None, 0);

            Shared.sb.End();
            base.Draw(gameTime);
        }
    }
}

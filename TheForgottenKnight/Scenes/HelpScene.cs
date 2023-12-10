/* HelpScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.12.01: Created        
 */
using Microsoft.Xna.Framework;
using TiledCS;

namespace TheForgottenKnight.Scenes
{
    /// <summary>
    /// Represents a scene dedicated to providing help and instructions to the player.
    /// </summary>
    public class HelpScene : GameScene
	{
		private HelpMap helpMap;
		private int numberOfLevels;
		private int currentLevelIndex;
		private bool gameOver;
		private float delay = 0;
		private bool timerTrigger = false;


		/// <summary>
		/// Initializes a new instance of the HelpScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		public HelpScene(Game game) : base(game)
		{
			GameOver = false;

			currentLevelIndex = 1;

			TiledMap helpTiledMap = new TiledMap(Game.Content.RootDirectory + "\\maps\\help_map\\help_map.tmx");
			Vector2 bagPosition = new Vector2(Shared.stage.X / 2 + 48, Shared.stage.Y - Shared.labelFont.LineSpacing - 32) - Shared.displayPosShift;
			Vector2 timerPosition = new Vector2(Shared.stage.X / 2 + 48, Shared.stage.Y / 40 * 11) - Shared.displayPosShift;
			helpMap = new HelpMap(game, helpTiledMap, bagPosition, timerPosition);

			Components.Add(helpMap);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the help scene is in a game-over state.
		/// </summary>
		public bool GameOver { get => gameOver; set => gameOver = value; }


		/// <summary>
		/// Updates the help scene, checking for game-over conditions and handling resets.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Update(GameTime gameTime)
		{
			Game1 g = (Game1)Game;
			if (Enabled && g.CurrentScene != this)
			{
				g.CurrentScene = this;
			}

			if ((helpMap.CurrentLevelStatus == LevelStatus.Won || helpMap.CurrentLevelStatus == LevelStatus.Lost) && helpMap.PreviousLevleStatus == LevelStatus.Running)
			{
				helpMap.PreviousLevleStatus = helpMap.CurrentLevelStatus;

				if (!timerTrigger)
				{
					WaitTime(1);
				}

			}

			if (delay > 0) delay -= 1f / 1000f * (float)gameTime.ElapsedGameTime.Milliseconds;
			if (delay <= 0 && timerTrigger)
			{
				helpMap.ResetMap();
				helpMap.CountDownTimer.ResetTimer();
				timerTrigger = false;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Sets a delay time for triggering specific actions in the help scene.
		/// </summary>
		/// <param name="amountoftime">The time to wait before triggering the action.</param>
		private void WaitTime(float amountoftime)
		{
			delay = amountoftime;
			timerTrigger = true;
		}

	}
}

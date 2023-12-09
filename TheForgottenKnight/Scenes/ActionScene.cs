/* ActionScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace TheForgottenKnight.Scenes
{
	/// <summary>
	/// Represents the action scene in the game, managing level progression, player stats, and game over conditions.
	/// </summary>
	public class ActionScene : GameScene
	{
		private Map currentLevelMap;
		private int numberOfLevels;
		private int currentLevelIndex;
		private Dictionary<int, TiledMap> mapLevels;
		private Player player;
		private bool gameOver;
		private float delay = 0;
		private bool timerTrigger = false;

		/// <summary>
		/// Initializes a new instance of the ActionScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		/// <param name="player">The player object associated with the action scene.</param>
		public ActionScene(Game game, Player player) : base(game)
		{
			this.player = player;
			GameOver = false;

			currentLevelIndex = 1;
			mapLevels = SetMapLevels();
			currentLevelMap = new Map(game, mapLevels[currentLevelIndex]);

			Components.Add(currentLevelMap);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the game is over.
		/// </summary>
		public bool GameOver { get => gameOver; set => gameOver = value; }

		/// <summary>
		/// Updates the action scene, handling level completion, player stats, and game over conditions.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Update(GameTime gameTime)
		{
			Game1 g = (Game1)Game;
			if (Enabled && g.CurrentScene != this)
			{
				g.CurrentScene = this;
			}

			if (currentLevelMap.CurrentLevelStatus == LevelStatus.Won && currentLevelMap.PreviousLevleStatus == LevelStatus.Running)
			{
				currentLevelMap.PreviousLevleStatus = LevelStatus.Won;


				float timeSpentOnLevel = currentLevelMap.TimerStartTime - currentLevelMap.CountDownTimer.CountDownTime;

				player.UpdateLevelCompletedStats(timeSpentOnLevel);

				if (currentLevelIndex < numberOfLevels)
				{
					if (!timerTrigger)
					{
						WaitTime(1);
					}
				}
				else
				{
					GameOver = true;
					MediaPlayer.Stop();
				}
			}

			if (currentLevelMap.CurrentLevelStatus == LevelStatus.Lost && currentLevelMap.PreviousLevleStatus == LevelStatus.Running)
			{
				currentLevelMap.PreviousLevleStatus = LevelStatus.Lost;
				GameOver = true;
			}

			if (delay > 0) delay -= 1f / 1000f * (float)gameTime.ElapsedGameTime.Milliseconds;
			if (delay <= 0 && timerTrigger)
			{
				Components.Remove(currentLevelMap);
				currentLevelIndex++;
				currentLevelMap = new Map(Game, mapLevels[currentLevelIndex]);
				Components.Add(currentLevelMap);
				timerTrigger = false;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Sets the map levels based on the TMX files in the "maps" directory.
		/// </summary>
		/// <returns>A dictionary mapping level numbers to TiledMap instances.</returns>
		private Dictionary<int, TiledMap> SetMapLevels()
		{
			string mapsRootDirectory = Game.Content.RootDirectory + "\\maps";
			string[] mapFileNames = Directory.GetFiles(mapsRootDirectory, "*.tmx", SearchOption.TopDirectoryOnly);
			numberOfLevels = mapFileNames.Length;

			Dictionary<int, TiledMap> mapLevels = mapFileNames.ToDictionary(
				fileName => int.TryParse(Path.GetFileNameWithoutExtension(fileName), out int fileNumber) ? fileNumber : -1,
				fileName => new TiledMap(fileName)
			);

			return mapLevels;
		}

		/// <summary>
		/// Waits for a specified amount of time before triggering the next level.
		/// </summary>
		/// <param name="amountOfTime">The time to wait in seconds.</param>
		private void WaitTime(float amountoftime)
		{
			delay = amountoftime;
			timerTrigger = true;
		}
	}
}

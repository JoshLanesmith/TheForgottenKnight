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
	public class ActionScene : GameScene
	{
		//private Bat bat;
		private Map currentLevelMap;
		private int numberOfLevels;
		private int currentLevelIndex;
		private Dictionary<int, TiledMap> mapLevels;
		private Player player;
		private bool gameOver;
		private float delay = 0;
		private bool trigger = false;


		public ActionScene(Game game, Player player) : base(game)
		{
			this.player = player;
			GameOver = false;

			currentLevelIndex = 1;
			mapLevels = SetMapLevels();
			currentLevelMap = new Map(game, mapLevels[currentLevelIndex]);

			Components.Add(currentLevelMap);
		}

		public bool GameOver { get => gameOver; set => gameOver = value; }

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
					if (!trigger)
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
			if (delay <= 0 && trigger)
			{
				Components.Remove(currentLevelMap);
				currentLevelIndex++;
				currentLevelMap = new Map(Game, mapLevels[currentLevelIndex]);
				Components.Add(currentLevelMap);
				trigger = false;
			}

			base.Update(gameTime);
		}


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

		private void WaitTime(float amountoftime)
		{
			delay = amountoftime;
			trigger = true;
		}
	}
}

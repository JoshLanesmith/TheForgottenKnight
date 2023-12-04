using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace TheForgottenKnight.Scenes
{
	public class HelpScene : GameScene
	{
		private HelpMap helpMap;
		private int numberOfLevels;
		private int currentLevelIndex;
		private bool gameOver;
		private float delay = 0;
		private bool timerTrigger = false;

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

		public bool GameOver { get => gameOver; set => gameOver = value; }

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

		private void WaitTime(float amountoftime)
		{
			delay = amountoftime;
			timerTrigger = true;
		}

	}
}

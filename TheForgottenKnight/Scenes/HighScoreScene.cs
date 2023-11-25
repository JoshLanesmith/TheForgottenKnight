using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.Scenes
{
	public class HighScoreScene : GameScene
	{
		private HighScoreManager highScoreManager;

		public HighScoreScene(Game game) : base(game)
		{
			Vector2 tablePosition = new Vector2(100, 300);
			highScoreManager = new HighScoreManager(game, tablePosition);
			Components.Add(highScoreManager);
		}
	}
}

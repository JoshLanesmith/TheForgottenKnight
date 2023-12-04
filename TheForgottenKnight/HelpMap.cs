using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForgottenKnight.MapComponents;
using TiledCS;

namespace TheForgottenKnight
{
	public class HelpMap : Map
	{
		private Vector2 bagPosition;
		private Vector2 timerPosition;

		public HelpMap(Game game, TiledMap map, Vector2 bagPosition, Vector2 timerPosition) : base(game, map)
		{
			Components.Remove(bag);
			Components.Remove(CountDownTimer);

			this.bagPosition = bagPosition;
			bag = new Bag(game, this, bagPosition);
			Components.Add(bag);
			this.timerPosition = timerPosition;
			CountDownTimer = new CountDownTimer(game, this, TimerStartTime, timerPosition);
			Components.Add(CountDownTimer);
		}
	}
}

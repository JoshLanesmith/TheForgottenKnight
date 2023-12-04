using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.MapComponents
{
	/// <summary>
	/// Count down timer used to track how long the player spent in a level
	/// </summary>
	public class CountDownTimer : DrawableGameComponent
	{
		private Map map;
		private SpriteFont font;
		private Vector2 position;
		private string labelText = "Timer";
		private Color labelColor = Color.White;
		private float countDownTime;
		private float startCountDownTime;

		public float CountDownTime { get => countDownTime; }

		/// <summary>
		/// Create a count down timer for the level
		/// </summary>
		/// <param name="game">The game context for the timer</param>
		/// <param name="map">The map context for the timer</param>
		/// <param name="countDownTime">The start time of the timer</param>
		public CountDownTimer(Game game, Map map, float countDownTime) : base(game)
		{
			this.map = map;
			this.font = Shared.labelFont;
			position = new Vector2(16, 0);
			this.countDownTime = countDownTime;
			startCountDownTime = countDownTime;
		}

		public CountDownTimer(Game game, Map map, float countDownTime, Vector2 position) : base(game)
		{
			this.map = map;
			this.font = Shared.labelFont;
			this.position = position;
			this.countDownTime = countDownTime;
			startCountDownTime = countDownTime;
		}

		public void ResetTimer()
		{
			countDownTime = startCountDownTime;
		}

		public override void Update(GameTime gameTime)
		{
			// Reduce the CountDownTimer on update if the CurrentLevelStatus is Running 
			if (map.CurrentLevelStatus == LevelStatus.Running)
			{
				countDownTime -= 1f / 1000f * (float)gameTime.ElapsedGameTime.Milliseconds;
			}

			// Set the CurrentLevelStatus as Lost if the timer reaches zero
			if (countDownTime <= 0)
			{
				countDownTime = 0.0f;
				map.CurrentLevelStatus = LevelStatus.Lost;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			Shared.sb.DrawString(font, $"{labelText}: {countDownTime:0.##}", position + Shared.displayPosShift, labelColor);

			Shared.sb.End();

			base.Draw(gameTime);
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	/// <summary>
	/// Player class to track the player score throughout the levels.
	/// </summary>
	public class Player : GameComponent
	{


		//Player Information
		private float timeSpent;
		private int levelsCompleted;
		private string? playerName;

		/// <summary>
		/// Creates a new player with a timeSpent of 0.0 seconds and 0 levels completed. 
		/// </summary>
		/// <param name="game">The game context that the player exists within</param>
		public Player(Game game) : base(game)
		{
			timeSpent = 0.0f;
			levelsCompleted = 0;
		}

		public float TimeSpent { get => timeSpent; }
		public int LevelsCompleted { get => levelsCompleted; }
		public string PlayerName { get => playerName; set => playerName = value; }

		/// <summary>
		/// Update the total time spent and number of levels completed in a single run.
		/// </summary>
		/// <param name="timeSpentInLevel">Time spent in the level that was just completed</param>
		public void UpdateLevelCompletedStats(float timeSpentInLevel)
		{
			timeSpent += timeSpentInLevel;
			levelsCompleted++;
		}
	}
}

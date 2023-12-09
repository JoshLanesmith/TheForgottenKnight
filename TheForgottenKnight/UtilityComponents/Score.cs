using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	/// <summary>
	/// Score class to store a players name and score details
	/// </summary>
	public class Score
	{
		private string playerName;
		private int levelsCompleted;
		private float timeSpent;

		/// <summary>
		/// Create a Score object with an empty name and scores of 0
		/// </summary>
		public Score()
		{
			this.playerName = string.Empty;
			this.levelsCompleted = 0;
			this.timeSpent = 0f;
		}

		/// <summary>
		/// Create a Score object with an empty name and the given scores
		/// </summary>
		/// <param name="levelsCompleted">Number of levels completed</param>
		/// <param name="timeSpent">Amount of time spent in the levels</param>
		public Score(int levelsCompleted, float timeSpent)
		{
			this.playerName = string.Empty;
			this.levelsCompleted = levelsCompleted;
			this.timeSpent = timeSpent;
		}

		/// <summary>
		/// Create a Score object with a name and given scores
		/// </summary>
		/// <param name="playerName"></param>
		/// <param name="levelsCompleted"></param>
		/// <param name="timeSpent"></param>
		public Score(string playerName, int levelsCompleted, float timeSpent)
		{
			this.playerName = playerName;
			this.levelsCompleted = levelsCompleted;
			this.timeSpent = timeSpent;
		}

		public string PlayerName { get => playerName; set => playerName = value; }
		public int LevelsCompleted { get => levelsCompleted; set => levelsCompleted = value; }
		public float TimeSpent { get => timeSpent; set => timeSpent = value; }
	}
}

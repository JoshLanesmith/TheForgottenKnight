using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	/// <summary>
	/// HighScoreManager used to load, save, update, and display the high scores
	/// </summary>
	public class HighScoreManager : DrawableGameComponent
	{
		private List<Score> highScores;
		private string fileName;
		private Score? newScore;

		// Declare fields to draw the High Score Table
		private Vector2 tablePosition;
		private float column1X;
		private float column2X;
		private float column3X;
		private Color newScoreColor = Color.Yellow;
		private Color regularColor = Color.White;

		/// <summary>
		/// Creates a High Score Manager to load and display the current high scores
		/// </summary>
		/// <param name="game">The game context for the HighScoreManager</param>
		/// <param name="tablePosition">The postion for the top left point of the display table</param>
		public HighScoreManager(Game game, Vector2 tablePosition) : base(game)
		{
			fileName = "HighScores.txt";
			HighScores = LoadHighScores();
			SortHighScores();
			this.tablePosition = tablePosition + Shared.displayPosShift;
			column1X = 0;
			column2X = column1X + 300;
			column3X = column2X + 250;
		}

		/// <summary>
		/// Creates a High Score Manager with a newScore paramater to check and update the high scores after a game has ended
		/// </summary>
		/// <param name="game">The game context for the HighScoreManager</param>
		/// <param name="newScore">New Score to check against the current high scores</param>
		/// <param name="tablePosition">The postion for the top left point of the display table</param>
		public HighScoreManager(Game game, Score newScore, Vector2 tablePosition) : base(game)
		{
			fileName = "HighScores.txt";
			HighScores = LoadHighScores();
			SortHighScores();
			this.newScore = newScore;
			this.tablePosition = tablePosition + Shared.displayPosShift;
			column1X = 0;
			column2X = column1X + 300;
			column3X = column2X + 250;
		}

		public List<Score> HighScores { get => highScores; set => highScores = value; }

		/// <summary>
		/// Load high scores from a text file into a list of Scores
		/// </summary>
		/// <returns>Returns a List<Score></returns>
		private List<Score> LoadHighScores()
		{
			List<Score> highScores = new List<Score>(); ;

			// Check if the file exists and create an empty .txt file if not
			if (!File.Exists(fileName))
			{
				File.WriteAllText(fileName, string.Empty);
			}

			// Read through each line of the file and extract the player name, levels completed, and time spent
			StreamReader reader = new StreamReader(fileName);
			while (reader.Peek() != -1)
			{

				string str = reader.ReadLine();

				string[] scoreDetails = str.Split('|');

				Score highScore = new Score(scoreDetails[0], int.Parse(scoreDetails[1]), float.Parse(scoreDetails[2]));

				highScores.Add(highScore);
			}

			reader.Close();

			return highScores;
		}

		/// <summary>
		/// Sort the HighScores in order of most levels completed and least amount of time spent
		/// </summary>
		private void SortHighScores()
		{
			highScores.Sort((x, y) =>
			{
				int levelComparison = -x.LevelsCompleted.CompareTo(y.LevelsCompleted);

				return levelComparison != 0 ? levelComparison : x.TimeSpent.CompareTo(y.TimeSpent);
			});
		}

		/// <summary>
		/// Check if the new Score is a high score and add it to the list of high scores
		/// </summary>
		/// <param name="newScore">Score being checked against the current list of scores</param>
		/// <returns>Returns true if the new Score was added to the high scores, or false if it was not added</returns>
		public bool NewScoreIsHighScore(Score newScore)
		{
			// Add score if there are empty high score slots and the player completed at least 1 level
			if (HighScores.Count() < Shared.numberOfHighScoresStored && newScore.LevelsCompleted > 0)
			{
				AddHighScore(newScore);
				return true;
			}

			// Compare the new Score against the currently full list of high scores to see if the new Score beat any of them
			foreach (Score score in HighScores)
			{
				if (newScore.LevelsCompleted > score.LevelsCompleted || (newScore.LevelsCompleted == score.LevelsCompleted && newScore.TimeSpent > score.TimeSpent))
				{
					AddHighScore(newScore);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Add a new high score to the list of high scores and remove the smallest if the list was already full
		/// </summary>
		/// <param name="newHighScore">New high score to be added to the list</param>
		public void AddHighScore(Score newHighScore)
		{
			highScores.Add(newHighScore);

			SortHighScores();

			if (highScores.Count() > Shared.numberOfHighScoresStored)
			{
				highScores.RemoveAt(highScores.Count() - 1);
				highScores.TrimExcess();
			}
		}

		/// <summary>
		/// Save the list of high scores to the .txt file
		/// </summary>
		public void SaveHighScores()
		{
			StreamWriter writer = new StreamWriter(fileName, false);
			foreach (Score score in HighScores)
			{
				writer.WriteLine($"{score.PlayerName}|{score.LevelsCompleted}|{score.TimeSpent}");
			}
			writer.Close();
		}

		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();

			// Table Title
			Shared.sb.DrawString(Shared.smallFont, "High Scores", tablePosition, regularColor);

			// Table header row
			Shared.sb.DrawString(Shared.smallFont, "Name", tablePosition + new Vector2(column1X, Shared.smallFont.LineSpacing), regularColor);
			Shared.sb.DrawString(Shared.smallFont, "Number of Levels", tablePosition + new Vector2(column2X, Shared.smallFont.LineSpacing), regularColor);
			Shared.sb.DrawString(Shared.smallFont, "Time Spent", tablePosition + new Vector2(column3X, Shared.smallFont.LineSpacing), regularColor);

			// Table body rows
			for (int i = 0; i < Shared.numberOfHighScoresStored; i++)
			{
				// set default name and scores
				string name = "___";
				string levels = "0";
				string time = "0.00";

				// If the counter is within the range of the high scores list then update the name and scores to be displayed in the row
				if (HighScores.Count() > i)
				{
					name = HighScores[i].PlayerName;
					levels = HighScores[i].LevelsCompleted.ToString();
					time = HighScores[i].TimeSpent.ToString("0.##");
				}

				// Check if the row to be displayed is the new score to be added and highlight the row text differently
				if (i < HighScores.Count() && HighScores[i] == newScore)
				{
					// If the new Score player name is null or empty then prompt the player to 'Enter Name'
					if (string.IsNullOrEmpty(newScore.PlayerName))
					{
						Shared.sb.DrawString(Shared.smallFont, $"{i + 1}. Enter Name", tablePosition + new Vector2(column1X, Shared.smallFont.LineSpacing * (2 + i)), newScoreColor);
					}
					else
					{
						// If the user has entered a name then diplay it
						Shared.sb.DrawString(Shared.smallFont, $"{i + 1}. {HighScores[i].PlayerName}", tablePosition + new Vector2(column1X, Shared.smallFont.LineSpacing * (2 + i)), newScoreColor);
					}
					Shared.sb.DrawString(Shared.smallFont, $"{levels}", tablePosition + new Vector2(column2X, Shared.smallFont.LineSpacing * (2 + i)), newScoreColor);
					Shared.sb.DrawString(Shared.smallFont, $"{time}", tablePosition + new Vector2(column3X, Shared.smallFont.LineSpacing * (2 + i)), newScoreColor);

				}
				else
				{
					// Draw all rows that are not the new Score with the regular text color
					Shared.sb.DrawString(Shared.smallFont, $"{i + 1}. {name}", tablePosition + new Vector2(column1X, Shared.smallFont.LineSpacing * (2 + i)), regularColor);
					Shared.sb.DrawString(Shared.smallFont, $"{levels}", tablePosition + new Vector2(column2X, Shared.smallFont.LineSpacing * (2 + i)), regularColor);
					Shared.sb.DrawString(Shared.smallFont, $"{time}", tablePosition + new Vector2(column3X, Shared.smallFont.LineSpacing * (2 + i)), regularColor);
				}
			}
			Shared.sb.End();

			base.Draw(gameTime);
		}
	}
}

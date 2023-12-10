/* HelpMap.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.12.4: Created        
 */
using Microsoft.Xna.Framework;
using TheForgottenKnight.MapComponents;
using TiledCS;

namespace TheForgottenKnight
{
    /// <summary>
    /// Represents the Helpmap,  dedicated to providing help and instructions to the player.
    /// </summary>
    public class HelpMap : Map
	{
		private Vector2 bagPosition;
		private Vector2 timerPosition;

		/// <summary>
		/// Initializes a new help map 
		/// </summary>
		/// <param name="game">Game instance</param>
		/// <param name="map">Instance of tiledamp</param>
		/// <param name="bagPosition">postion of item in play bag</param>
		/// <param name="timerPosition">postiion of time on screen</param>
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

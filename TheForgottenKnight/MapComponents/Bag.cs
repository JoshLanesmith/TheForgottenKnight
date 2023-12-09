/* Bag.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Bag class to track items that have been pickup by the player
    /// </summary>
    public class Bag : DrawableGameComponent
	{
		private Map map;
		private List<PickupObject> bagItems;
		private SpriteFont font;
		private Vector2 position;
		private string labelText = "Bag";
		private Color labelColor = Color.White;
		private int displayEdgeBuffer = 32;

		/// <summary>
		/// Creates a bag with zero items in it at the bottom left corner of the map display
		/// </summary>
		/// <param name="game">The game context for the bag</param>
		/// <param name="map">The map context for the bag</param>
		public Bag(Game game, Map map) : base(game)
		{
			this.map = map;
			BagItems = new List<PickupObject>();
			font = Shared.labelFont;
			position = new Vector2(map.TileWidth, Shared.stage.Y - displayEdgeBuffer - font.LineSpacing);
			
			// Initialize the bag as not enabled so that it isn't displayed right away
			this.Enabled = false;
		}

		/// <summary>
		/// Creates a bag with zero items at the position provided
		/// </summary>
		/// <param name="game">The game context for the bag</param>
		/// <param name="map">The map context for the bag</param>
		/// <param name="position">Position to place the bag</param>
		public Bag(Game game, Map map, Vector2 position) : base(game)
		{
			this.map = map;
			BagItems = new List<PickupObject>();
			font = Shared.labelFont;
			this.position = position;

			// Initialize the bag as not enabled so that it isn't displayed right away
			this.Enabled = false;
		}

		public List<PickupObject> BagItems { get => bagItems; set => bagItems = value; }

		/// <summary>
		/// Add an item to the bag
		/// </summary>
		/// <param name="item">PickupObject that the player just picked up</param>
		public void AddItemToBag(PickupObject item)
		{
			BagItems.Add(item);

			// Change the position of the item to display in the bag section of the map
			Vector2 bagItemPostion = new Vector2(position.X + map.TileWidth * (BagItems.Count() - 1), position.Y + font.LineSpacing) / map.MapScaleFactor;
			item.PickupItem(bagItemPostion);

			// Enable the bag if the bag has 1 or more items
			if (BagItems.Count() > 0)
			{
				this.Enabled = true;
			}
		}

		/// <summary>
		/// Reset the bag to empty and not showing
		/// </summary>
		public void ResetBag()
		{
			BagItems.Clear();
			this.Enabled = false;
		}

		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			if (this.Enabled)
			{
				Shared.sb.DrawString(font, labelText, position + Shared.displayPosShift, labelColor);
			}
			Shared.sb.End();

			base.Draw(gameTime);
		}


	}
}

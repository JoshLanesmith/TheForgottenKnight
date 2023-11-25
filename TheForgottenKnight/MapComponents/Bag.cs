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
	/// Bag class to track items that have been pickup by the player
	/// </summary>
	internal class Bag : DrawableGameComponent
	{
		private List<PickupObject> bagItems;
		private SpriteFont font;
		private Vector2 position;
		private string labelText = "Bag";
		private Color labelColor = Color.White;

		/// <summary>
		/// Creates a bag with zero items in it
		/// </summary>
		/// <param name="game"></param>
		public Bag(Game game) : base(game)
		{
			BagItems = new List<PickupObject>();
			font = Shared.labelFont;
			position = new Vector2(16, Shared.stage.Y - 16 - font.LineSpacing);
			
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
			Vector2 bagItemPostion = new Vector2(16 * BagItems.Count(), position.Y + font.LineSpacing) + Shared.displayPosShift;
			item.PickupItem(bagItemPostion);

			// Enable the bag if the bag has 1 or more items
			if (BagItems.Count() > 0)
			{
				this.Enabled = true;
			}
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

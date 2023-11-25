using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.MapComponents
{
	/// <summary>
	/// 
	/// </summary>
	public class PickupObject : AbstractInteractiveObject
	{
		private bool isLevelKey;

		/// <summary>
		/// Create a pickup object
		/// </summary>
		/// <param name="game">The game context for the pickup object</param>
		/// <param name="map">The map level context for the pickup object</param>
		/// <param name="tex">The tileset texture used to draw the pickup object</param>
		/// <param name="width">The width of the pickup object in pixels</param>
		/// <param name="height">The height of the pickup object in pixels</param>
		/// <param name="tilesetRow">The row of the tileset frame to use</param>
		/// <param name="tilesetColumn">The column of the tileset frame to use</param>
		/// <param name="startingX">The X value of the pickup object's starting position</param>
		/// <param name="startingY">The Y value of the pickup object's starting position</param>
		/// <param name="isLevelKey">Boolean identifying the object as the Level Key or not</param>
		public PickupObject(Game game, Map map, Texture2D tex, int width, int height, int tilesetRow, int tilesetColumn, float startingX, float startingY, bool isLevelKey) 
			: base(game, map, tex, width, height, tilesetRow, tilesetColumn, startingX, startingY)
		{
			this.IsLevelKey = isLevelKey;
		}

		public bool IsLevelKey { get => isLevelKey; set => isLevelKey = value; }

		/// <summary>
		/// Update the item's postion when picked up to display in the bag section
		/// </summary>
		/// <param name="bagPosition">Position to place the item in the bag</param>
		public void PickupItem(Vector2 bagPosition)
		{
			position = bagPosition;
		}
	}
}

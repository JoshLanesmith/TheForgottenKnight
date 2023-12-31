﻿/* BaseInteractiveObject.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.12.04: Created        
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Abstract class used for all interactive map objects that can move and be collided with
    /// </summary>
    public abstract class BaseInteractiveObject : DrawableGameComponent
	{
		protected int tiledEditorObjectOriginAdjustment;
		protected Map map;
		protected Texture2D tex;
		protected float startingX;
		protected float startingY;
		protected int originalTilesetRow;
		protected int originalTilesetColumn;
		protected int width;
		protected int height;
		protected int tilesetRow;
		protected int tilesetColumn;
		protected Rectangle tilesetRec;
		protected Vector2 position;

		/// <summary>
		/// Defaule constructor creates and empty object
		/// </summary>
		/// <param name="game">The game context for the object</param>
		public BaseInteractiveObject(Game game) : base(game)
		{

		}

		/// <summary>
		/// Create an object with a starting position and texture details for drawing
		/// </summary>
		/// <param name="game">The game context for the object</param>
		/// <param name="map">The map level context for the object</param>
		/// <param name="tex">The tileset texture used to draw the object</param>
		/// <param name="width">The width of the object in pixels</param>
		/// <param name="height">The height of the object in pixels</param>
		/// <param name="tilesetRow">The row of the tileset frame to use</param>
		/// <param name="tilesetColumn">The column of the tileset frame to use</param>
		/// <param name="startingX">The X value of the objects starting position</param>
		/// <param name="startingY">The Y value of the objects starting position</param>
		public BaseInteractiveObject(Game game, Map map, Texture2D tex,
			 int width, int height, int tilesetRow, int tilesetColumn, float startingX, float startingY) : base(game)
		{
			// Account for TiledObject teating the tile origin as the bottom left corner of the tile
			tiledEditorObjectOriginAdjustment = -height;

			// Set data required to reset the object within the map
			this.startingX = startingX;
			this.startingY = startingY + tiledEditorObjectOriginAdjustment;
			originalTilesetRow = tilesetRow;
			originalTilesetColumn = tilesetColumn;
			
			this.map = map;
			this.tex = tex;
			this.width = width;
			this.height = height;
			this.tilesetRow = tilesetRow;
			this.tilesetColumn = tilesetColumn;
            position = new Vector2(this.startingX, this.startingY);
			tilesetRec = new Rectangle(width * tilesetColumn, height * tilesetRow, width, height);
		}

		/// <summary>
		/// Get the rctangle bounds of the object for collision detection
		/// </summary>s
		/// <returns>Return a Rectangle as the current bounds of the object</returns>
		public Rectangle GetBounds()
		{
			return new Rectangle((int)position.X, (int)position.Y, width - 1, height - 1);
		}

		/// <summary>
		/// Reset the object to its original starting position
		/// </summary>
		public virtual void ResetPosition()
		{
			tilesetRec.X = width * originalTilesetColumn;
			tilesetRec.Y = height * originalTilesetRow;
			position.X = startingX;
			position.Y = startingY;
		}

		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			Shared.sb.Draw(tex, position * map.MapScaleFactor + Shared.displayPosShift, tilesetRec, Color.White, 0f, new Vector2(), map.MapScaleFactor, SpriteEffects.None, 0);

			Shared.sb.End();
			base.Draw(gameTime);
		}
	}
}

/* FloorLayer.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledCS;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Floor layer for the map defining visual of the floor with no collision detection
    /// </summary>
    public class FloorLayer : DrawableGameComponent
    {
        protected Map map;
        protected List<Tileset> utilizedTilesets;

        protected TiledLayer tileLayer;

        protected int mapRows;
        protected int mapColumns;

        protected int tileWidth;
        protected int tileHeight;


        /// <summary>
        /// Create floor layer for the map
        /// </summary>
        /// <param name="game">The game context for the collision layer</param>
        /// <param name="map">The map context for the collision layer</param>
        /// <param name="utilizedTilesets">List of Tilesets used in the map</param>
        /// <param name="tileLayer">The tiled layer data from the .tmx file</param>
        /// <param name="mapRows">Number of rows in the map</param>
        /// <param name="mapColumns">Number of columns in the map</param>
        /// <param name="tileWidth">Width of each tile</param>
        /// <param name="tileHeight">Height of each tile</param>
        public FloorLayer(Game game, Map map, List<Tileset> utilizedTilesets, TiledLayer tileLayer,
			int mapRows, int mapColumns, int tileWidth, int tileHeight) : base(game)
		{
			this.map = map;
			this.utilizedTilesets = utilizedTilesets;
			this.tileLayer = tileLayer;
			this.mapRows = mapRows;
			this.mapColumns = mapColumns;
			this.tileWidth = tileWidth;
			this.tileHeight = tileHeight;
		}

		public override void Draw(GameTime gameTime)
        {
			Shared.sb.Begin();
			// Loop through the array of data for each tile in the layer
			for (int i = 0; i < tileLayer.data.Length; i++)
            {
				// Isolate the gid for the tile, used to identify the specific tileset and tile used to draw the object
				int gid = tileLayer.data[i];
                
                Tileset usedTileset = new Tileset();
                int tilesetFirstgid = 0;

                // If empty tile, do nothing
                if (gid == 0)
                {
                    continue;
                }

				// Loop through tilesets used in the map and use the gid to identify which one is used for this tile
				for (int j = 0; j < utilizedTilesets.Count; j++)
                {
					// if the tile gid is in the range of the tileset's assigned gids then set the tileset as the usedTilesed
					if (gid >= utilizedTilesets[j].FirstGid && gid <= utilizedTilesets[j].LastGid)
                    {
						usedTileset = utilizedTilesets[j];
                        tilesetFirstgid = utilizedTilesets[j].FirstGid;
                    }
                }

				// Identify the specific frame of the tileset used to draw the object
				int tileFrame = gid - tilesetFirstgid;

				// Calculate the column and row of the tile frame within the tileset texture
				int column = tileFrame % usedTileset.TiledTileset.Columns;
                int row = (int)Math.Floor(tileFrame / (double)usedTileset.TiledTileset.Columns);

				// Calculate the X and Y positions for the object
				float x = i % mapRows * tileWidth;
                float y = (float)Math.Floor(i / (double)mapRows) * tileHeight;
                Vector2 position = new Vector2(x * map.MapScaleFactor, y * map.MapScaleFactor);

                // Define the rectangle to isolate the frame in the tileset used to draw the tile
                Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

				//Shared.sb.Draw(usedTileset.Tex, new Rectangle((int)(x + Shared.displayStartPoint.X), (int)(y + Shared.displayStartPoint.Y), tileWidth, tileHeight), tilesetRec, Color.White);
				Shared.sb.Draw(usedTileset.Tex, position + Shared.displayPosShift, tilesetRec, Color.White, 0f, new Vector2(), map.MapScaleFactor, SpriteEffects.None, 0f);

            }
			Shared.sb.End();

            base.Draw(gameTime);
        }
    }
}

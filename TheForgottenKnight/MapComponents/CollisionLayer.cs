/* CollisionLayer.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TiledCS;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Collision layer for the map defining stationary objects with collision boundaries
    /// </summary>
    public class CollisionLayer : FloorLayer
    {
        private List<Rectangle> collisionObjects;

		/// <summary>
		/// Create collision layer for the map
		/// </summary>
		/// <param name="game">The game context for the collision layer</param>
        /// <param name="map">The map context for the collision layer</param>
		/// <param name="utilizedTilesets">List of Tilesets used in the map</param>
		/// <param name="tileLayer">The tiled layer data from the .tmx file</param>
		/// <param name="mapRows">Number of rows in the map</param>
		/// <param name="mapColumns">Number of columns in the map</param>
		/// <param name="tileWidth">Width of each tile</param>
		/// <param name="tileHeight">Height of each tile</param>
		public CollisionLayer(Game game, Map map, List<Tileset> utilizedTilesets, TiledLayer tileLayer,
            int mapRows, int mapColumns, int tileWidth, int tileHeight) :
            base(game, map, utilizedTilesets, tileLayer, mapRows, mapColumns, tileWidth, tileHeight)
        {
            collisionObjects = LoadCollisionObjects();
        }

        public List<Rectangle> CollisionObjects { get => collisionObjects; }

        /// <summary>
        /// Load all of the boundaries for the collision objects in this layer
        /// </summary>
        /// <returns>Returns a List<Rectangles></returns>
        private List<Rectangle> LoadCollisionObjects()
        {
            List<Rectangle> collisionObjects = new List<Rectangle>();

            // Loop through the array of data for each tile in the layer
            for (int i = 0; i < tileLayer.data.Length; i++)
            {
                // If empty tile, do nothing
                if (tileLayer.data[i] == 0)
                {
                    continue;
                }

                // Calculate the X and Y positions for the object
                float x = i % mapRows * tileWidth;
                float y = (float)Math.Floor(i / (double)mapRows) * tileHeight;

                // Create the object's Rectagle and add it to the list of collision objects
                Rectangle collisionObject = new Rectangle((int)x, (int)y, tileWidth, tileHeight);
                collisionObjects.Add(collisionObject);
            }

            return collisionObjects;
        }
    }


}

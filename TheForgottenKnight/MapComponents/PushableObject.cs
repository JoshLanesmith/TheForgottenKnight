/* PushableObject.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Represents a pushable object in the game.
    /// </summary>
    public class PushableObject : BaseInteractiveObject
	{
		private float scale = 1;

		private List<CollisionLayer> collisionLayers;
		private List<PickupObject> pickupObjects;
		private List<Door> doors;



		/// <summary>
		/// Create a pushable object
		/// </summary>
		/// <param name="game">The game context for the pushable object</param>
		/// <param name="map">The map level context for the pushable object</param>
		/// <param name="tex">The tileset texture used to draw the pushable object</param>
		/// <param name="width">The width of the pushable object in pixels</param>
		/// <param name="height">The height of the pushable object in pixels</param>
		/// <param name="tilesetRow">The row of the tileset frame to use</param>
		/// <param name="tilesetColumn">The column of the tileset frame to use</param>
		/// <param name="startingX">The X value of the pushable object's starting position</param>
		/// <param name="startingY">The Y value of the pushable object's starting position</param>
		public PushableObject(Game game, Map map, Texture2D tex, int width, int height, int tilesetRow, int tilesetColumn, float startingX, float startingY)
			: base(game, map, tex, width, height, tilesetRow, tilesetColumn, startingX, startingY)
		{
			collisionLayers = map.CollisionLayers;
			pickupObjects = map.PickupObjects;
			doors = map.Doors;
		}

		/// <summary>
		/// Update the object's position when pushed
		/// </summary>
		/// <param name="xMovement">Movement caused on the x access by the push</param>
		/// <param name="yMovement">Movement caused on the x access by the push</param>
		/// <param name="hitObject">Out paramater indicating if the object collided with another object when pushed</param>
		public void PushObject(float xMovement, float yMovement, out bool hitObject)
		{
			hitObject = false;

			Vector2 initPos = position;

			position.X += xMovement;
			position.Y += yMovement;

			if (IsColliding())
			{
				position = initPos;
				hitObject = true;

			}
		}

		/// <summary>
		/// Check if object is colliding with any other collision objects
		/// </summary>
		/// <returns>Return true if it is colliding with another object and false if it is not colliding</returns>
		private bool IsColliding()
		{
			foreach (CollisionLayer layer in collisionLayers)
			{
				foreach (Rectangle rectangle in layer.CollisionObjects)
				{
					if (rectangle.Intersects(GetBounds()))
					{
						return true;
					}
				}
			}
			foreach (PushableObject item in map.PushableObjects)
			{
				if (item != this && item.GetBounds().Intersects(GetBounds()))
				{
					return true;
				}
			}
			foreach (PickupObject item in pickupObjects)
			{
				if (item.GetBounds().Intersects(GetBounds()))
				{
					return true;
				}
			}
			foreach (Door door in doors)
			{
				if (door.GetBounds().Intersects(GetBounds()))
				{
					return true;
				}
			}


			return false;
		}
	}
}

﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TheForgottenKnight.MapComponents;
using TiledCS;

namespace TheForgottenKnight
{
	/// <summary>
	/// Enum to track if a level is currently running, won, or lost.
	/// </summary>
	public enum LevelStatus
	{
		Running,
		Won,
		Lost
	}

	/// <summary>
	/// Map class used to create all map objects relating to each level
	/// </summary>
	public class Map : DrawableGameComponent
    {
		private TiledMap map;
		private List<Tileset> tilesets;
		private List<FloorLayer> floorLayers;
		private List<CollisionLayer> collisionLayers;
		private List<PushableObject> pushableObjects;
		private List<PickupObject> pickupObjects;
		private List<Door> doors;
		private PlayerObject playerObject;
		private Bag bag;
		private PickupObject? levelKey;
		private Door levelCompleteDoor;
		private float timerStartTime = 50;
		private CountDownTimer countDownTimer;
		private LevelStatus levelStatus;
		private LevelStatus previousLevleStatus;
		private int tileWidth;
		private int tileHeight;
		private float mapScaleFactor;

		public List<GameComponent> Components { get; set; }
		public List<CollisionLayer> CollisionLayers { get => collisionLayers; set => collisionLayers = value; }
		public List<PushableObject> PushableObjects { get => pushableObjects; set => pushableObjects = value; }
		public List<PickupObject> PickupObjects { get => pickupObjects; set => pickupObjects = value; }
		public List<Door> Doors { get => doors; set => doors = value; }
		internal Bag Bag { get => bag; set => bag = value; }
		public PickupObject LevelKey { get => levelKey; set => levelKey = value; }
		public LevelStatus CurrentLevelStatus { get => levelStatus; set => levelStatus = value; }
		public LevelStatus PreviousLevleStatus { get => previousLevleStatus; set => previousLevleStatus = value; }
		public float TimerStartTime { get => timerStartTime; set => timerStartTime = value; }
		public CountDownTimer CountDownTimer { get => countDownTimer; set => countDownTimer = value; }
		public float MapScaleFactor { get => mapScaleFactor; set => mapScaleFactor = value; }

		/// <summary>
		/// Create the map level based on the TiledMap being loaded up
		/// </summary>
		/// <param name="game">The game context for the map</param>
		/// <param name="map">The tiled map with the data read from a .tmx file</param>
		public Map(Game game, TiledMap map) : base(game)
		{
			Components = new List<GameComponent>();
			
			// Set general map details
			this.map = map;
			tileWidth = map.TileWidth;
			tileHeight = map.TileHeight;
			mapScaleFactor = Shared.gameDisplaySize.X / (float)(map.Width * map.TileWidth);
			Debug.WriteLine(mapScaleFactor);

			// Load/set all map components
			tilesets = LoadTilesets();
			floorLayers = LoadFloorLayers();
			collisionLayers = LoadCollisionLayer();
			pushableObjects = LoadPushableObjects();
			pickupObjects = LoadPickupObjects();
			doors = LoadDoors();
			playerObject = LoadPlayerObject();
			bag = new Bag(game);
			Components.Add(bag);
			
			// Setup properties used to track the level status and level completion
			levelCompleteDoor = new Door(game);
			TimerStartTime = float.Parse(map.Properties[0].value);
			CountDownTimer = new CountDownTimer(game, this, TimerStartTime);
			CurrentLevelStatus = LevelStatus.Running;
			PreviousLevleStatus = LevelStatus.Running;
			Components.Add(CountDownTimer);

		}

		/// <summary>
		/// Load all tilesets used in the current map based on the .tmx file
		/// </summary>
		/// <returns>Returns a List<Tileset> to be used by other map components for drawing</returns>
		private List<Tileset> LoadTilesets()
		{
			List<Tileset> tilesets = new List<Tileset>();

			// Read through the list of tilesets from the map
			for (int i = 0; i < map.Tilesets.Length; i++)
			{
				// Read the tileset .tsx file
				FileStream reader = new FileStream(Game.Content.RootDirectory + $"\\maps\\{map.Tilesets[i].source}", FileMode.Open, FileAccess.Read);
				TiledTileset tiledTileset = new TiledTileset(reader);
				
				// Load the image used for the tileset
				Texture2D tex = Game.Content.Load<Texture2D>($"maps/tilesets/{tiledTileset.Name}");

				// Create a Tileset object to group necessary data together
				Tileset tileset = new Tileset(tiledTileset, tex, map.Tilesets[i].firstgid);

				tilesets.Add(tileset);
				reader.Close();
			}

			return tilesets;
		}

		/// <summary>
		/// Load the floor layers for the map based on the .tmx file
		/// </summary>
		/// <returns>Returns a List<FloorLayer></returns>
		private List<FloorLayer> LoadFloorLayers()
		{
			List<FloorLayer> floorLayers = new List<FloorLayer>();

			foreach (TiledGroup group in map.Groups)
			{
				if (group.name == "floorLayers")
				{
					// Loop through all layers in the 'floorLayers' group to create the FloorLayer object and add it to the map components
					foreach (TiledLayer layer in group.layers)
					{
						FloorLayer newFloorLayer = new FloorLayer(Game,this, tilesets, layer,
							map.Width, map.Height, map.TileWidth, map.TileHeight);

						floorLayers.Add(newFloorLayer);
						Components.Add(newFloorLayer);
					}
				}
			}

			return floorLayers;
		}

		/// <summary>
		/// Load the collision layers for the map based on the .tmx file
		/// </summary>
		/// <returns>Returns a List<CollisionLayer></returns>
		private List<CollisionLayer> LoadCollisionLayer()
		{
			List<CollisionLayer> collisionLayers = new List<CollisionLayer>();

			foreach (TiledGroup group in map.Groups)
			{
				if (group.name == "collisionLayers")
				{
					
					// Loop through all layers in the 'collisionLayers' group to create the collisionLayer object and add it to the map components
					foreach (TiledLayer layer in group.layers)
					{
						CollisionLayer newCollisionLayer = new CollisionLayer(Game, this, tilesets, layer,
							map.Width, map.Height, map.TileWidth, map.TileHeight);

						collisionLayers.Add(newCollisionLayer);
						Components.Add(newCollisionLayer);
					}
				}
			}
			return collisionLayers;
		}

		/// <summary>
		/// Load the pushable objects for the map based on the .tmx file 
		/// </summary>
		/// <returns>Returns a List<PushableObject></returns>
		private List<PushableObject> LoadPushableObjects()
		{
			List<PushableObject> pushableObjects = new List<PushableObject>();

			foreach (TiledGroup group in map.Groups)
			{
				if (group.name == "pushObjectLayers")
				{
					// Loop through all layers in the 'pushObjectLayers' group to create the Puahable Objects and add it to the map components
					foreach (TiledLayer layer in group.layers)
					{
						// Loop through each tile object in the object layer
						foreach (TiledObject tile in layer.objects)
						{
							// Isolate the gid for the tile, used to identify the specific tileset and tile used to draw the object
							int gid = tile.gid;

							Tileset usedTileset = new Tileset();
							int tilesetFirstGid = 0;

							// If empty tile, do nothing
							if (gid == 0)
							{
								continue;
							}

							// Loop through tilesets used in the map and use the gid to identify which one is used for this tile
							for (int j = 0; j < tilesets.Count; j++)
							{
								// if the tile gid is in the range of the tileset's assigned gids then set the tileset as the usedTilesed
								if (gid >= tilesets[j].FirstGid && gid <= tilesets[j].LastGid)
								{
									usedTileset = tilesets[j];
									tilesetFirstGid = tilesets[j].FirstGid;
									break;
								}
							}
							
							// Identify the specific frame of the tileset used to draw the object
							int tileFrame = gid - tilesetFirstGid;

							// Calculate the column and row of the tile frame within the tileset texture
							int column = tileFrame % usedTileset.TiledTileset.Columns;
							int row = (int)Math.Floor((double)tileFrame / (double)usedTileset.TiledTileset.Columns);

							// Create the pushable object and add it to the list of objects and to the map components
							PushableObject newBox = new PushableObject(Game, this, usedTileset.Tex, (int)tile.width, (int)tile.height, row, column, tile.x, tile.y);
							this.Components.Add(newBox);
							pushableObjects.Add(newBox);

						}
					}
				}
			}

			return pushableObjects;
		}

		/// <summary>
		/// Load the pickup objects for the map based on the .tmx file 
		/// </summary>
		/// <returns>Returns a List<PickupObject></returns>
		private List<PickupObject> LoadPickupObjects()
		{
			List<PickupObject> pickupObjects = new List<PickupObject>();

			foreach (TiledGroup group in map.Groups)
			{
				if (group.name == "pickupObjectLayers")
				{
					// Loop through all layers in the 'pickupObjectLayers' group to create the Pickup Objects and add it to the map components
					foreach (TiledLayer layer in group.layers)
					{
						// Loop through each tile object in the object layer
						foreach (TiledObject tile in layer.objects)
						{
							// Isolate the gid for the tile, used to identify the specific tileset and tile used to draw the object
							int gid = tile.gid;
							
							// Set the object as not the Level Key by default
							bool isLevelKey = false;

							Tileset usedTileset = new Tileset();
							int tilesetFirstgid = 0;

							// Loop through the tile propties and to check if it is the Level Key
							foreach (TiledProperty property in tile.properties)
							{
								if (property.name == "isLevelKey")
								{
									isLevelKey = bool.Parse(property.value);
								}
							}

							// If empty tile, do nothing
							if (gid == 0)
							{
								continue;
							}

							// Loop through tilesets used in the map and use the gid to identify which one is used for this tile
							for (int j = 0; j < tilesets.Count; j++)
							{
								// if the tile gid is in the range of the tileset's assigned gids then set the tileset as the usedTilesed
								if (gid >= tilesets[j].FirstGid && gid <= tilesets[j].LastGid)
								{
									usedTileset = tilesets[j];
									tilesetFirstgid = tilesets[j].FirstGid;
									continue;
								}
							}

							// Identify the specific frame of the tileset used to draw the object
							int tileFrame = gid - tilesetFirstgid;

							// Calculate the column and row of the tile frame within the tileset texture
							int column = tileFrame % usedTileset.TiledTileset.Columns;
							int row = (int)Math.Floor((double)tileFrame / (double)usedTileset.TiledTileset.Columns);

							// Create the pickup object and add it to the list of objects and to the map components
							PickupObject newItem = new PickupObject(Game, this, usedTileset.Tex, (int)tile.width, (int)tile.height, row, column, tile.x, tile.y, isLevelKey);

							if (isLevelKey)
							{
								// If the new item is the level key then set it to the map field
								LevelKey = newItem;
							}

							this.Components.Add(newItem);
							pickupObjects.Add(newItem);

						}
					}
				}
			}

			return pickupObjects;
		}

		/// <summary>
		/// Load the door objects for the map based on the .tmx file 
		/// </summary>
		/// <returns>Returns a List<Door></returns>
		private List<Door> LoadDoors()
		{
			List<Door> doors = new List<Door>();

			foreach (TiledGroup group in map.Groups)
			{
				if (group.name == "doorLayers")
				{
					// Loop through all layers in the 'doorLayers' group to create the Doors and add it to the map components
					foreach (TiledLayer layer in group.layers)
					{
						// Loop through each tile object in the object layer
						foreach (TiledObject tile in layer.objects)
						{
							// Isolate the gid for the tile, used to identify the specific tileset and tile used to draw the object
							int gid = tile.gid;
							
							// Set the object as not the Level Complete Door by default
							bool isLevelCompleteDoor = false;
							
							Tileset usedTileset = new Tileset();
							int tilesetFirstgid = 0;

							// Loop through the tile propties and to check if it is the Level Complete Door
							foreach (TiledProperty property in tile.properties)
							{
								if (property.name == "isLevelCompleteDoor")
								{
									isLevelCompleteDoor = bool.Parse(property.value);
								}
							}

							// If empty tile, do nothing
							if (gid == 0)
							{
								continue;
							}

							// Loop through tilesets used in the map and use the gid to identify which one is used for this tile
							for (int j = 0; j < tilesets.Count; j++)
							{
								// if the tile gid is in the range of the tileset's assigned gids then set the tileset as the usedTilesed
								if (gid >= tilesets[j].FirstGid && gid <= tilesets[j].LastGid)
								{
									usedTileset = tilesets[j];
									tilesetFirstgid = tilesets[j].FirstGid;
									continue;
								}
							}

							// Identify the specific frame of the tileset used to draw the object
							int tileFrame = gid - tilesetFirstgid;


							// Calculate the column and row of the tile frame within the tileset texture
							int column = tileFrame % usedTileset.TiledTileset.Columns;
							int row = (int)Math.Floor((double)tileFrame / (double)usedTileset.TiledTileset.Columns);

							// Create the door object and add it to the list of objects and to the map components
							Door newDoor = new Door(Game, this, usedTileset.Tex, (int)tile.width, (int)tile.height, row, column, tile.x, tile.y, isLevelCompleteDoor);

							if (isLevelCompleteDoor)
							{
								// If the new item is the level complete door then set it to the map field
								levelCompleteDoor = newDoor;
							}

							this.Components.Add(newDoor);
							doors.Add(newDoor);

						}
					}
				}
			}

			return doors;
		}

		/// <summary>
		/// Load the player object for the map based on the .tmx file
		/// </summary>
		/// <returns>Returns a PlayerObject</returns>
		private PlayerObject LoadPlayerObject()
		{
			// Load the texture, define the initial position, create the player object, and add it to the map components
			Texture2D ballTex = Game.Content.Load<Texture2D>("images/Ball");

			Vector2 playerStartPosition = new Vector2();
			if (map.Layers.Count() == 1 && map.Layers[0].objects.Count() == 1)
			{
				
				playerStartPosition.X = map.Layers[0].objects[0].x;
				playerStartPosition.Y = map.Layers[0].objects[0].y;
			}
			else
			{
				playerStartPosition.X = Shared.stage.X / 2 - ballTex.Width / 2;
				playerStartPosition.Y = Shared.stage.Y / 2 - ballTex.Height / 2;
			}

			PlayerObject ball = new PlayerObject(Game, this, ballTex, playerStartPosition);
			this.Components.Add(ball);

			return ball;
		}

		public override void Update(GameTime gameTime)
		{
			// Call the update function for all map components
			foreach (GameComponent item in Components)
			{
				if (item.Enabled)
				{
					item.Update(gameTime);
				}
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			// Call the draw function for all drawable map components
			foreach (GameComponent item in Components)
			{
				if (item is DrawableGameComponent)
				{
					DrawableGameComponent comp = (DrawableGameComponent)item;
					if (comp.Visible)
					{
						comp.Draw(gameTime);
					}
				}
			}

			base.Draw(gameTime);
		}

	}
}

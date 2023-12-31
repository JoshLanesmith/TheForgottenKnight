﻿/* PlayerObject.cs
 * The Forgotten Knight
 *	Revision History
 *			Josh Lanesmith, 2023.11.20: Created		
 *			Miles Purvis, 2023.11.20: Added Animation			
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Player object to manage the movement of the player through the map.
    /// </summary>
    public class PlayerObject : DrawableGameComponent
	{
		private Map map;
		private Texture2D[] animationSheet;
		private float collisionBoxScale = 0.95f;

		//List Components
		private List<CollisionLayer> collisionLayers;
		private List<PushableObject> pushableObjects;
		private List<PickupObject> pickupObjects;
		private List<Door> doors;

		//Movement
		public Vector2 position;
		public Vector2 originalPosition;
		private float moveSpeed = 1.5f;

		//Animations
		private Animator[] playerIdle;
		private Animator[] playerWalk;
		private Animator currentAnimation;
		private Animator currentIdle;
		private int tileWidth;
		private int tileHeight;

		//AnimationSheets
		private Texture2D playerIdleSheet;
		private Texture2D playerWalkSheet;

		//SFX
		private SoundEffect[] soundEffects;
		private float elapsedTime = 0f;
		private float interval = 0.2f; // 1 second interval
		private Random random;
		private SoundEffect boxPush;
		private SoundEffect doorOpen;
		private SoundEffect pickUpItem;



		/// <summary>
		/// Player Object constructor - handles the player character.
		/// </summary>
		/// <param name="game">Game from DrawableGameComponent inheritance to load sound effects for the player.</param>
		/// <param name="map">The map associated with the player.</param>
		/// <param name="animationSheet">Animation Sheet.</param>
		/// <param name="position">Position of the player on the screen.</param>
		public PlayerObject(Game game, Map map, Texture2D[] animationSheet, Vector2 position) : base(game)
		{
			this.animationSheet = animationSheet;
			this.position = position;
			originalPosition = this.position;
			this.map = map;
			collisionLayers = map.CollisionLayers;
			pushableObjects = map.PushableObjects;
			pickupObjects = map.PickupObjects;
			doors = map.Doors;

			#region Animation
			//Create new animator
			playerIdle = new Animator[4];
			playerWalk = new Animator[4];
			tileWidth = 16;
			tileHeight = 16;

			//Assign Sheets
			playerIdleSheet = animationSheet[0];
			playerWalkSheet = animationSheet[1];

			//Idling 
			playerIdle[0] = new Animator(map, playerIdleSheet, 0, tileWidth, tileHeight); //Down
			playerIdle[1] = new Animator(map, playerIdleSheet, 1, tileWidth, tileHeight); //Up
			playerIdle[2] = new Animator(map, playerIdleSheet, 2, tileWidth, tileHeight); //Left
			playerIdle[3] = new Animator(map, playerIdleSheet, 3, tileWidth, tileHeight); //Right

			//Walking
			playerWalk[0] = new Animator(map, playerWalkSheet, 0, tileWidth, tileHeight);//Down
			playerWalk[1] = new Animator(map, playerWalkSheet, 1, tileWidth, tileHeight); //Up
			playerWalk[2] = new Animator(map, playerWalkSheet, 2, tileWidth, tileHeight); //Left
			playerWalk[3] = new Animator(map, playerWalkSheet, 3, tileWidth, tileHeight); //Right

			//Default Idle
			currentIdle = playerIdle[0];
			currentAnimation = currentIdle;
			#endregion

			#region SFX

			//Walking
			SoundEffect walk1 = game.Content.Load<SoundEffect>("sfx/player-sfx/walk_1");
			SoundEffect walk2 = game.Content.Load<SoundEffect>("sfx/player-sfx/walk_2");
			SoundEffect walk3 = game.Content.Load<SoundEffect>("sfx/player-sfx/walk_3");

			soundEffects = new SoundEffect[] { walk1, walk2, walk3 };

			//PushingBox
			boxPush = game.Content.Load<SoundEffect>("sfx/object-sfx/crate_push_1");
			doorOpen = game.Content.Load<SoundEffect>("sfx/object-sfx/door_open");

			//Pick Up Item
			pickUpItem = game.Content.Load<SoundEffect>("sfx/object-sfx/pickup_key");

			#endregion

		}

		/// <summary>
		/// Updates the player's position and handles various game logic.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Update(GameTime gameTime)
		{
			bool isWalkingSFX = false;
			Vector2 initPos = position;

			//Idle
			currentAnimation = currentIdle;

			KeyboardState keyboardstate = Keyboard.GetState();
			if (keyboardstate.IsKeyDown(Keys.D))//Move right
			{
				position.X += moveSpeed;
				currentAnimation = playerWalk[3];
				currentIdle = playerIdle[3];
				isWalkingSFX = true;

				if (IsColliding())
				{
					position.X = initPos.X;
				}
				if (IsPushing(out PushableObject pushedItem, gameTime))
				{
					pushedItem.PushObject(moveSpeed, 0, out bool hitObject);

					if (hitObject)
					{
						position.X = initPos.X;
					}
				}

			}
			if (keyboardstate.IsKeyDown(Keys.A))//Move right
			{
				position.X -= moveSpeed;
				currentAnimation = playerWalk[2];
				currentIdle = playerIdle[2];
				isWalkingSFX = true;

				if (IsColliding())
				{
					position.X = initPos.X;
				}
				if (IsPushing(out PushableObject pushedItem, gameTime))
				{
					pushedItem.PushObject(-moveSpeed, 0, out bool hitObject);

					if (hitObject)
					{
						position.X = initPos.X;
					}
				}
			}
			if (keyboardstate.IsKeyDown(Keys.W))//Move right
			{
				position.Y -= moveSpeed;
				currentAnimation = playerWalk[1];
				currentIdle = playerIdle[1];
				isWalkingSFX = true;


				if (IsColliding())
				{
					position.Y = initPos.Y;
				}
				if (IsPushing(out PushableObject pushedItem, gameTime))
				{
					pushedItem.PushObject(0, -moveSpeed, out bool hitObject);

					if (hitObject)
					{
						position.Y = initPos.Y;
					}
				}
			}
			if (keyboardstate.IsKeyDown(Keys.S))//Move right
			{
				position.Y += moveSpeed;
				currentAnimation = playerWalk[0];
				currentIdle = playerIdle[0];
				isWalkingSFX = true;

				if (IsColliding())
				{
					position.Y = initPos.Y;
				}
				if (IsPushing(out PushableObject pushedItem, gameTime))
				{
					pushedItem.PushObject(0, moveSpeed, out bool hitObject);

					if (hitObject)
					{
						position.Y = initPos.Y;
					}
				}
			}

			elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Check if time has passed
			if (elapsedTime >= interval)
			{
				if (isWalkingSFX)
				{
					PlayWalkSound();
				}

				// Reset elapsed time
				elapsedTime = 0f;
			}

			if (IsPickingUpItem(out PickupObject pickedupItem))
			{
				map.Bag.AddItemToBag(pickedupItem);
			}


			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the player's current animation.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Draw(GameTime gameTime)
		{
			Shared.sb.Begin();
			currentAnimation.Animate(Shared.sb, gameTime, position);
			Shared.sb.End();
			base.Draw(gameTime);
		}

		/// <summary>
		/// Gets the bounding rectangle for collision detection.
		/// </summary>
		/// <returns>Rectangle representing the player's bounding box.</returns>
		public Rectangle GetBounds()
		{
			return new Rectangle((int)position.X, (int)position.Y, (int)(tileWidth * collisionBoxScale), (int)(tileHeight * collisionBoxScale));
		}

		/// <summary>
		/// Checks if the player is colliding with any collision objects or doors in the map.
		/// </summary>
		/// <returns>True if the player is colliding with any collision objects or locked doors, otherwise false.</returns>
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

			foreach (Door door in doors)
			{
				if (door.GetBounds().Intersects(GetBounds()))
				{
					if (!door.IsUnlocked && map.Bag.BagItems.Contains(map.LevelKey))
					{
						foreach (Door lockedDoor in doors)
						{
							lockedDoor.UnlockDoor();

						}
						doorOpen.Play();
					}
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the player is pushing a pushable object and plays a sound effect if a push occurs.
		/// </summary>
		/// <param name="pushedItem">The pushable object being pushed (output parameter).</param>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		/// <returns>True if the player is pushing a pushable object, otherwise false.</returns>
		private bool IsPushing(out PushableObject pushedItem, GameTime gameTime)
		{
			pushedItem = null;

			foreach (PushableObject item in pushableObjects)
			{
				if (item.GetBounds().Intersects(GetBounds()))
				{
					elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

					// Check if time has passed
					if (elapsedTime >= interval)
					{
						boxPush.Play();
					}

					pushedItem = item;
					return true;

				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the player is picking up a pickup object and plays a sound effect if a pickup occurs.
		/// </summary>
		/// <param name="pickedupItem">The pickup object being picked up (output parameter).</param>
		/// <returns>True if the player is picking up a pickup object, otherwise false.</returns>
		private bool IsPickingUpItem(out PickupObject pickedupItem)
		{
			pickedupItem = null;

			foreach (PickupObject item in pickupObjects)
			{
				if (item.GetBounds().Intersects(GetBounds()))
				{
					pickedupItem = item;
					pickUpItem.Play();
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Plays a random walking sound from the array of sound effects.
		/// </summary>
		private void PlayWalkSound()
		{
			random = new Random();
			int randomIndex = random.Next(0, soundEffects.Length);

			soundEffects[randomIndex]?.Play();
		}

		/// <summary>
		/// Resets the player's position to the original position.
		/// </summary>
		public void ResetPlayer()
		{
			position = originalPosition;
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.MapComponents
{
    /// <summary>
    /// Player object to manage the movement of the player through the map
    /// </summary>
    public class PlayerObject : DrawableGameComponent
    {
        private Map map;
        private Texture2D[] animationSheet;
        private float scale = 0.25f;

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

		//AnimationSheets
		private Texture2D playerIdleSheet;
		private Texture2D playerWalkSheet;

		//SFX
		private SoundEffect[] soundEffects;
		private float elapsedTime = 0f;
		private float interval = 0.2f; // 1 second interval
		private Random random;


		/// <summary>
		/// Player Object constuctor - handles the player character
		/// </summary>
		/// <param name="game">game from drawablegame component inheritance to load soundeffects for player</param>
		/// <param name="map">**TODO**</param>
		/// <param name="animationSheet">Animation Sheet</param>
		/// <param name="position">position of player on screen</param>
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

			//Assign Sheets
			playerIdleSheet = animationSheet[0];
			playerWalkSheet = animationSheet[1];

			//Idling 
			playerIdle[0] = new Animator(playerIdleSheet, 0, 16, 16); //Down
			playerIdle[1] = new Animator(playerIdleSheet, 1, 16, 16); //Up
			playerIdle[2] = new Animator(playerIdleSheet, 2, 16, 16); //Left
			playerIdle[3] = new Animator(playerIdleSheet, 3, 16, 16); //Right

			//Walking
			playerWalk[0] = new Animator(playerWalkSheet, 0, 16, 16);//Down
			playerWalk[1] = new Animator(playerWalkSheet, 1, 16, 16); //Up
			playerWalk[2] = new Animator(playerWalkSheet, 2, 16, 16); //Left
			playerWalk[3] = new Animator(playerWalkSheet, 3, 16, 16); //Right

			//Default Idle
			currentIdle = playerIdle[0];
			#endregion

			#region SFX

			//Walking
			SoundEffect walk1 = game.Content.Load<SoundEffect>("sfx/player-sfx/walk_1");
			SoundEffect walk2 = game.Content.Load<SoundEffect>("sfx/player-sfx/walk_2");
			SoundEffect walk3 = game.Content.Load<SoundEffect>("sfx/player-sfx/walk_3");

			soundEffects = new SoundEffect[] { walk1, walk2, walk3 };

			#endregion

		}
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
                if (IsPushing(out PushableObject pushedItem))
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
                if (IsPushing(out PushableObject pushedItem))
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
                if (IsPushing(out PushableObject pushedItem))
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
                if (IsPushing(out PushableObject pushedItem))
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

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();
			/*	Shared.sb.Draw(tex, position, new Rectangle(0, 0, tex.Width, tex.Height), Color.White, 0.0f, new Vector2(), scale * map.MapScaleFactor, SpriteEffects.None, 0f);*/

			currentAnimation.Animate(Shared.sb, gameTime, position);
			Shared.sb.End();
            base.Draw(gameTime);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(animationSheet[0].Width * scale), (int)(animationSheet[0].Height * scale));
        }

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
					}
					return true;
				}
			}

			return false;
        }

        private bool IsPushing(out PushableObject pushedItem)
        {
            pushedItem = null;

            foreach (PushableObject item in pushableObjects)
            {
                if (item.GetBounds().Intersects(GetBounds()))
                {
                    pushedItem = item;
                    return true;
                }
            }

            return false;
        }

        private bool IsPickingUpItem(out PickupObject pickedupItem)
        {
            pickedupItem = null;

            foreach (PickupObject item in pickupObjects)
            {
                if (item.GetBounds().Intersects(GetBounds()))
                {
                    pickedupItem = item;
                    return true;
                }
			}

			return false;
		}


		/// <summary>
		/// Play Random Walking sound from the array of sfx
		/// </summary>
		private void PlayWalkSound()
		{
			random = new Random();
			int randomIndex = random.Next(0, soundEffects.Length);

			soundEffects[randomIndex]?.Play();
		}
        public void ResetPlayer()
        {
            position = originalPosition;
        }
    }
}

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
        private Texture2D tex;
        public Vector2 position;
        public Vector2 originalPosition;
        private float moveSpeed = 1.5f;
        private float scale = 0.25f;

        private List<CollisionLayer> collisionLayers;
        private List<PushableObject> pushableObjects;
        private List<PickupObject> pickupObjects;
        private List<Door> doors;
         
        public PlayerObject(Game game, Map map, Texture2D tex, Vector2 position) : base(game)
        {
            this.tex = tex;
            this.position = position;
            originalPosition = this.position;
            this.map = map;
            collisionLayers = map.CollisionLayers;
            pushableObjects = map.PushableObjects;
            pickupObjects = map.PickupObjects;
            doors = map.Doors;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 initPos = position;

            KeyboardState keyboardstate = Keyboard.GetState();
            if (keyboardstate.IsKeyDown(Keys.D))//Move right
            {
                position.X += moveSpeed;

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

            if (IsPickingUpItem(out PickupObject pickedupItem))
            {
                map.Bag.AddItemToBag(pickedupItem);
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();
			Shared.sb.Draw(tex, position * map.MapScaleFactor + Shared.displayPosShift, new Rectangle(0, 0, tex.Width, tex.Height), 
                Color.White, 0.0f, new Vector2(), scale * map.MapScaleFactor, SpriteEffects.None, 0f);
			Shared.sb.End();
            base.Draw(gameTime);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)(tex.Width * scale), (int)(tex.Height * scale));
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

        public void ResetPlayer()
        {
            position = originalPosition;
        }
    }
}

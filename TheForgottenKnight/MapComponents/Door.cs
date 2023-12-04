using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.MapComponents
{
	public class Door : BaseInteractiveObject
	{
		private bool isUnlocked;
		private bool isLevelCompleteDoor;

		/// <summary>
		/// Default constructor to create an empty door reference
		/// </summary>
		/// <param name="game">The game context for the door</param>
		public Door(Game game) : base(game)
		{

		}

		/// <summary>
		/// Create a closed door
		/// </summary>
		/// <param name="game">The game context for the door</param>
		/// <param name="map">The map level context for the door</param>
		/// <param name="tex">The tileset texture used to draw the door</param>
		/// <param name="width">The width of the door in pixels</param>
		/// <param name="height">The height of the door in pixels</param>
		/// <param name="tilesetRow">The row of the tileset frame to use</param>
		/// <param name="tilesetColumn">The column of the tileset frame to use</param>
		/// <param name="startingX">The X value of the door's starting position</param>
		/// <param name="startingY">The Y value of the door's starting position</param>
		/// <param name="isLevelCompleteDoor">Boolean identifying the door as the Level Complete Door or not</param>
		public Door(Game game, Map map, Texture2D tex, int width, int height, int tilesetRow, int tilesetColumn, float startingX, float startingY, bool isLevelCompleteDoor = false)
			: base(game, map, tex, width, height, tilesetRow, tilesetColumn, startingX, startingY)
		{
			this.isLevelCompleteDoor = isLevelCompleteDoor;
			isUnlocked = false;
		}

		public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }

		/// <summary>
		/// Set the door as unlocked
		/// </summary>
		public void UnlockDoor()
		{
			if (isUnlocked)
			{
				return;
			}
			else
			{
				IsUnlocked = true;
				tilesetColumn += 1;
				tilesetRec = new Rectangle(width * tilesetColumn, height * tilesetRow, width, height); 
			}
		}

		public override void ResetPosition()
		{
			isUnlocked = false;
			base.ResetPosition();
		}

		public override void Update(GameTime gameTime)
		{
			if (isLevelCompleteDoor && isUnlocked)
			{
				map.CurrentLevelStatus = LevelStatus.Won;
			}
			base.Update(gameTime);
		}
	}
}

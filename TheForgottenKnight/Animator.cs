﻿/* Animator.cs
 * The Forgotten Knight
 *	Revision History
 *			Miles Purvis, 2023.11.26: Created
 *			
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	/// <summary>
	/// Handles animation
	/// </summary>
	public class Animator
	{
		private Texture2D animationSheet;
		private int column;
		private int width;
		private int height;
		private int frames;
		private int c = 0;
		private int timeSinceLastFrame = 0;

		/// <summary>
		/// Animator class to handle sprite sheet animation
		/// </summary>
		/// <param name="animationSheet">The animation sheet to be animated</param>
		/// <param name="column">The direction which the animation will parse the document</param>
		/// <param name="width">each sprits dimension eg.16x16 for width</param>
		/// <param name="height">each sprits dimension eg.16x16 for Height</param>
		public Animator(Texture2D animationSheet, int column, int width, int height)
		{
			this.animationSheet = animationSheet;
			this.column = column;
			this.width = width;
			this.height = height;

			frames = animationSheet.Height / height;

		}

		/// <summary>
		/// Animate Function takes sprite batch game time position and animation 
		/// speed to loop through sprite sheet and call the draw method
		/// </summary>
		/// <param name="spriteBatch">Needed for draw override</param>
		/// <param name="gameTime">Passed into for correct looping time of animation sheet</param>
		/// <param name="position">position where to draw animation</param>
		/// <param name="animationSpeed">speed which animations are drawn</param>
		public void Animate(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, int animationSpeed = 100)
		{

			if (c < frames)
			{
				spriteBatch.Draw(animationSheet, position, new Rectangle(width * column, height * c, 16, 16), Color.White);
				timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

				if (timeSinceLastFrame > animationSpeed)
				{
					timeSinceLastFrame -= animationSpeed;
					c++;

					if (c == frames)
					{
						c = 0;
					}
				}
			}

		}
	}
}

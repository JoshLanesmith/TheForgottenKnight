/* Animator.cs
 * The Forgotten Knight
 *	Revision History
 *			Miles Purvis, 2023.11.26: Created		
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheForgottenKnight
{
    /// <summary>
    /// Animator class to handle sprite sheet animation.
    /// </summary>
    public class Animator
	{
		private Map map;
		private Texture2D animationSheet;
		private int column;
		private int width;
		private int height;
		private int frames;
		private int c = 0;
		private int timeSinceLastFrame = 0;

		/// <summary>
		/// Initializes a new instance of the Animator class.
		/// </summary>
		/// <param name="map">The map associated with the animator.</param>
		/// <param name="animationSheet">The animation sheet to be animated.</param>
		/// <param name="column">The direction in which the animation will parse the document.</param>
		/// <param name="width">The width of each sprite dimension (e.g., 16x16 for width).</param>
		/// <param name="height">The height of each sprite dimension (e.g., 16x16 for height).</param>
		public Animator(Map map, Texture2D animationSheet, int column, int width, int height)
		{
			this.map = map;
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
				spriteBatch.Draw(animationSheet, position * map.MapScaleFactor + Shared.displayPosShift, new Rectangle(width * column, height * c, width, height),
					Color.White, 0.0f, Vector2.Zero, map.MapScaleFactor, SpriteEffects.None, 0);
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

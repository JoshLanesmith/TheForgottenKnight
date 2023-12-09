/* GameScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.Scenes
{
    public abstract class GameScene : DrawableGameComponent
    {
		/// <summary>
		/// Gets or sets the list of game components associated with the scene.
		/// </summary>
		public List<GameComponent> Components { get; set; }

		/// <summary>
		/// Initializes a new instance of the GameScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		protected GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            Hide();
        }

		/// <summary>
		/// Hides the game scene, setting Enabled and Visible properties to false.
		/// </summary>
		public virtual void Hide()
		{
			this.Enabled = false;
			this.Visible = false;
		}
		/// <summary>
		/// Shows the game scene, setting Enabled and Visible properties to true.
		/// </summary>
		public virtual void Show()
		{
			this.Enabled = true;
			this.Visible = true;
		}

		/// <summary>
		/// Updates the game scene, iterating through enabled components and updating them.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Update(GameTime gameTime)
        {
			foreach (GameComponent item in Components)
			{
				if (item.Enabled)
				{
					item.Update(gameTime);
				}
			}

			base.Update(gameTime);
        }

		/// <summary>
		/// Draws the game scene, iterating through visible drawable components and drawing them.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing state.</param>
		public override void Draw(GameTime gameTime)
        {
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

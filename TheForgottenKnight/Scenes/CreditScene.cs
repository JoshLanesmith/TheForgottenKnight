/* CreditScene.cs
 * The Forgotten Knight
 *    Revision History
 *            Miles Purvis, 2023.12.08: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.Scenes
{
	/// <summary>
	/// Represents the credit scene in the game, displaying credits using a parallax scrolling background.
	/// </summary>
	public class CreditScene : GameScene
	{
		private Rectangle srcRec;
		private Texture2D tex;
		private Vector2 pos;
		private Vector2 speed;

		/// <summary>
		/// Initializes a new instance of the CreditScene class.
		/// </summary>
		/// <param name="game">The Game instance.</param>
		public CreditScene(Game game) : base(game)
		{
			tex = Game.Content.Load<Texture2D>("images/dungeon");
			srcRec = new Rectangle(0, 0, tex.Width, tex.Height);
			pos = new Vector2(0,(Shared.stage.Y - Shared.gameDisplaySize.X));
			speed=new Vector2(2, 0);

			Parallax para = new Parallax(Game,tex,srcRec,speed,pos);
			this.Components.Add(para);

		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight.Scenes
{
	public class CreditScene : GameScene
	{
		private Rectangle srcRec;
		private Texture2D tex;
		private Vector2 pos;
		private Vector2 speed;


		public CreditScene(Game game) : base(game)
		{
			tex = Game.Content.Load<Texture2D>("images/dungeon");
			srcRec = new Rectangle(0, 0, tex.Width, tex.Height);
			pos = new Vector2(0,(Shared.stage.Y - Shared.gameDisplaySize.X));
			speed=new Vector2(2, 0);

			Parallax para = new Parallax(Game,tex,srcRec,speed,pos);
			this.Components.Add(para);

		}

		protected override void LoadContent()
		{
			
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
		
			base.Draw(gameTime);
		}
	}
}

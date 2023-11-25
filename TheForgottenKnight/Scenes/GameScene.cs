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
		public List<GameComponent> Components { get; set; }

		protected GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            Hide();
        }

		public virtual void Hide()
		{
			this.Enabled = false;
			this.Visible = false;
		}
		public virtual void Show()
		{
			this.Enabled = true;
			this.Visible = true;
		}

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

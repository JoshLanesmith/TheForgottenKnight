using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
    /// <summary>
    /// MenuComonent used to draw the menu items and toggle the selected menu items
    /// </summary>
    public class MenuComponent : DrawableGameComponent
    {
        private SpriteFont regularFont, hilightFont;
        private List<string> menuItems;

        public int SelectedIndex { get; set; }
        private Vector2 position;
        private Color regularColor = Color.White;
        private Color hilightColor = Color.Red;

        private KeyboardState oldState;

        /// <summary>
        /// Generate menu items with an array of strings for the menu items' text
        /// </summary>
        /// <param name="game">The game context for the nenu</param>
        /// <param name="menus">Array of strings for the menu items</param>
        public MenuComponent(Game game, string[] menus) : base(game)
        {
            this.regularFont = Shared.regularFont;
            this.hilightFont = Shared.hilightFont;

            menuItems = menus.ToList();
            position = new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            // Toggle down through the menu items 
            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                SelectedIndex++;
                
                // If currently on the last menu item then loop to the top item
                if (SelectedIndex == menuItems.Count)
                {
                    SelectedIndex = 0;
                }
            }

			// Toggle up through the menu items 
			if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;

				// If currently on the first menu item then loop to the bottom item
				if (SelectedIndex == -1)
                {
                    SelectedIndex = menuItems.Count() - 1;
                }
            }


            oldState = ks;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 tempPos = position;
			Shared.sb.Begin();

            for (int i = 0; i < menuItems.Count(); i++)
            {
                if (i == SelectedIndex)
                {
                    // Draw the selected item with the hilighted font and color
                    Shared.sb.DrawString(hilightFont, menuItems[i], tempPos, hilightColor);
                    tempPos.Y += hilightFont.LineSpacing;
                }
                else
                {
                    // Draw all other items with the regular font and color
					Shared.sb.DrawString(regularFont, menuItems[i], tempPos, regularColor);
                    tempPos.Y += hilightFont.LineSpacing;
                }

            }

			Shared.sb.End();

            base.Draw(gameTime);
        }
    }
}

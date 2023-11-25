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
    /// Shared static objects accessible to all classes within the game
    /// </summary>
    public class Shared
    {
		public static SpriteBatch sb;
		public static Vector2 stage;
        public static SpriteFont regularFont;
        public static SpriteFont hilightFont;
        public static SpriteFont labelFont;
        public static SpriteFont smallFont;
        public static Vector2 displayPosShift;
        public static Vector2 gameDisplaySize;
		public static int numberOfHighScoresStored = 10;

	}
}

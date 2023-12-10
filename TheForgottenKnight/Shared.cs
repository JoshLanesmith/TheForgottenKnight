/* Shared.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.20: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TheForgottenKnight
{
    /// <summary>
    /// Shared static objects accessible to all classes within the game
    /// </summary>
    public class Shared
    {
		public static SpriteBatch sb;
		public static Vector2 stage;

        //Fonts
        public static SpriteFont regularFont;
        public static SpriteFont highlightFont;
        public static SpriteFont labelFont;
        public static SpriteFont smallFont;
        public static SpriteFont titleFont;

        public static Vector2 displayPosShift;
        public static Vector2 gameDisplaySize;
		public static int numberOfHighScoresStored = 10;

        //Images
        public static Texture2D menuBgImage;
        public static Texture2D highscoreBgImage;
        public static Texture2D gameWonBgImage;
        public static Texture2D scrollPnlImage;
        public static Texture2D scrollPnlImageSmall;

        //Songs
        public static Song menuSong;
        public static Song highscoreSong;
        public static Song gameSong;
        public static Song helpSong;
  

	}
}

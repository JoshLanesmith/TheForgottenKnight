/* ClickableString.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.12.05: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheForgottenKnight
{
    /// <summary>
    /// Clickable string to display a tring with boundaries to track the mouse point and trigger an event
    /// </summary>
    public class ClickableString : DrawableGameComponent
    {
        /// <summary>
		/// OnClick delegate to allow the function of the button to be defined through the constructor
		/// </summary>
		public delegate void OnClick();

        private SpriteFont defaultFont, highlightFont, drawFont;
        private string message;
        private Vector2 position;
        private Rectangle boundaries;
        private Color defaultColor;
        private Color highlightColor;
        private Color drawColor;
        private ButtonStatus buttonStatus;
        private OnClick onClick;
        private bool isSelected;

        public Rectangle Bounds { get => boundaries; set => boundaries = value; }
        public ButtonStatus ButtonStatus { get => buttonStatus; set => buttonStatus = value; }

        /// <summary>
        /// Create a clickable string with a specified message and onClick event
        /// </summary>
        /// <param name="game">The game context for the ClickaleString</param>
        /// <param name="defaultFont">The default font used to display the text</param>
        /// <param name="highlightFont">The font used to highlight the text when the item is selected</param>
        /// <param name="message">The string displayed as the clickable item</param>
        /// <param name="position">The position of the top left point of the string</param>
        /// <param name="defaultColor">The default color used to display the text</param>
        /// <param name="highlightColor">The color used to highlight the text when the item is selected</param>
        /// <param name="onClick">The OnClick fuction to be executed</param>
        public ClickableString(Game game, SpriteFont defaultFont, SpriteFont highlightFont, string message, Vector2 position,
            Color defaultColor, Color highlightColor, OnClick onClick) : base(game)
        {
            this.defaultFont = defaultFont;
            this.highlightFont = highlightFont;
            this.message = message;
            this.position = position;
            this.defaultColor = defaultColor;
            this.highlightColor = highlightColor;
            Bounds = new Rectangle((int)position.X, (int)position.Y, (int)defaultFont.MeasureString(message).X, defaultFont.LineSpacing);
            drawFont = defaultFont;
            drawColor = defaultColor;
            this.onClick = onClick;

            ButtonStatus = ButtonStatus.Neutral;
            isSelected = false;

        }

        /// <summary>
        /// Toggle the item between selected and not selected
        /// </summary>
        public void ToggleSelected()
        {
            isSelected = !isSelected;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            Point mousePoint = new Point(ms.X, ms.Y);

            // Check if the button is hovered over by the mouse and clicked
            if (isSelected)
            {
                // Update the color and font used to draw the item when highlighted
                drawColor = highlightColor;
                drawFont = highlightFont;

                // Trigger the onClick event when the left mouse button is clicked
                if (Bounds.Contains(mousePoint) && ms.LeftButton == ButtonState.Pressed && ButtonStatus == ButtonStatus.Hover)
                {
                    ButtonStatus = ButtonStatus.Clicked;

                    onClick();
                }
            }
            else
            {
                // Reset the draw color and font to the default state
                drawColor = defaultColor;
                drawFont = defaultFont;
                ButtonStatus = ButtonStatus.Neutral;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();
            Shared.sb.DrawString(drawFont, message, position, drawColor);
            Shared.sb.End();
            base.Draw(gameTime);
        }
    }
}

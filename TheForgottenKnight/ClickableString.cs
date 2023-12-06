using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
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

        public ClickableString(Game game, SpriteFont defaultFont, SpriteFont highlightFont, string message, Vector2 position,
            Color defaultColor, Color highlightColor, OnClick onClick) : base(game)
        {
            this.defaultFont = defaultFont;
            this.highlightFont = highlightFont;
            this.message = message;
            this.position = position;
            this.defaultColor = defaultColor;
            this.highlightColor = highlightColor;
            Bounds = new Rectangle((int)position.X, (int)position.Y + defaultFont.LineSpacing, (int)defaultFont.MeasureString(message).X, defaultFont.LineSpacing);
            drawFont = defaultFont;
            drawColor = defaultColor;
            this.onClick = onClick;

            ButtonStatus = ButtonStatus.Neutral;
            isSelected = false;

        }

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
                drawColor = highlightColor;
                drawFont = highlightFont;

                if (Bounds.Contains(mousePoint) && ms.LeftButton == ButtonState.Pressed && ButtonStatus == ButtonStatus.Hover)
                {
                    ButtonStatus = ButtonStatus.Clicked;

                    onClick();
                }
            }
            else
            {
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
            //Debug.WriteLine(position);

            base.Draw(gameTime);
        }
    }
}

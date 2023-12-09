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
    public enum ButtonStatus
    {
        Neutral,
        Hover,
        Clicked,
        Disabled
    }

    /// <summary>
    /// Button compontent to display a button and execute a specified action
    /// </summary>
    public class ButtonComponent : DrawableGameComponent
    {
        /// <summary>
        /// OnClick delegate to allow the function of the button to be defined through the constructor
        /// </summary>
        public delegate void OnClick();


        private ButtonStatus buttonStatus;
        private Vector2 position;
        private Rectangle boundaries;
        private Rectangle srcRect;
        private Texture2D? tex;
        private OnClick onClick;
        private Dictionary<ButtonStatus, Texture2D>? buttonTextures;
        private ButtonState previousLeftButtonState;
        private ButtonState currentLeftButtonState;
        private float scale;

        public ButtonStatus ButtonStatus { get => buttonStatus; set => buttonStatus = value; }

        /// <summary>
        /// Create a button with a specified position texture and OnClick acction
        /// </summary>
        /// <param name="game">The game context for the ButtonComponent</param>
        /// <param name="position">The postion of the button's top left corner</param>
        /// <param name="tex">The image used for the button</param>
        /// <param name="onClick">The OnClick fuction to be executed</param>
        public ButtonComponent(Game game, Vector2 position, Texture2D tex, OnClick onClick) : base(game)
        {
            this.tex = tex;
            this.onClick = onClick;

            this.position = position + Shared.displayPosShift;

            boundaries = new Rectangle((int)this.position.X, (int)this.position.Y, tex.Width, tex.Height);

            ButtonStatus = ButtonStatus.Neutral;
        }

        public ButtonComponent(Game game, Vector2 position, Dictionary<ButtonStatus, Texture2D> buttonTextures, int buttonWidth, float scale, OnClick onClick) : base(game)
        {
            this.buttonTextures = buttonTextures;
            this.onClick = onClick;

            this.position = position + Shared.displayPosShift;
            this.scale = scale;

            ButtonStatus = ButtonStatus.Neutral;
            boundaries = new Rectangle((int)this.position.X, (int)this.position.Y, (int)(buttonTextures[ButtonStatus].Width * scale), (int)(buttonTextures[ButtonStatus].Height * scale));
            srcRect = new Rectangle(0, 0, buttonTextures[ButtonStatus].Width, buttonTextures[ButtonStatus].Height);
            previousLeftButtonState = ButtonState.Released;
            currentLeftButtonState = ButtonState.Released;
        }

        public override void Update(GameTime gameTime)
        {
            if (ButtonStatus != ButtonStatus.Disabled)
            {
                MouseState ms = Mouse.GetState();
                Point mousePoint = new Point(ms.X, ms.Y);

                currentLeftButtonState = ms.LeftButton;

                // Check if the button is hovered over by the mouse and clicked
                if (boundaries.Contains(mousePoint))
                {
                    if (currentLeftButtonState == ButtonState.Released)
                    {

                        ButtonStatus = ButtonStatus.Hover;
                    }

                    if (currentLeftButtonState == ButtonState.Pressed && previousLeftButtonState == ButtonState.Released)
                    {
                        ButtonStatus = ButtonStatus.Clicked;
                        previousLeftButtonState = currentLeftButtonState;

                    }
                    else if (currentLeftButtonState == ButtonState.Released && previousLeftButtonState == ButtonState.Pressed)
                    {
                        onClick();
                    }
                }
                else
                {
                    ButtonStatus = ButtonStatus.Neutral;
                }
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Shared.sb.Begin();

            Shared.sb.Draw(buttonTextures[ButtonStatus], position, srcRect, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            Shared.sb.End();


            base.Draw(gameTime);
        }


    }
}

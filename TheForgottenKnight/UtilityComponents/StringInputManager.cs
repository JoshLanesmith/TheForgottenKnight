/* StringInputManager.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.11.24: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TheForgottenKnight
{
    /// <summary>
    /// String input manager to allow users to type in the UI
    /// </summary>
    public class StringInputManager : GameComponent
	{
		private string inputText = string.Empty;
		private int maxStringLength = 9;
		private Keys[] lastPressedKeys = new Keys[5];

		public string InputText { get => inputText; set => inputText = value; }

		/// <summary>
		/// Create an empty string
		/// </summary>
		/// <param name="game"></param>
		public StringInputManager(Game game) : base(game)
		{
		}

		public override void Update(GameTime gameTime)
		{
			GetKeys();
			base.Update(gameTime);
		}

		/// <summary>
		/// Track key presses on the keyboard
		/// </summary>
		public void GetKeys()
		{
			KeyboardState kbState = Keyboard.GetState();

			// Store keys that have been pressed in the current update frame
			Keys[] pressedKeys = kbState.GetPressedKeys();

			foreach (Keys key in pressedKeys)
			{
				// If current keys pressed are not in last keys pressed then trigger onKeyDown action
				if (!lastPressedKeys.Contains(key))
				{
					OnKeyDown(key);
				}
			}

			// Update last keys pressed with the current pressed keys
			lastPressedKeys = pressedKeys;
		}

		public void OnKeyDown(Keys key)
		{
			// Remove last character in string when backspace is pressed
			if (key == Keys.Back && inputText.Length > 0)
			{
				inputText = inputText.Remove(inputText.Length - 1);
			}
			// Add the pressed key to the string if it is a letter or number up to the max string length
			else if(Regex.IsMatch(key.ToString(), @"^[0-9A-Z]$") && inputText.Length < maxStringLength)
			{
				inputText += key.ToString(); 
			}
            // Add an empty space to the string if the space bar is pressed
            else if (key == Keys.Space && inputText.Length < maxStringLength)
			{
				inputText += ' ';
			}
		}
	}
}

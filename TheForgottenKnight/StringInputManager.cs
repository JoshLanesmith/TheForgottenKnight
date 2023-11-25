using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheForgottenKnight
{
	public class StringInputManager : GameComponent
	{
		private string inputText = string.Empty;

		private Keys[] lastPressedKeys = new Keys[5];

		public string InputText { get => inputText; set => inputText = value; }

		public StringInputManager(Game game) : base(game)
		{
		}

		public override void Update(GameTime gameTime)
		{
			GetKeys();
			base.Update(gameTime);
		}

		public void GetKeys()
		{
			KeyboardState kbState = Keyboard.GetState();

			Keys[] pressedKeys = kbState.GetPressedKeys();

			foreach (Keys key in lastPressedKeys)
			{
				if (!pressedKeys.Contains(key))
				{
					// Key is no longer pressed
					OnKeyUp(key);
				}
			}

			foreach (Keys key in pressedKeys)
			{
				if (!lastPressedKeys.Contains(key))
				{
					OnKeyDown(key);
				}
			}

			lastPressedKeys = pressedKeys;
		}

		public void OnKeyUp(Keys key)
		{

		}

		public void OnKeyDown(Keys key)
		{
			if (key == Keys.Back && inputText.Length > 0)
			{
				inputText = inputText.Remove(inputText.Length - 1);
			}
			else if(Regex.IsMatch(key.ToString(), @"^[0-9A-Z]$") && inputText.Length < 12)
			{
				inputText += key.ToString(); 
			}
			else if(key == Keys.Space)
			{
				inputText += ' ';
			}
		}
	}
}

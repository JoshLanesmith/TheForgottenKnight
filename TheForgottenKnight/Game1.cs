﻿/* Game1.cs
 * The Forgotten Knight
 *    Revision History
 *            Josh Lanesmith, 2023.12.04: Created        
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TheForgottenKnight.Scenes;

namespace TheForgottenKnight
{
    /// <summary>
    /// The main class representing the game logic and managing different scenes.
    /// </summary>
    public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Player player;
		private float delay = 0;
		private bool trigger = false;
		private GameScene? previousScene;
		private GameScene? currentScene;
		private bool isPlayingSong = false;

		// Declare all scenes here
		private StartScene startScene;
		private HelpScene helpScene;
		private ActionScene actionScene;
		private EndScene endScene;
		private HighScoreScene highScoreScene;
		private CreditScene creditScene;

		public SpriteBatch SpriteBatch { get => _spriteBatch; set => _spriteBatch = value; }
		public GameScene PreviousScene { get => previousScene; set => previousScene = value; }
		public GameScene CurrentScene { get => currentScene; set => currentScene = value; }

		//Menu SFX
		private SoundEffect menuSelectSfx;

		/// <summary>
		/// Initializes a new instance of the Game1 class.
		/// </summary>
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		/// <summary>
		/// Performs initialization, including setting up display dimensions and loading assets.
		/// </summary>
		protected override void Initialize()
		{
            this.IsMouseVisible = false;
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			_graphics.IsFullScreen = true;
			_graphics.PreferredBackBufferWidth = screenWidth;
			_graphics.PreferredBackBufferHeight = screenHeight;
			_graphics.ApplyChanges();

			Shared.displayPosShift = screenWidth > screenHeight ?
				new Vector2((screenWidth - screenHeight) / 2, 0) :
				new Vector2(0, (screenHeight - screenWidth) / 2);

			Shared.gameDisplaySize = screenWidth > screenHeight ?
				new Vector2(screenHeight, screenHeight) :
				new Vector2(screenWidth, screenWidth);


			Shared.stage = new Vector2(_graphics.PreferredBackBufferWidth,
				_graphics.PreferredBackBufferHeight);

			_spriteBatch = new SpriteBatch(GraphicsDevice);
			Shared.sb = _spriteBatch;

			Shared.regularFont = this.Content.Load<SpriteFont>("fonts/RegularFont");
			Shared.highlightFont = this.Content.Load<SpriteFont>("fonts/HilightFont");
			Shared.labelFont = this.Content.Load<SpriteFont>("fonts/LabelFont");
			Shared.smallFont = this.Content.Load<SpriteFont>("fonts/SmallFont");
			Shared.titleFont = this.Content.Load<SpriteFont>("fonts/TitleFont");
			Shared.menuBgImage = this.Content.Load<Texture2D>("images/start-menu-bgImage/bgImage");
			Shared.highscoreBgImage = this.Content.Load<Texture2D>("images/highscoreBgImg");
			Shared.gameWonBgImage = this.Content.Load<Texture2D>("images/gameWonBgImg");
			Shared.scrollPnlImage = this.Content.Load<Texture2D>("images/ScrollPnlImg");
			Shared.scrollPnlImageSmall = this.Content.Load<Texture2D>("images/ScrollPnlImgSmall");
			Shared.menuSong = this.Content.Load<Song>("sfx/songs/menu_song");
			Shared.highscoreSong = this.Content.Load<Song>("sfx/songs/highscore_song");
			Shared.helpSong = this.Content.Load<Song>("sfx/songs/help_song");
			Shared.gameSong = this.Content.Load<Song>("sfx/songs/game_song");
			
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.1f;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			//Menu sfx
			menuSelectSfx = this.Content.Load<SoundEffect>("sfx/start-menu-sfx/menuSelect");

			player = new Player(this);


			startScene = new StartScene(this);
			this.Components.Add(startScene);

			helpScene = new HelpScene(this);
			this.Components.Add(helpScene);

			actionScene = new ActionScene(this, player);
			this.Components.Add(actionScene);

			endScene = new EndScene(this, player);
			this.Components.Add(endScene);

			highScoreScene = new HighScoreScene(this);
			this.Components.Add(highScoreScene);

			creditScene = new CreditScene(this);
			this.Components.Add(creditScene);

			// Show startScene at the beginning
			startScene.Show();
			CurrentScene = startScene;
		}

		/// <summary>
		/// Hides all scenes to manage switching between them.
		/// </summary>
		private void HideAllScenes()
		{
			foreach (GameComponent item in this.Components)
			{
				if (item is GameScene)
				{
					GameScene gs = (GameScene)item;

					gs.Hide();
				}
			}
		}

		/// <summary>
		/// Initiates a delay for scene transitions.
		/// </summary>
		/// <param name="amountoftime">The time to wait before triggering the next scene.</param>
		private void WaitTime(float amountoftime)
		{
			delay = amountoftime;
			trigger = true;
		}

		/// <summary>
		/// Resets the game state by creating new instances of necessary components.
		/// </summary>
		public void ResetGame()
		{
			this.Components.Remove(player);
			this.Components.Remove(actionScene);
			this.Components.Remove(endScene);

			player = new Player(this);
			actionScene = new ActionScene(this, player);
			endScene = new EndScene(this, player);

			this.Components.Add(player);
			this.Components.Add(actionScene);
			this.Components.Add(endScene);

			HideAllScenes();
			PreviousScene = CurrentScene;
			startScene.Show();
		}

		/// <summary>
		/// Switches to the action scene.
		/// </summary>
		public void GoToActionScene()
		{
            menuSelectSfx.Play();
            startScene.Hide();
            PreviousScene = CurrentScene;
            actionScene.Show();
            MediaPlayer.Play(Shared.gameSong);
        }

		/// <summary>
		/// Switches to the Help scene.
		/// </summary>
		public void GoToHelpScene()
		{
            menuSelectSfx.Play();
            startScene.Hide();
            PreviousScene = CurrentScene;
            helpScene.Show();
        }

		/// <summary>
		/// Switches to the Highscore scene.
		/// </summary>
		public void GoToHighscoreScene()
		{
            menuSelectSfx.Play();
            startScene.Hide();
            PreviousScene = CurrentScene;
            highScoreScene.Show();
        }
		/// <summary>
		/// Switches to the Credit scene.
		/// </summary>
		public void GoToCreditScene()
		{
			menuSelectSfx.Play();
			startScene.Hide();
			PreviousScene = CurrentScene;
			creditScene.Show();
		}

		/// <summary>
		/// Exits program.
		/// </summary>
		public void ExitGame()
		{
			Exit();
		}

		protected override void Update(GameTime gameTime)
		{
			
			KeyboardState ks = Keyboard.GetState();
			int selectedIndex = 0;

			if (startScene.Enabled)
			{
				selectedIndex = startScene.Menu.SelectedIndex;

				if (!isPlayingSong)
				{
					isPlayingSong = true;
					MediaPlayer.Play(Shared.menuSong);
			
				}

				if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter))
				{
					GoToActionScene();
				}
				else if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter))
				{
					GoToHelpScene();
				}
				else if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter))
				{
					GoToHighscoreScene();
				}
				else if (selectedIndex == 3 && ks.IsKeyDown(Keys.Enter))
				{
					GoToCreditScene();
				}
				else if (selectedIndex == 4 && ks.IsKeyDown(Keys.Enter))
				{
					ExitGame();
				}
			}
			else
			{
				if (ks.IsKeyDown(Keys.Escape) && CurrentScene != endScene)
				{
					HideAllScenes();
					PreviousScene = CurrentScene;
					startScene.Show();
					MediaPlayer.Stop();
					isPlayingSong = false;
				}

				if (actionScene.Enabled && actionScene.GameOver)
				{
					
					if (!trigger)
					{
						MediaPlayer.Play(Shared.highscoreSong);
						WaitTime(1);
					}

					if (delay > 0) delay -= 1f / 1000f * (float)gameTime.ElapsedGameTime.Milliseconds;
					if (delay <= 0 && trigger)
					{
						actionScene.Hide();
						PreviousScene = CurrentScene;
						endScene.Show();
						trigger = false;
						isPlayingSong = false;
					}
				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			base.Draw(gameTime);
		}

	}
}
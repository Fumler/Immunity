using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace immunity
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum GameState
        {
            Menu,
            Running,
            ServerList,
            Lobby
        }

        public static GameState gameState;

        public int height = 768;
        public int width = 1024;

        private bool gameStateNumber;

        private Vector2 mousePosition;

        private Gui topbar;
        private Gui actionbar;
        private const int ACTIONBUTTONOFFSET_X = 65;
        private const int MENUBUTTONOFFSET_X = 200;
        private Button rangedTowerButton, splashTowerButton, deleteTowerButton, nextWaveButton, upgradeTowerButton;
        private Button menuOne, menuTwo, menuThree, menuFour, menuFive;
        private MessageHandler toast, networkMessages;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Map map;
        private Pathfinder pathfinder;
        private PathView pathview;

        private Player player;

        private List<int>[] enemies = new List<int>[]
        {
            new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        private List<string> chatlog;

        private WaveHandler waveHandler;

        private Input input;
        private TextInput serverName;
        private Network network;

        private StorageHandler storageHandler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            gameState = GameState.Menu;
            gameStateNumber = true;

            input = new Input();
            toast = new MessageHandler(width, height);
            networkMessages = new MessageHandler(width, height);
            serverName = new TextInput(new Rectangle((width / 2)-200, 50, 200, 50));
            network = new Network();
            chatlog = new List<string>();

            // Action bar objects
            rangedTowerButton = new Button(new Rectangle(5, height - ACTIONBUTTONOFFSET_X, 60, 60), 10, "Basic ranged tower, low damage, single target.", Keys.D1);
            splashTowerButton = new Button(new Rectangle(5 + ACTIONBUTTONOFFSET_X, height - ACTIONBUTTONOFFSET_X, 60, 60), 20, "Basic splash tower, high damage, multiple targets.", Keys.D2);
            deleteTowerButton = new Button(new Rectangle(width - (ACTIONBUTTONOFFSET_X * 2), height - ACTIONBUTTONOFFSET_X, 60, 60), 3, "Deletes a tower, 50% gold return for normal towers, 100% for walls.", Keys.D);
            nextWaveButton = new Button(new Rectangle(width - ACTIONBUTTONOFFSET_X, height - ACTIONBUTTONOFFSET_X, 60, 60), 0, "Starts a new wave.", Keys.N);
            topbar = new Gui(new Rectangle(0, 0, width, 24));
            actionbar = new Gui(new Rectangle(0, (height - 70), width, 70));
            Button.GameHeight = height;
            Button.GameWidth = width;

            // Menu objects
            menuOne = new Button(new Rectangle(width / 2 - MENUBUTTONOFFSET_X, MENUBUTTONOFFSET_X, 400, 70), 13, "Start a new game.", Keys.D1);
            menuTwo = new Button(new Rectangle(width / 2 - MENUBUTTONOFFSET_X, MENUBUTTONOFFSET_X + 75, 400, 70), 14, "Multiplayer.", Keys.D2);
            menuThree = new Button(new Rectangle(width / 2 - MENUBUTTONOFFSET_X, MENUBUTTONOFFSET_X + (75 * 2), 400, 70), 15, "Check the game controls.", Keys.D3);
            menuFour = new Button(new Rectangle(width / 2 - MENUBUTTONOFFSET_X, MENUBUTTONOFFSET_X + (75 * 3), 400, 70), 17, "Save the game.", Keys.D4);
            menuFive = new Button(new Rectangle(width / 2 - MENUBUTTONOFFSET_X, MENUBUTTONOFFSET_X + (75 * 4), 400, 70), 16, "Exit the game.", Keys.D5);

            // Map object
            map = new Map();
            pathfinder = new Pathfinder(map);
            pathview = new PathView();
            
            // Enemy objects
            Unit.SetPathfinder(pathfinder, map);
            Unit.LoadPath();
            waveHandler = new WaveHandler(enemies);
            pathview.Path = Unit.Path;

            ContentHolder.Initialize();

            // Player object
            player = new Player(5, 1000, ref map);
            player.Wave = waveHandler.WaveNumber;

            storageHandler = new StorageHandler();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Sets the mouse position in our window.
            Mouse.WindowHandle = this.Window.Handle;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentHolder.Load(Content);

            // Button event function triggers
            rangedTowerButton.clicked += new EventHandler(ButtonClicked);
            splashTowerButton.clicked += new EventHandler(ButtonClicked);
            deleteTowerButton.clicked += new EventHandler(ButtonClicked);
            nextWaveButton.clicked += new EventHandler(ButtonClicked);
            

            // Menu button events
            menuOne.clicked += new EventHandler(ButtonClicked);
            menuTwo.clicked += new EventHandler(ButtonClicked);
            menuThree.clicked += new EventHandler(ButtonClicked);
            menuFour.clicked += new EventHandler(ButtonClicked);
            menuFive.clicked += new EventHandler(ButtonClicked);
            

            network.received += new Network.EventHandler(ReceivedNetwork);

            // Event trigger for unit death
            Unit.onDeath += new EventHandler(UnitDeath);
            Unit.onLastTile += new EventHandler(UnitReachEnd);

            // BUTTON TEXTURES
            // 0 - unit1 (placeholder button)
            // 1 - tower1
            // 2 - tower1upgrade1
            // 3 - tower2
            // 4 - tower2upgrade1
            // 5 - delete tower
            // 6 - new wave
            // 7 - new game menu 
            // 8 - multiplayer menu
            // 9 - controls menu
            // 10 - exit menu
            // 11 - blackbox
            // 12 - resume
            // 13 - Save game

            pathview.Texture = ContentHolder.TowerTextures[4];

            toast.InitVars(ContentHolder.Buttons[1], ContentHolder.Fonts[2]);
            networkMessages.InitVars(ContentHolder.Buttons[1], ContentHolder.Fonts[1]);
            network.Toast(ref networkMessages);

            serverName.InitVars(ContentHolder.Buttons[1], ContentHolder.Fonts);
            Button.Buttons = ContentHolder.Buttons;
            Gui.Font = ContentHolder.Fonts[1];
            Gui.Sprites = ContentHolder.GuiSprites;

            Unit.SetSprites(ContentHolder.UnitSprites, ContentHolder.Fonts[0]);
            Ammunition.SetSprites(ContentHolder.AmmunitionSprites);
            map.SetTextures(ContentHolder.TowerTextures);
            Tower.Turret = ContentHolder.TowerTextures[9];
            Button.Fonts = ContentHolder.Fonts;

            Thread thread = new Thread(new ThreadStart(PlaySong));
            thread.IsBackground = true;
            thread.Start();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(51, 0, 0));

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // BUTTON TEXTURES
            // 0 - unit1 (placeholder button)
            // 1 - tower1
            // 2 - tower1upgrade1
            // 3 - tower2
            // 4 - tower2upgrade1
            // 5 - delete tower
            // 6 - new wave
            // 7 - new game menu 
            // 8 - multiplayer menu
            // 9 - controls menu
            // 10 - exit menu
            // 11 - blackbox
            // 12 - resume
            // 13 - Save game

            if (gameState == GameState.Menu)
            {
                graphics.GraphicsDevice.Clear(Color.DarkRed);

                // menu buttons
                if (gameStateNumber)
                {
                    menuOne.Draw(spriteBatch, 7);

                } else
                    menuOne.Draw(spriteBatch, 12);

                
                menuTwo.Draw(spriteBatch, 8);
                menuThree.Draw(spriteBatch, 9);
                menuFour.Draw(spriteBatch, 13);
                menuFive.Draw(spriteBatch, 10);

                spriteBatch.Draw(ContentHolder.GuiSprites[3], new Rectangle((width / 2) - (ContentHolder.GuiSprites[3].Width / 2), 50, ContentHolder.GuiSprites[3].Width, ContentHolder.GuiSprites[3].Height), Color.White);
            }
            else if (gameState == GameState.Running)
            {
                map.Draw(spriteBatch);
                pathview.Draw(spriteBatch);
                player.Draw(spriteBatch);
                waveHandler.Draw(spriteBatch);
                actionbar.Draw(spriteBatch, 0, player);
                topbar.Draw(spriteBatch, 1, player);
                rangedTowerButton.Draw(spriteBatch, 1);
                splashTowerButton.Draw(spriteBatch, 3);
                deleteTowerButton.Draw(spriteBatch, 5);
                nextWaveButton.Draw(spriteBatch, 6);
            }
            else if (gameState == GameState.ServerList)
            {
                serverName.Draw(spriteBatch);
            }
            else if (gameState == GameState.Lobby)
            {
                serverName.Draw(spriteBatch);
                int i = 0;
                foreach (String text in chatlog)
                {
                    spriteBatch.DrawString(ContentHolder.Fonts[1], text, new Vector2(30, 50 + i), Color.White);
                    i += 20;
                }
            }

            toast.Draw(spriteBatch);
            networkMessages.Draw(spriteBatch);
            spriteBatch.Draw(ContentHolder.GuiSprites[2], mousePosition, new Color(255, 255, 255, 255));

            spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update();

            if (gameState == GameState.Menu)
            {
                if (input.IsKeyPressedOnce(Keys.Escape))
                {
                    gameState = GameState.Running;
                }

                menuOne.Update(gameTime);
                menuTwo.Update(gameTime);
                menuThree.Update(gameTime);
                menuFour.Update(gameTime);
                menuFive.Update(gameTime);
            }
            else if (gameState == GameState.Running)
            {
                if (input.IsKeyPressedOnce(Keys.Escape))
                {
                    gameState = GameState.Menu;
                }

                waveHandler.Update(gameTime);
                player.Update(ref waveHandler.GetCurrentWave().enemies, gameTime, toast, ref pathview);
                rangedTowerButton.Update(gameTime);
                splashTowerButton.Update(gameTime);
                deleteTowerButton.Update(gameTime);
                nextWaveButton.Update(gameTime);
            }else if(gameState == GameState.ServerList)
            {
                serverName.Update();
                if (input.IsKeyPressedOnce(Keys.F2))
                    network.Deliver("createlobby;"+serverName.Value);
                if (input.IsKeyPressedOnce(Keys.F1))
                    network.Deliver("username;"+serverName.Value);

            }else if(gameState == GameState.Lobby)
            {
                serverName.Update();
                if (input.IsKeyPressedOnce(Keys.Enter))
                {
                    network.Deliver("msglobby;" + serverName.Value);
                    serverName.Value = "";
                }
            }

            toast.Update(gameTime.TotalGameTime);
            networkMessages.Update(gameTime.TotalGameTime);

            switch (player.NewTowerType)
            {
                case 3: player.Tile = ContentHolder.TowerTextures[3]; break;
                case 10: player.Tile = ContentHolder.TowerTextures[10]; break;
                case 11: player.Tile = ContentHolder.TowerTextures[11]; break;
                case 20: player.Tile = ContentHolder.TowerTextures[20]; break;
                case 21: player.Tile = ContentHolder.TowerTextures[21]; break;
                default: player.Tile = ContentHolder.TowerTextures[2]; break;
            }

            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        private void ReceivedNetwork(string n)
        {
            string[] action = n.Split(new string[] { ";" }, StringSplitOptions.None);
            switch (action[0])
            {
                case "startlobby":
                    gameState = GameState.Lobby;
                    break;
                case "msglobby":
                    chatlog.Add(action[1]);
                    break;
            }
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            int actionType = ((Button)sender).type;

            switch (actionType)
            {
                case 0:
                    //    toast.AddMessage("Dude, you can't start a new wave yet....... ಠ益ಠ", new TimeSpan(0, 0, 3));
                    waveHandler.StartNextWave();
                    player.Wave = waveHandler.WaveNumber;
                    SaveGame("Auto_Save");
                    break;
 
                case 13: gameState = GameState.Running; break;
                case 14: gameState = GameState.ServerList; /* Multiplayer */ break;
                    gameStateNumber = false; 
                    break;
                case 15: /* show controls */ break;
                case 16:
                    /* CLOSE GAME */
                    network.Disconnect();

                    this.Exit(); break;
                case 17: SaveGame("Player_Save"); break;

                default: player.NewTowerType = ((Button)sender).type; break;
            }
        }

        private void UnitDeath(object sender, EventArgs e)
        {
            toast.AddMessage("Virus annihilated!", new TimeSpan(0, 0, 3));
            player.Gold += 50;
            player.Kills++;
            System.Diagnostics.Debug.WriteLine(player.Kills);
        }

        private void UnitReachEnd(object sender, EventArgs e)
        {
            toast.AddMessage("Virus made it to your brain!", new TimeSpan(0, 0, 3));
            player.Lives--;
        }

        private void PlaySong()
        {
            Song song = Content.Load<Song>("sounds//song");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(song);
        }

        private void SaveGame(String fileName)
        {
            StorageHandler.SaveGameData save = new StorageHandler.SaveGameData();

            save.lives = player.Lives;
            save.gold = player.Gold;
            save.wave = player.Wave;

            //Turn the map into a one dimensional array as the xml serializer does not support multidimensional arrays.
            int[] singleDimMap = new int[map.Height * map.Width];
            int i = 0;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    singleDimMap[i] = map.GetIndex(x, y);
                    i++;
                }
            }
            save.map = singleDimMap;
            
            storageHandler.SaveGame(save, "Immunity_Container", fileName+ ".sav");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace immunity
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int height = 768;
        public int width = 1024;

        private Gui topbar;
        private Gui actionbar;
        private Button buttonOne, buttonTwo, buttonThree;
        private MessageHandler toast;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Map map;
        private Pathfinder pathfinder;
        private PathView pathview;

        private Player player;

        private List<int>[] enemies = new List<int>[]
        {
            new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
            new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 }
        };
        private WaveHandler waveHandler;

        private Input input;

        /// <summary>
        /// All the textures for towers and everything
        /// that has to do with placing and removing towers.
        /// </summary>
        /// <param name="0">Provides a snapshot of timing values.</param>
        private Texture2D[] towerPlacementTextures;
        private List<Texture2D> ammunitionSprites;
        private List<SpriteFont> fonts;
        private List<Texture2D> buttons;
        private List<Texture2D> guiSprites;
        private List<Texture2D> unitSprites;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
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
            
            map.Draw(spriteBatch);

            pathview.Draw(spriteBatch);
            player.Draw(spriteBatch);

            waveHandler.Draw(spriteBatch);

            actionbar.Draw(spriteBatch, 0, player);
            topbar.Draw(spriteBatch, 1, player);
            buttonOne.Draw(spriteBatch, 0);
            buttonTwo.Draw(spriteBatch, 0);
            buttonThree.Draw(spriteBatch, 0);

            toast.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
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

            this.IsMouseVisible = true;

            input = new Input();
            toast = new MessageHandler(width, height);

            // Action bar objects
            buttonOne = new Button(new Rectangle(5, height - 65, 60, 60), 10, "Basic ranged tower, low damage, single target.");
            buttonTwo = new Button(new Rectangle(70, height - 65, 60, 60), 20, "Basic splash tower, high damage, multiple targets.");
            buttonThree = new Button(new Rectangle(135, height - 65, 60, 60), 3, "Deletes a tower, 50% gold return for normal towers, 100% for walls.");
            topbar = new Gui(new Rectangle(0, 0, width, 24));
            actionbar = new Gui(new Rectangle(0, (height - 70), width, 70));

            // Map object
            map = new Map();
            pathfinder = new Pathfinder(map);
            pathview = new PathView();

            // Player object
            player = new Player(5, 1000, ref map);

            // Enemy objects
            Unit.LoadPath(pathfinder, new Point(0, 0), new Point(map.Width - 1, map.Height - 1));
            pathview.Path = Unit.GetPath();
            waveHandler = new WaveHandler(enemies);
            waveHandler.CurrentWave.Start();

            towerPlacementTextures = new Texture2D[33];

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            buttonOne.clicked += new EventHandler(TowerButtonClicked);
            buttonTwo.clicked += new EventHandler(TowerButtonClicked);
            buttonThree.clicked += new EventHandler(TowerButtonClicked);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Sets the mouse position in our window.
            Mouse.WindowHandle = this.Window.Handle;

            towerPlacementTextures[0] = Content.Load<Texture2D>("sprites\\bg");
            towerPlacementTextures[1] = Content.Load<Texture2D>("sprites\\tumor");
            towerPlacementTextures[2] = Content.Load<Texture2D>("sprites\\hoverTile");
            towerPlacementTextures[3] = Content.Load<Texture2D>("sprites\\deleteTile");
            towerPlacementTextures[9] = Content.Load<Texture2D>("sprites\\towers\\turret");
            towerPlacementTextures[10] = Content.Load<Texture2D>("sprites\\towers\\Tier1Ranged");
            towerPlacementTextures[11] = Content.Load<Texture2D>("sprites\\towers\\Tier2Ranged");
            towerPlacementTextures[20] = Content.Load<Texture2D>("sprites\\towers\\Tier1Splash");
            towerPlacementTextures[21] = Content.Load<Texture2D>("sprites\\towers\\Tier2Splash");

            unitSprites = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\RedCell"),
                Content.Load<Texture2D>("sprites\\WhiteCell")
            };

            guiSprites = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\actionbar"),
                Content.Load<Texture2D>("sprites\\topbar")
            };

            buttons = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\buttons\\unit1"),
                Content.Load<Texture2D>("sprites\\blackbox")
            };

            fonts = new List<SpriteFont>() {
                Content.Load<SpriteFont>("fonts\\segoe"),
                Content.Load<SpriteFont>("fonts\\miramonte"),
                Content.Load<SpriteFont>("fonts\\miramonteBig")
            };

            ammunitionSprites = new List<Texture2D>();
            for (int i = 0; i <= 33; i++)
            {
                Texture2D temp = Content.Load<Texture2D>("sprites\\Ammo");
                ammunitionSprites.Add(temp);
            }

            pathview.Texture = Content.Load<Texture2D>("sprites\\path");
            pathview.Texture = Content.Load<Texture2D>("sprites\\path");

            toast.InitVars(buttons[1], fonts);
            buttonOne.SetSprites(buttons);
            buttonTwo.SetSprites(buttons);
            buttonThree.SetSprites(buttons);
            actionbar.SetSprites(guiSprites);
            topbar.Fonts = actionbar.Fonts = fonts[1];
            topbar.SetSprites(guiSprites);
           
            Unit.SetSprites(unitSprites);
            Ammunition.SetSprites(ammunitionSprites);
            map.SetTextures(towerPlacementTextures);
            Tower.Turret = towerPlacementTextures[9];
            Button.Fonts = fonts;

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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update();
            

            waveHandler.Update(gameTime);
            if (waveHandler.CurrentWave.WaveFinished)
            {
                waveHandler.StartNextWave();
            }
            player.Update(ref waveHandler.CurrentWave.enemies, gameTime, toast);
            toast.Update(gameTime.TotalGameTime);
            buttonOne.Update(gameTime);
            buttonTwo.Update(gameTime);
            buttonThree.Update(gameTime);

            // Allows the game to exit
            if (input.IsKeyPressed(Keys.Escape))
                this.Exit();

            //spawnDelay++;
            //if (spawnDelay > 15 && lastUsedUnit != unitList.Count)
            //{
            //    unitsOnMap.Add(unitList[lastUsedUnit]);
            //    lastUsedUnit++;
            //    spawnDelay = 0;
            //}

            switch (player.NewTowerType)
            {
                case 3: player.Tile = towerPlacementTextures[3]; break;
                case 10: player.Tile = towerPlacementTextures[10]; break;
                case 11: player.Tile = towerPlacementTextures[11]; break;
                case 20: player.Tile = towerPlacementTextures[20]; break;
                case 21: player.Tile = towerPlacementTextures[21]; break;
                default: player.Tile = towerPlacementTextures[2]; break;
            }
      
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        private void TowerButtonClicked(object sender, EventArgs e)
        {
            player.NewTowerType = ((Button)sender).type;
            System.Diagnostics.Debug.WriteLine(player.NewTowerType);
        }

        private void PlaySong()
        {
            Song song = Content.Load<Song>("sounds//song");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.0f;
            MediaPlayer.Play(song);
        }
    }
}
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
        private Button buttonOne, buttonTwo, buttonThree, buttonFour;
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
            buttonFour.Draw(spriteBatch, 0);

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
            buttonOne = new Button(new Rectangle(5, height - 65, 60, 60), 10, "Basic ranged tower, low damage, single target.", width, height);
            buttonTwo = new Button(new Rectangle(70, height - 65, 60, 60), 20, "Basic splash tower, high damage, multiple targets.",width, height);
            buttonThree = new Button(new Rectangle(135, height - 65, 60, 60), 3, "Deletes a tower, 50% gold return for normal towers, 100% for walls.", width, height);
            buttonFour = new Button(new Rectangle(width - 65, height - 65, 60, 60), 0, "Starts a new wave.", width, height);
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

            ContentHolder.Initialize();

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
            buttonOne.clicked += new EventHandler(ButtonClicked);
            buttonTwo.clicked += new EventHandler(ButtonClicked);
            buttonThree.clicked += new EventHandler(ButtonClicked);
            buttonFour.clicked += new EventHandler(ButtonClicked);


            pathview.Texture = ContentHolder.TowerTextures[4];

            toast.InitVars(ContentHolder.Buttons[1], ContentHolder.Fonts);
            Button.Buttons = ContentHolder.Buttons;
            Gui.Font = ContentHolder.Fonts[1];
            Gui.Sprites = ContentHolder.GuiSprites;

            Unit.SetSprites(ContentHolder.UnitSprites);
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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update();
            

            waveHandler.Update(gameTime);
            player.Update(ref waveHandler.GetCurrentWave().enemies, gameTime, toast);
            toast.Update(gameTime.TotalGameTime);
            buttonOne.Update(gameTime);
            buttonTwo.Update(gameTime);
            buttonThree.Update(gameTime);
            buttonFour.Update(gameTime);

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
                case 3: player.Tile = ContentHolder.TowerTextures[3]; break;
                case 10: player.Tile = ContentHolder.TowerTextures[10]; break;
                case 11: player.Tile = ContentHolder.TowerTextures[11]; break;
                case 20: player.Tile = ContentHolder.TowerTextures[20]; break;
                case 21: player.Tile = ContentHolder.TowerTextures[21]; break;
                default: player.Tile = ContentHolder.TowerTextures[2]; break;
            }
      
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            int actionType = ((Button)sender).type;

            switch (actionType)
            {
                case 0: waveHandler.StartNextWave(); break;
                default: player.NewTowerType = ((Button)sender).type; break;
            }
            
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
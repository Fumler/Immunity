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

        private List<Unit> unitList;
        private List<Unit> unitsOnMap;
        private int[] units = { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        private int spawnDelay;
        private int lastUsedUnit;

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
            foreach (Unit unit in unitList)
            {
                unit.Draw(spriteBatch);
            }

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
            unitsOnMap = new List<Unit>();
            unitList = new List<Unit>();
            Unit.LoadPath(pathfinder, new Point(0, 0), new Point(map.Width - 1, map.Height - 1));
            pathview.Path = Unit.GetPath();

            UnitFactory.CreateUnits(units, ref unitList);
            spawnDelay = 0;
            lastUsedUnit = 0;
            towerPlacementTextures = new Texture2D[33];

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
            buttonOne.clicked += new EventHandler(TowerButtonClicked);
            buttonTwo.clicked += new EventHandler(TowerButtonClicked);
            buttonThree.clicked += new EventHandler(TowerButtonClicked);


            pathview.Texture = ContentHolder.TowerTextures[4];

            toast.InitVars(ContentHolder.Buttons[1], fonts);
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
            
            for (int i = 0; i < unitsOnMap.Count; i++)
            {
                Unit unit = unitsOnMap[i];
                unit.Update();
                if (unit.Health < 0)
                {
                    unitsOnMap.Remove(unit);
                    i--;
                }
            }
            player.Update(ref unitsOnMap, gameTime, toast);
            toast.Update(gameTime.TotalGameTime);
            buttonOne.Update(gameTime);
            buttonTwo.Update(gameTime);
            buttonThree.Update(gameTime);

            // Allows the game to exit
            if (input.IsKeyPressed(Keys.Escape))
                this.Exit();

            spawnDelay++;
            if (spawnDelay > 15 && lastUsedUnit != unitList.Count)
            {
                unitsOnMap.Add(unitList[lastUsedUnit]);
                lastUsedUnit++;
                spawnDelay = 0;
            }

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
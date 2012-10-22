using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace immunity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int height = 768;
        public int width = 1024;
        private Gui actionbar;
        private Button buttonTest;
        private GraphicsDeviceManager graphics;
        private Map map;
        private Pathfinder pathfinder;
        private Player player;
        private SpriteBatch spriteBatch;
        private Gui topbar;
        private Unit unit;
        private Unit unit2;

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
            map.draw(spriteBatch);
            unit.draw(spriteBatch, 0);
            unit2.draw(spriteBatch, 1);

            actionbar.draw(spriteBatch, 2, player);
            topbar.draw(spriteBatch, 3, player);

            buttonTest.draw(spriteBatch, 0);
            spriteBatch.End();

            //gun.Play();

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

            // Actionbar objects
            buttonTest = new Button(new Rectangle(5, height - 65, 60, 60));
            topbar = new Gui(new Rectangle(0, 0, width, 24));
            actionbar = new Gui(new Rectangle(0, (height - 70), width, 70));

            // Map object
            map = new Map();
            pathfinder = new Pathfinder(map);

            // Player object
            player = new Player();
            player.gold = 5;
            player.lives = 15;

            // Enemy objects
            Unit.loadPath(pathfinder, new Point(0, 0), new Point(map.width - 1, map.height - 1));
            unit = new Unit(0);
            unit2 = new Unit(1);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            List<Texture2D> textures = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\bg"),
                Content.Load<Texture2D>("sprites\\tumor")
            };

            List<Texture2D> sprites = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\RedCell"),
                Content.Load<Texture2D>("sprites\\WhiteCell"),
                Content.Load<Texture2D>("sprites\\actionbar"),
                Content.Load<Texture2D>("sprites\\topbar")
            };

            List<Texture2D> buttons = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\buttons\\unit1")
            };

            List<SpriteFont> fonts = new List<SpriteFont>() {
                Content.Load<SpriteFont>("fonts\\segoe"),
                Content.Load<SpriteFont>("fonts\\miramonte")
            };

            buttonTest.setSprites(buttons);
            actionbar.setSprites(sprites);
            actionbar.setFonts(fonts);
            topbar.setSprites(sprites);
            topbar.setFonts(fonts);
            unit.setSprites(sprites);
            unit2.setSprites(sprites);
            map.setTextures(textures);

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
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            unit.Update();
            unit2.Update();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }
    }
}
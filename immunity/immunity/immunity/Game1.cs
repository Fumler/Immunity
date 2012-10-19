using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace immunity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map map;
        Pathfinder pathfinder;
        Unit unit;
        Gui actionbar;
        Gui topbar;

        int width = 1024;
        int height = 768;

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

            //gui = new Gui(new Vector2(500, (480 - 34)));
            topbar = new Gui(new Rectangle(0, 0, width, 32));
            actionbar = new Gui(new Rectangle(0, (height - 32), width, 32));

            map = new Map();
            pathfinder = new Pathfinder(map);

            Unit.loadPath(pathfinder, new Point(0, 0), new Point(map.width-1, map.height-1));
            unit = new Unit(0);
            
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
                Content.Load<Texture2D>("sprites\\grass"),
                Content.Load<Texture2D>("sprites\\tree")
                
            };

            List<Texture2D> sprites = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\RedCell"),
                Content.Load<Texture2D>("sprites\\WhiteCell"),
                Content.Load<Texture2D>("sprites\\gui")
            };

            List<SpriteFont> fonts = new List<SpriteFont>() {
                Content.Load<SpriteFont>("fonts\\font")
            };


            actionbar.setSprites(sprites);
            actionbar.setFonts(fonts);
            topbar.setSprites(sprites);
            topbar.setFonts(fonts);
            unit.setSprites(sprites);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            unit.Update();
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            map.draw(spriteBatch);
            unit.draw(spriteBatch);

            actionbar.draw(spriteBatch);
            topbar.draw(spriteBatch);
            spriteBatch.End();
            //gun.Play();

            base.Draw(gameTime);
        }
    }
}

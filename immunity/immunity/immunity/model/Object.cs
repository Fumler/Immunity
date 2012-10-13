using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// This is a base class that acts as a parent for entities in the game.
    /// </summary>
    public class Object : Microsoft.Xna.Framework.GameComponent
    {
        private static int lastIdUsed;

        private int id;
        private int hitpoints;
        private Vector2 coordinates;
        private string spriteName;
        private Texture2D sprite;
        private bool passable;

        protected int getlastId() {
            return lastIdUsed;
        }

        protected int getID() {
            return id;
        }

        protected int getHealth() {
            return hitpoints;
        }

        protected bool isPassable() {
            return passable;
        }

        protected Vector2 getPossition() {
            return coordinates;
        }

        protected void changePossition(Vector2 coords) {
            coordinates = coords;
        }

        protected void damage(int damage) {
            hitpoints -= damage;
        }

        protected void heal(int health) {
            hitpoints += health;
        }

        protected void draw() { 
            // TODO
            // Stuff.
        }

        private void loadSprite() {
            // TODO
            // Load sprite with name <spriteName> to <sprite>.
        }

        public Object(Game game, int id, int hp, Vector2 coords, string sprite, bool pass)
            : base(game)
        {
            this.id = id;
            this.hitpoints = hp;
            this.coordinates = coords;
            this.spriteName = sprite;
            this.passable = pass;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO:
            // Stuff

            loadSprite();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}

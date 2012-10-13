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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Unit : Controllable
    {
        private string type;
        private int ammunition;

        public int getAmmo() {
            return ammunition;
        }

        public void moveTo(Vector2 coords) {
            Vector2 tempCoords = new Vector2(0, 0);

            // TODO
            // Calculate move

            base.changePossition(tempCoords);
        }

        public Unit(Game game, int id, int hp, Vector2 coords, string sprite, bool pass, Player player, string type, int ammo, int damage)
            : base(game, id, hp, coords, sprite, pass, player, damage)
        {
            this.type = type;
            this.ammunition = ammo;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class Player
    {
        private int lives;
        private int gold;
        private int map;
        private int level;
        private int cellX, cellY, tileX, tileY;
        private int newTowerType = 0;
        private Input mouse = new Input();

        private Texture2D tile;

        private List<Tower> towers = new List<Tower>();

        public Player(int lives, int gold) {
            this.lives = lives;
            this.gold = gold;
            XElement xml = new XElement("player");
            xml.Save("test.xml");
        }
        public int Lives {
            get { return lives; }
            set { this.lives = value; }
        }
        public int Gold {
            get { return gold; }
            set { this.gold = value; }
        }
        public Texture2D Tile {
            set { this.tile = value; }
        }
        public int NewTowerType {
            set { this.newTowerType = value; }
        }
        public void Update() {
            mouse.update();

            cellX = (int)(mouse.currentMouseState.X / 32);
            cellY = (int)((mouse.currentMouseState.Y - 24) / 32);

            System.Diagnostics.Debug.WriteLine(cellX + " - " + cellY);
            if (mouse.releaseLeftClick) {
                if (newTowerType != 0) {
                    towers.Add(new Tower());
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(tile, new Vector2(cellX * 32, (cellY * 32) + 24), Color.White);
        }
    }
}

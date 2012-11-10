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
        private int currentMap;
        private int level;
        private int cellX, cellY, tileX, tileY;
        private int newTowerType = 0;
        private Input mouse = new Input();
        private Map map;

        private Texture2D tile;

        private List<Tower> towers = new List<Tower>();

        public Player(int lives, int gold) {
            this.lives = lives;
            this.gold = gold;
            //XElement xml = new XElement("player");
            //xml.Save("test.xml");
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
            get { return newTowerType; }
        }
        public void Map(ref Map map) {
            this.map = map;
        }
        public void Update() {
            mouse.update();

            cellX = (int)(mouse.currentMouseState.X / 32);
            cellY = (int)((mouse.currentMouseState.Y - 24) / 32);

            if (mouse.releaseLeftClick && (map.height-1) >= cellY && (map.width-1) >= cellX) {
                if (newTowerType != 0) {
                    if (map.getIndex(cellX, cellY) == 0) {
                        map.AddToMap(cellX, cellY, newTowerType);
                        Pathfinder p = new Pathfinder(map);
                        List<Vector2> t = p.FindPath(new Point(0,0), new Point(map.width - 1, map.height - 1));
                        if (t.Count == 0) {
                            System.Diagnostics.Debug.WriteLine("Nooooooo");
                            map.AddToMap(cellX, cellY, 0);
                        }
                    }
                    //towers.Add(new Tower());
                }
            }
            if (mouse.releaseRightClick) {
                newTowerType = 0;
            }
            
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(tile, new Vector2(cellX * 32, (cellY * 32) + 24), new Color(255, 255, 255, 50));
        }
    }
}

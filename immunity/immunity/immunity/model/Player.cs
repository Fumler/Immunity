using System.Collections.Generic;
using System.Xml.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Player
    {
        private int lives;
        private int gold;
        private int currentMap;
        private int wave = 1;
        private int cellX, cellY, tileX, tileY;
        private int newTowerType = 0;
        private Input mouse = new Input();
        private Map map;

        private Texture2D tile;

        private Tower[,] towers;

        public Player(int lives, int gold, ref Map map)
        {
            this.map = map;
            this.lives = lives;
            this.gold = gold;
            towers = new Tower[map.Width, map.Height];
            //XElement xml = new XElement("player");
            //xml.Save("test.xml");
        }

        public int Lives
        {
            get { return lives; }
            set { this.lives = value; }
        }
        public int Wave
        {
            get { return wave; }
            set { this.wave = value; }
        }
        public int Gold
        {
            get { return gold; }
            set { this.gold = value; }
        }

        public Texture2D Tile
        {
            set { this.tile = value; }
        }

        public int NewTowerType
        {
            set { this.newTowerType = value; }
            get { return newTowerType; }
        }

        public void Update(ref List<Unit> enemies, GameTime gameTime, MessageHandler toast)
        {
            mouse.Update();

            cellX = (int)(mouse.currentMouseState.X / 32);
            cellY = (int)((mouse.currentMouseState.Y - 24) / 32);

            if (mouse.ReleaseLeftClick && (map.Height - 1) >= cellY && (map.Width - 1) >= cellX)
            {
                if (newTowerType != 0)
                {
                    if (map.GetIndex(cellX, cellY) != 0 && newTowerType == 3)
                    {
                        int sellType = map.GetIndex(cellX, cellY);
                        gold += (Tower.GetCost(sellType) == 1) ? Tower.GetCost(sellType) : (int)(Tower.GetCost(sellType) * 0.5f);
                        map.AddToMap(cellX, cellY, 0);
                        towers[cellX, cellY] = null;
                    }
                    else if (map.GetIndex(cellX, cellY) == 0)
                    {
                        
                        if (gold >= Tower.GetCost(newTowerType))
                        {
                            gold -= Tower.GetCost(newTowerType);
                            map.AddToMap(cellX, cellY, newTowerType);
                            Pathfinder p = new Pathfinder(map);
                            List<Vector2> t = p.FindPath(new Point(0, 0), new Point(map.Width - 1, map.Height - 1));
                            if (t.Count == 0)
                            {
                                System.Diagnostics.Debug.WriteLine("Nooooooo");
                                map.AddToMap(cellX, cellY, 0);
                                toast.addMessage("ಠ_ಠ Du är for dårlig at bygga tårn.", new TimeSpan(0, 0, 3));
                                gold += Tower.GetCost(newTowerType);
                            }
                            else
                            {
                                towers[cellX, cellY] = new Tower(newTowerType, cellX, cellY);
                            }
                        }
                        else
                        {
                            toast.addMessage("(╯°□°）╯︵ ʎǝuoɯ ǝɹoɯ ou", new TimeSpan(0, 0, 3));

                        }
                    }
                }
            }
            if (mouse.ReleaseRightClick)
            {
                newTowerType = 0;
            }
           
            for (int y = 0; y < towers.GetLength(1); y++)
            {
                for (int x = 0; x < towers.GetLength(0); x++)
                {
                    if (towers[x, y] != null)
                    {
                        towers[x, y].Update(ref enemies, gameTime);
                    }
                }
            }   
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tile, new Vector2(cellX * 32, (cellY * 32) + 24), new Color(255, 255, 255, 50));
            for (int y = 0; y < towers.GetLength(1); y++)
            {
                for (int x = 0; x < towers.GetLength(0); x++)
                {
                    if (towers[x, y] != null)
                    {
                        towers[x, y].Draw(spriteBatch);
                    }
                }
            }
        }
    }
}

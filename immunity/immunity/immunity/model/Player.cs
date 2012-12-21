﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Player
    {
        private int lives;
        private int gold;
        private int currentMap;
        private int wave;
        private int kills, towersBuilt;
        private int cellX, cellY, tileX, tileY;
        private int newTowerType = 0;
        private Input mouse = new Input();
        private Map map;
        private string name;
        private MessageHandler toast;
        private Texture2D tile;

        private Tower[,] towers;
        private Tower selectedTower = null;

        private WaveHandler waveHandler;
        private List<Unit> enemies;

        public Player(int lives, int gold, ref Map map, ref MessageHandler toast, ref WaveHandler waveHandler)
        {
            this.map = map;
            this.lives = lives;
            this.gold = gold;
            this.wave = 0;
            this.toast = toast;
            this.waveHandler = waveHandler;
            towers = new Tower[map.Width, map.Height];

            //XElement xml = new XElement("player");
            //xml.Save("test.xml");
        }

        public int Lives
        {
            get { return lives; }
            set { this.lives = value; }
        }

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public int Wave
        {
            get { return wave; }
            set { this.wave = value; }
        }

        public int Kills
        {
            get { return kills; }
            set { this.kills = value; }
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

        public Tower SelectedTower
        {
            get { return selectedTower; }
        }

        public void UpdateTowerList()
        {
            System.Diagnostics.Debug.WriteLine(" x "+ selectedTower.Type);
            towers[selectedTower.PositionX,selectedTower.PositionY] = selectedTower;
        }

        public void Update(GameTime gameTime, ref PathView path)
        {
            this.enemies = waveHandler.GetCurrentWave().enemies;
            mouse.Update();

            cellX = (int)(mouse.currentMouseState.X / 32);
            cellY = (int)((mouse.currentMouseState.Y - 24) / 32);

            if (mouse.ReleaseLeftClick && (map.Height - 1) >= cellY && (map.Width - 1) >= cellX)
            {
                if (newTowerType != 0 && !waveHandler.WaveStarted())
                {
                    selectedTower = null;
                    if (map.GetIndex(cellX, cellY) != 0 && towers[cellX, cellY] != null && newTowerType == 3)
                    {
                        int sellType = map.GetIndex(cellX, cellY);
                        if (sellType == 1)
                        {
                            gold += Tower.GetCost(sellType);
                        }
                        else
                        {
                            gold += (int)(Tower.GetCost(sellType) * 0.5f);
                        }
                        map.AddToMap(cellX, cellY, 0);
                        towers[cellX, cellY] = null;
                        Pathfinder p = new Pathfinder(map);
                        List<Vector2> newPath = p.FindPath(new Point(0, 0), new Point(map.Width - 1, map.Height - 1));
                        path.Path = newPath;
                        Unit.Path = newPath;
                    }

                    else if (map.GetIndex(cellX, cellY) == 0 && newTowerType != 3)
                    {
                        if (gold >= Tower.GetCost(newTowerType))
                        {
                            gold -= Tower.GetCost(newTowerType);
                            map.AddToMap(cellX, cellY, newTowerType);
                            Pathfinder p = new Pathfinder(map);
                            List<Vector2> t = p.FindPath(new Point(0, 0), new Point(map.Width - 1, map.Height - 1));
                            if (t.Count == 0)
                            {
                                map.AddToMap(cellX, cellY, 0);
                                toast.AddMessage("ಠ_ಠ Du är for dårlig at bygga tårn.", new TimeSpan(0, 0, 3));
                                gold += Tower.GetCost(newTowerType);
                            }
                            else
                            {
                                path.Path = t;
                                Unit.Path = t;
                                towers[cellX, cellY] = new Tower(newTowerType, cellX, cellY);
                            }
                        }
                        else
                        {
                            toast.AddMessage("(╯°□°）╯︵ ʎǝuoɯ ǝɹoɯ ou", new TimeSpan(0, 0, 3));
                        }
                    }
                }else if (towers[cellX, cellY] != null && newTowerType == 0 && towers[cellX, cellY].Type != 1)
                {
                    selectedTower = towers[cellX, cellY];
                }
            }
            if (mouse.ReleaseRightClick)
            {
                newTowerType = 0;
                selectedTower = null;
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
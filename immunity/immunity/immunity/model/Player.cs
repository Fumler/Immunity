using System;
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
            towers[selectedTower.PositionX,selectedTower.PositionY] = selectedTower;
        }

        public void Update(GameTime gameTime, ref PathView path)
        {
            this.enemies = waveHandler.GetCurrentWave().enemies;
            mouse.Update();

            // Finding out what cell in the array the mouse pointer is
            cellX = (int)(mouse.currentMouseState.X / 32);
            cellY = (int)((mouse.currentMouseState.Y - 24) / 32);

            // Only enter if you are releasing Left mouse and you are inside the array of the map
            if (mouse.ReleaseLeftClick && (map.Height - 1) >= cellY && (map.Width - 1) >= cellX && cellX >= 0 && cellY >= 0)
            {
                // Only enter if you have selected a tower you want to place and the round has not started
                if (newTowerType != 0 && !waveHandler.WaveStarted())
                {
                    // set the tower you want to upgrade to null
                    selectedTower = null;

                    // Only enter if there is something in the map array placed on the position you  have clicked
                    // and the newtowertype is 3(Which is the delete)
                    if (map.GetIndex(cellX, cellY) != 0 && towers[cellX, cellY] != null && newTowerType == 3)
                    {
                        // getting the type of tower you want to sell
                        int sellType = map.GetIndex(cellX, cellY);

                        // If tower is type one, which is a not an attacking tower you will get all you money back
                        // if not then 50% back
                        if (sellType == 1)
                        {
                            gold += Tower.GetCost(sellType);
                        }
                        else
                        {
                            gold += (int)(Tower.GetCost(sellType) * 0.5f);
                        }

                        // Add 0 to the position where the tower used to be.
                        map.AddToMap(cellX, cellY, 0);
                        // Remove the tower object by setting it to null
                        towers[cellX, cellY] = null;
                        // Sending the edited map in to the pathfinder again to get new path
                        Pathfinder p = new Pathfinder(map);
                        List<Vector2> newPath = p.FindPath(new Point(0, 0), new Point(map.Width - 1, map.Height - 1));
                        // send new path to the Pathview class to show new path
                        path.Path = newPath;
                        // send new path to the units so they know the new path when they start walking
                        Unit.Path = newPath;
                    }
                    // Only enter if there is nothing on that position of the map and you have the
                    // not pressed the delete tower button
                    else if (map.GetIndex(cellX, cellY) == 0 && newTowerType != 3)
                    {
                        // only enter if you have enough gold
                        if (gold >= Tower.GetCost(newTowerType))
                        {
                            gold -= Tower.GetCost(newTowerType);
                            // Add tower to the map
                            map.AddToMap(cellX, cellY, newTowerType);
                            Pathfinder p = new Pathfinder(map);
                            List<Vector2> t = p.FindPath(new Point(0, 0), new Point(map.Width - 1, map.Height - 1));

                            // After checking new path. if count == 0 the path is blocked off and there is no way
                            // for the creeps to go. So remove the point added to the map refund money and tell the
                            // player
                            if (t.Count == 0)
                            {
                                map.AddToMap(cellX, cellY, 0);
                                toast.AddMessage("ಠ_ಠ Du är for dårlig at bygga tårn.");
                                gold += Tower.GetCost(newTowerType);
                            }
                            else
                            {
                                // Add new path to pathview and the units
                                path.Path = t;
                                Unit.Path = t;
                                // add new tower object to the array
                                towers[cellX, cellY] = new Tower(newTowerType, cellX, cellY);
                            }
                        }
                        else
                        {
                            // Player does not have enough gold tell them
                            toast.AddMessage("(╯°□°）╯︵ ʎǝuoɯ ǝɹoɯ ou");
                        }
                    }
                }else if (towers[cellX, cellY] != null && newTowerType == 0 && towers[cellX, cellY].Type != 1)
                {
                    // Select tower you want to upgrade
                    selectedTower = towers[cellX, cellY];
                }
            }
            if (mouse.ReleaseRightClick)
            {
                // Deselect towers you want to upgrade or build.
                newTowerType = 0;
                selectedTower = null;
            }
            // Go through the array of the tower objects to tell them the new position of the enemies
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
            // Draw the tower you have selected to place out or the standard hover tile
            spriteBatch.Draw(tile, new Vector2(cellX * 32, (cellY * 32) + 24), new Color(255, 255, 255, 50));
            // draw all the towerobjects
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
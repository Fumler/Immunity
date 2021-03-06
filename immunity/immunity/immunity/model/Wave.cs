﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class Wave
    {
        private int waveNumber = 0;
        private int numOfEnemies = 0;
        private int enemiesSpawned = 0;

        private float spawnTimer;

        private bool spawingEnemies = false;
        private int enemiesAtEndOfPath;

        private List<int> enemyTypes;
        public List<Unit> enemies = new List<Unit>();

        //Accessors
        public bool WaveFinished
        {
            get { return (enemies.Count == 0 && numOfEnemies == enemiesSpawned) && numOfEnemies != 0; }
        }

        public bool WaveStarted
        {
            get { return (enemiesSpawned > 0); }
        }

        public int EnemyAtEndOfPath
        {
            get { return enemiesAtEndOfPath; }
        }

        public bool SpawningEnemies
        {
            get { return spawingEnemies; }
        }

        //Constructor
        public Wave(List<int> enemyTypes, int waveNumber)
        {
            this.numOfEnemies = enemyTypes.Count;
            this.enemyTypes = enemyTypes;
            this.waveNumber = waveNumber;
        }

        //Methods
        private void AddEnemy(int unitType)
        {
            Unit enemy = new Unit(unitType, waveNumber);
            enemies.Add(enemy);
            spawnTimer = 0;
            enemiesSpawned++;
        }

        public void Start()
        {
            spawingEnemies = true;
        }

        public void Update(GameTime gameTime)
        {
            if (enemiesSpawned == numOfEnemies)
            {
                spawingEnemies = false;
            }

            if (spawingEnemies)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (spawnTimer > 0.5f)
                {
                    AddEnemy(enemyTypes[enemiesSpawned]);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update();
                if (enemies[i].IsDead)
                {
                    if (enemies[i].Health > 0)
                    {
                        enemiesAtEndOfPath++;
                    }

                    enemies.Remove(enemies[i]);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Unit enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }
    }
}
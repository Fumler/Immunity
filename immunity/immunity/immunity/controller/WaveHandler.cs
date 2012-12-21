using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class WaveHandler
    {
        private int waveNumber;
        private int numberOfWaves;
        private Queue<Wave> waves = new Queue<Wave>();

        //Accessors
        public Wave GetCurrentWave()
        {
            if (waves.Count > 0)
            {
                return waves.Peek();
            }
            else
            {
                return new Wave(new List<int> { }, 0); // Dirty fix to stop crashes after last wave
            }
        }

        public int WaveNumber
        {
            get { return waveNumber; }
        }

        //Constructors
        public WaveHandler(List<int>[] enemies)
        {
            this.numberOfWaves = enemies.Length;
            waveNumber = 1;

            for (int i = 0; i < numberOfWaves; i++)
            {
                Wave wave = new Wave(enemies[i], i + 1);
                waves.Enqueue(wave);
            }
        }

        //Methods
        public void StartNextWave()
        {
            if (waves.Count > 0)
            {
                GetCurrentWave().Start();
            }
        }

        public void Update(GameTime gameTime)
        {
            GetCurrentWave().Update(gameTime);

            if (GetCurrentWave().WaveFinished)
            {
                waves.Dequeue();
                waveNumber++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (waves.Count > 0)
            {
                GetCurrentWave().Draw(spriteBatch);
            }
        }
    }
}
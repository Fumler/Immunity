using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class WaveHandler
    {
        private int numberOfWaves;
        private Queue<Wave> waves = new Queue<Wave>();

        //Accessors
        public Wave CurrentWave
        {
            get { return waves.Peek(); }
        }

        //Constructors
        public WaveHandler(List<int>[] enemies)
        {
            this.numberOfWaves = enemies.Length;

            for (int i = 0; i < numberOfWaves; i++)
            {
                Wave wave = new Wave(enemies[i]);
                waves.Enqueue(wave);
            }
        }

        //Methods
        public void StartNextWave()
        {
            CurrentWave.Start();
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentWave.WaveFinished)
            {
                waves.Dequeue();
            }

            CurrentWave.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
        }
    }
}

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
        public Wave GetCurrentWave()
        {
            if (waves.Count > 0)
            {
                return waves.Peek();
            }
            else
            {
               return new Wave(new List<int>{});
            }
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

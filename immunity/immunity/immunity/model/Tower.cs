﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    class Tower
    {
        protected int cost;
        protected int damage;
        protected int level;
        protected int range;

        public int Cost
        {
            get { return cost; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public int Level
        {
            get { return level; }
        }

        public int Range
        {
            get { return range; }
        }

        public Tower()
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {

        }

        public void Update()
        { 
        
        }

        public void Shoot()
        { 
            
        }
    }
}

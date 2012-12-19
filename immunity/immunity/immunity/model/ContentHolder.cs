using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace immunity
{
    class ContentHolder
    {
        private static Texture2D[] towerTextures;
        private static List<Texture2D> unitSprites, guiSprites, buttons, ammunitionSprites;
        private static List<SpriteFont> fonts;
        private static List<Song> songs;
        private static List<SoundEffect> sounds;

        public static Texture2D[] TowerTextures {
            get { return towerTextures; }
        }

        public static List<Texture2D> UnitSprites {
            get { return unitSprites; }
        }

        public static List<Texture2D> GuiSprites
        {
            get { return guiSprites; }
        }

        public static List<Texture2D> Buttons
        {
            get { return buttons; }
        }

        public static List<Texture2D> AmmunitionSprites
        {
            get { return ammunitionSprites; }
        }

        public static List<SpriteFont> Fonts
        {
            get { return fonts; }
        }

        public static void Initialize()
        {   
            towerTextures = new Texture2D[33];
        }

        public static void Load(ContentManager Content)
        {
            towerTextures[0] = Content.Load<Texture2D>("sprites\\bg");
            towerTextures[1] = Content.Load<Texture2D>("sprites\\tumor");
            towerTextures[2] = Content.Load<Texture2D>("sprites\\hoverTile");
            towerTextures[3] = Content.Load<Texture2D>("sprites\\deleteTile");
            towerTextures[4] = Content.Load<Texture2D>("sprites\\path");
            towerTextures[9] = Content.Load<Texture2D>("sprites\\towers\\turret");
            towerTextures[10] = Content.Load<Texture2D>("sprites\\towers\\Tier1Ranged");
            towerTextures[11] = Content.Load<Texture2D>("sprites\\towers\\Tier2Ranged");
            towerTextures[20] = Content.Load<Texture2D>("sprites\\towers\\Tier1Splash");
            towerTextures[21] = Content.Load<Texture2D>("sprites\\towers\\Tier2Splash");

            unitSprites = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\RedCell"),
                Content.Load<Texture2D>("sprites\\WhiteCell")
            };

            guiSprites = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\actionbar"),
                Content.Load<Texture2D>("sprites\\topbar"),
                Content.Load<Texture2D>("sprites\\cursor"),
                Content.Load<Texture2D>("sprites\\gametitle")
            };

            buttons = new List<Texture2D>() {
                Content.Load<Texture2D>("sprites\\buttons\\unit1"),
                Content.Load<Texture2D>("sprites\\buttons\\tower1"),
                Content.Load<Texture2D>("sprites\\buttons\\tower1upgrade1"),
                Content.Load<Texture2D>("sprites\\buttons\\tower2"),
                Content.Load<Texture2D>("sprites\\buttons\\tower2upgrade1"),
                Content.Load<Texture2D>("sprites\\buttons\\delete"),
                Content.Load<Texture2D>("sprites\\buttons\\newwave"),
                Content.Load<Texture2D>("sprites\\buttons\\newgame"),
                Content.Load<Texture2D>("sprites\\buttons\\multiplayer"),
                Content.Load<Texture2D>("sprites\\buttons\\controls"),
                Content.Load<Texture2D>("sprites\\buttons\\exit"),
                Content.Load<Texture2D>("sprites\\blackbox")
            };

            fonts = new List<SpriteFont>() {
                Content.Load<SpriteFont>("fonts\\segoe"),
                Content.Load<SpriteFont>("fonts\\miramonte"),
                Content.Load<SpriteFont>("fonts\\miramonteBig")
            };

            ammunitionSprites = new List<Texture2D>();
            for (int i = 0; i <= 33; i++)
            {
                Texture2D temp = Content.Load<Texture2D>("sprites\\Ammo");
                ammunitionSprites.Add(temp);
            }

            songs = new List<Song>() {
                Content.Load<Song>("sounds//song")
            };
        }
    }
}

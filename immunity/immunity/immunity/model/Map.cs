using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    /// <summary>
    /// A physical representation of the map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// A constant that defines how big one tile is.
        /// </summary>
        public const int TILESIZE = 32;


        /// <summary>
        /// The map weight.
        /// </summary>
        private int[,] mapArray = new int[,]
        {
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 1, }, 
            { 1, 1, 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, },
            { 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 1, 0, }, 
            { 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, }, 
            { 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 1, },
            { 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, }, 
            { 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, }, 
            { 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, }, 
            { 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, }, 
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, },
            { 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, },
            { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, },
            { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, },
            { 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, },
            { 1, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, },
        };

        private List<Texture2D> textures;

        /// <summary>
        /// The width of the map.
        /// </summary>
        public int width
        {
            get { return mapArray.GetLength(1); }
        }
        /// <summary>
        /// The height of the map.
        /// </summary>
        public int height
        {
            get { return mapArray.GetLength(0); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Map() {
        }

        /// <summary>
        /// Sets the textures for the map to draw.
        /// </summary>
        public void setTextures(List<Texture2D> textures)
        {
            this.textures = textures;
        }

        /// <summary>
        /// Returns the tile index for the given cell.
        /// </summary>
        public int getIndex(int cellX, int cellY)
        {
            if (cellX < 0 || cellX > width - 1 || cellY < 0 || cellY > height - 1)
                return 0;

            return mapArray[cellY, cellX];
        }

        /// <summary>
        /// Draws the map.
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            if (textures == null)
            {
                return;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = mapArray[y, x];

                    spriteBatch.Draw(textures[index], new Vector2(x, y) * TILESIZE, Color.White);
                }
            }
        }

        public void loadMap(String mapName, int mapX, int mapY) {
            mapArray = new int[mapX, mapY];

            using (StreamReader streamReader = new StreamReader(mapName)) {
                int x = 0;
                int y = 0;

                while (!streamReader.EndOfStream) {
                    string line = streamReader.ReadLine();
                    string[] numbers = line.Split(',');

                    foreach (string s in numbers)
                    {
                        int tile = int.Parse(s);
                        mapArray[x,y] = tile;
                        x++;
                    }

                    y++;
                }
            }
        }
    }
}

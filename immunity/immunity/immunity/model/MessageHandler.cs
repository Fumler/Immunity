using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace immunity
{
    internal class MessageHandler
    {
        private SpriteFont font;
        private Texture2D texture;

        private List<TimeSpan> timeToLive = new List<TimeSpan>();
        private List<String> message = new List<String>();
        private List<Vector2> position = new List<Vector2>();

        private TimeSpan gameTime;
        private int screenHeight, screenWidth;

        public void InitVars(Texture2D texture, SpriteFont font)
        {
            this.texture = texture;
            this.font = font;
        }

        public MessageHandler(int width, int height)
        {
            this.screenHeight = height;
            this.screenWidth = width;
        }

        public void Update(TimeSpan gameTime)
        {
            this.gameTime = gameTime;
            if (message.Count > 0)
            {
                for (int i = 0; i < message.Count; i++)
                {
                    if (gameTime > timeToLive[i])
                    {
                        timeToLive.Remove(timeToLive[i]);
                        message.Remove(message[i]);
                        position.Remove(position[i]);
                        i--;
                    }
                }
            }
        }

        public void AddMessage(string text)
        {
            this.timeToLive.Add(gameTime + new TimeSpan(0, 0, 3));
            this.message.Add(text);
            Vector2 pos = new Vector2();
            pos.X = (int)((screenWidth / 2) - font.MeasureString(text).X * 0.5);
            pos.Y = screenHeight / 2 - 20;
            this.position.Add(pos);
        }
        public void AddMessage(string text, TimeSpan timeTilDeath)
        {
            this.timeToLive.Add(gameTime + timeTilDeath);
            this.message.Add(text);
            Vector2 pos = new Vector2();
            pos.X = (int)((screenWidth / 2) - font.MeasureString(text).X * 0.5);
            pos.Y = screenHeight / 2 - 20;
            this.position.Add(pos);
        }

        public void AddMessage(string text, int x, int y)
        {
            this.timeToLive.Add(gameTime + new TimeSpan(0, 0, 3));
            this.message.Add(text);
            this.position.Add(new Vector2(x, y));
        }

        public void AddMessage(string text, TimeSpan timeTilDeath, int x, int y)
        {
            this.timeToLive.Add(gameTime + timeTilDeath);
            this.message.Add(text);
            this.position.Add(new Vector2(x, y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (message.Count > 0)
            {
                for (int i = 0; i < message.Count; i++)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)position[i].X - 10, (int)position[i].Y + (40 * i), (int)font.MeasureString(message[i]).X + 20, 40), Color.Black);
                    spriteBatch.DrawString(font, message[i], new Vector2(position[i].X, position[i].Y + (40 * i)), Color.White);
                }
            }
        }
    }
}
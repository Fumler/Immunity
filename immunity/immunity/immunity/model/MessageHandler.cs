using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace immunity
{
    class MessageHandler
    {
        SpriteFont font;
        Texture2D texture;

        List<TimeSpan> timeToLive = new List<TimeSpan>();
        List<String> message = new List<String>();
        List<Vector2> position = new List<Vector2>();

        TimeSpan gameTime;
        int screenHeight, screenWidth;

        public void InitVars(Texture2D texture, SpriteFont font) {
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
                        System.Diagnostics.Debug.WriteLine(timeToLive[i]);
                        timeToLive.Remove(timeToLive[i]);
                        message.Remove(message[i]);
                        position.Remove(position[i]);
                        i--;
                    }
                }
            }
        }
        public void AddMessage(String text, TimeSpan timeTilDeath)
        {
            TimeSpan temp = gameTime + timeTilDeath;
            this.timeToLive.Add(temp);
            this.message.Add(text);
            Vector2 pos = new Vector2();
            pos.X = (int)((screenWidth / 2) - font.MeasureString(text).X * 0.5);
            pos.Y = screenHeight / 2 - 20;
            this.position.Add(pos);
            System.Diagnostics.Debug.WriteLine("Added toast" + message.Count);
        }
        public void AddMessage(String text, TimeSpan timeTilDeath, int x, int y)
        {
            TimeSpan temp = gameTime + timeTilDeath;
            this.timeToLive.Add(temp);
            this.message.Add(text);
            this.position.Add(new Vector2(x, y));
            System.Diagnostics.Debug.WriteLine("Added toast" + message.Count);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (message.Count > 0)
            {
                for (int i = 0; i < message.Count; i++)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)position[i].X - 10, (int)position[i].Y, (int)font.MeasureString(message[i]).X + 20, 40), Color.Black);
                    spriteBatch.DrawString(font, message[i], position[i], Color.White);
                }
            }
        }
    }


}

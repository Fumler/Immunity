using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace immunity
{
    public class StorageHandler
    {
        [Serializable]
        public struct SaveGameData
        {
            public int lives;
            public int gold;
            public int wave;
            public int[] map;
        }

        private StorageDevice device;
        private String containerName;
        private String fileName;
        private SaveGameData saveGameData;

        public StorageHandler()
        {
        }

        public void SaveGame(SaveGameData saveGameData, string containerName, string fileName)
        {
            device = null;
            this.saveGameData = saveGameData;
            this.containerName = containerName;
            this.fileName = fileName;

            try
            {
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.SaveToFile, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SaveToFile(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);

            // Open a storage container.
            IAsyncResult r = device.BeginOpenContainer(containerName, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(r);
            result.AsyncWaitHandle.Close();

            // Delete old file and create new one.
            if (container.FileExists(fileName))
            {
                container.DeleteFile(fileName);
            }
            Stream fileStream = container.CreateFile(fileName);

            // Write data to file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(fileStream, saveGameData);

            // Close file.
            fileStream.Close();
            container.Dispose();
        }
    }
}
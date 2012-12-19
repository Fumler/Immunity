using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace immunity
{
    class Network
    {
        private TcpClient connection;
        private bool connected = false;
        private MessageHandler toastnet;
        private Thread netmsgs;
        private bool runThread = true;

        public event EventHandler received;
        public delegate void EventHandler(string n);

        public void Toast(ref MessageHandler messageHandler)
        {
            toastnet = messageHandler;
        }

        public void ConnectToServer()
        {
            try
            {
                connection = new TcpClient();
                connection.Connect("127.0.0.1", 8080);
                connected = true;
                toastnet.AddMessage("Connected to server!", new TimeSpan(0, 0, 3), 10, 10);
                System.Diagnostics.Debug.WriteLine("Connected");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("No connection to server");
                //toastnet.AddMessage("Could not connect to server!", new TimeSpan(0, 0, 3), 10, 10);
            }
        }
        public Network()
        {
            Thread net = new Thread(new ThreadStart(startConnecting));
            net.Start();
        }
        private void startConnecting()
        {
            do
            {
                Thread.Sleep(3000);
                ConnectToServer();
            } while (!connected);
            netmsgs = new Thread(new ThreadStart(Receive));
            netmsgs.Start();
        }
        public void Receive()
        {
            StreamReader reader = new StreamReader(connection.GetStream());

            string reply = null;
            try
            {
                while (runThread)
                {
                    System.Diagnostics.Debug.WriteLine("Thread");

                    reply = reader.ReadLine();
                    toastnet.AddMessage(reply, new TimeSpan(0, 0, 3), 10, 10);
                    if (reply != "")
                    {
                        string[] action = reply.Split(new string[] { ";" }, StringSplitOptions.None);
                        switch (action[0])
                        {
                            case "sysmsg":
                                toastnet.AddMessage(action[1], new TimeSpan(0, 0, 3), 10, 10);
                                break;
                            default:
                                received(reply);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("OH NOES");
                toastnet.AddMessage("Lost connection", new TimeSpan(0, 0, 3), 10, 10);
            }
        }
        public void Deliver(string msg)
        {
            if (!connected)
                ConnectToServer();
            try
            {
                StreamWriter writer = new StreamWriter(connection.GetStream());
                writer.WriteLine(msg);
                writer.Flush();
            }
            catch (Exception e)
            {
                connected = false;
                System.Diagnostics.Debug.WriteLine("Not connected to server.");
            }
        }
        public void Disconnect()
        {
            connection.Close();
        }
    }
}

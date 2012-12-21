using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace immunity
{
    class Network
    {
        private TcpClient connection;
        private bool connected = false;
        private bool running = true;
        private MessageHandler toastnet;
        private Thread netmsgs, net;

        public event EventHandler received;
        public delegate void EventHandler(string n);

        public bool Connected
        {
            get { return connected; }
        }

        public void Init(ref MessageHandler messageHandler)
        {
            toastnet = messageHandler;
            net = new Thread(new ThreadStart(ConnectToServer));
            net.Start();
        }
        public void Retry()
        {
            if (!net.IsAlive)
            {
                net = new Thread(new ThreadStart(ConnectToServer));
                net.Start();
            }
        }
        public void ConnectToServer()
        {
            toastnet.AddMessage("in!", new TimeSpan(0, 0, 3));
            try
            {
                connection = new TcpClient();
                connection.Connect("whg.no", 7707);
                netmsgs = new Thread(new ThreadStart(Receive));
                netmsgs.Start();
                connected = true;
                toastnet.AddMessage("Connected to server!", new TimeSpan(0, 0, 3), 10, 10);
                System.Diagnostics.Debug.WriteLine("Connected");
            }
            catch (Exception e)
            {
                connected = false;
                System.Diagnostics.Debug.WriteLine("No connection to server");
                //toastnet.AddMessage("Could not connect to server!", new TimeSpan(0, 0, 3), 10, 10);
            }
            toastnet.AddMessage("OUT!", new TimeSpan(0, 0, 3));
        }
        public Network()
        {
        }
        public void Receive()
        {
            StreamReader reader = new StreamReader(connection.GetStream());

            string reply = null;
            while (connected)
            {
                System.Diagnostics.Debug.WriteLine("Thread");
                try
                {
                    reply = reader.ReadLine();
                }
                catch (Exception e)
                {
                    reply = null;
                }
                if (reply != null)
                {
                    toastnet.AddMessage(reply, new TimeSpan(0, 0, 3), 10, 10);
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
                else
                {
                    connected = false;
                }
            }
            toastnet.AddMessage("STOPPED!", new TimeSpan(0, 0, 3));
        }
        public void Deliver(string msg)
        {
            if (!connected)
                Retry();
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
            running = false;
            if (connected)
                connection.Close();
        }
    }
}

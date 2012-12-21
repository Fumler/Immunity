using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace immunity
{
    internal class Network
    {
        private TcpClient connection;
        private StreamReader reader;
        private StreamWriter writer;
        private bool connected = false;
        private MessageHandler toastnet;
        private Thread netmsgs, connect;

        public event EventHandler received;

        public delegate void EventHandler(string n);

        public Network()
        {
        }

        /// <summary>
        /// Returns a bool which tells you if you are connected to the master server
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// Adds the networkMessage MessageHandler-object so you can show when you have
        /// connected to the master server or not
        /// It also starts a thread which is going start trying to connect to the master server
        /// I put it in its own thread so that the whole game stops until it connects or
        /// throws an exception
        /// </summary>
        public void Init(ref MessageHandler messageHandler)
        {
            toastnet = messageHandler;
            connect = new Thread(new ThreadStart(ConnectToServer));
            connect.IsBackground = true;
            connect.Start();
        }

        /// <summary>
        /// Tries to connect to the master server again on user request
        /// It does not run if the connect thread is running. Which it only
        /// is if it is trying to connect.
        /// </summary>
        public void Retry()
        {
            if (!connect.IsAlive)
            {
                connect = new Thread(new ThreadStart(ConnectToServer));
                connect.IsBackground = true;
                connect.Start();
            }
        }

        /// <summary>
        /// Tries to connect to the master server. If it is successful it will start a new thread
        /// which is the Recieve function, if not it throws an exception and stops the connect
        /// thread.
        /// </summary>
        public void ConnectToServer()
        {
            try
            {
                connection = new TcpClient();
                connection.Connect("whg.no", 7707);
                netmsgs = new Thread(new ThreadStart(Receive));
                netmsgs.IsBackground = true;
                netmsgs.Start();
                connected = true;
                toastnet.AddMessage("Connected to server!", 10, 10);
            }
            catch (Exception e)
            {
                connected = false;
                System.Diagnostics.Debug.WriteLine("No connection to server");

                //toastnet.AddMessage("Could not connect to server!", 10, 10);
            }
        }

        /// <summary>
        /// Is in a thread and will wait for a message sent from the master server.
        /// It then fires an event which triggers a function that will parse the information.
        /// unless it's a system message, it will then trigger a MessageHandler.
        /// </summary>
        public void Receive()
        {
            reader = new StreamReader(connection.GetStream());

            string reply = null;
            while (connected)
            {
                try
                {
                    reply = reader.ReadLine();
                }
                catch (Exception e)
                {
                    reply = null;
                    connected = false;
                }
                if (reply != null)
                {
                    string[] action = reply.Split(new string[] { ";" }, StringSplitOptions.None);
                    switch (action[0])
                    {
                        case "sysmsg":
                            toastnet.AddMessage(action[1], 10, 10);
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
            toastnet.AddMessage("Lost connection!", 10, 10);
        }

        /// <summary>
        /// Delivers a message from the client to the server.
        /// </summary>
        public void Deliver(string msg)
        {
            if (!connected)
                Retry();
            try
            {
                writer = new StreamWriter(connection.GetStream());
                writer.WriteLine(msg);
                writer.Flush();
            }
            catch (Exception e)
            {
                connected = false;
                System.Diagnostics.Debug.WriteLine("Not connected to server.");
            }
        }

        /// <summary>
        /// Is called when exit game button is pressed. It shuts down the Receive
        /// stream, which then stops the thread, and stops the connection to the master server, 
        /// </summary>
        public void Disconnect()
        {
            if (connected)
            {
                reader.Close();
                writer.Close();
                connection.Close();
            }
        }
    }
}

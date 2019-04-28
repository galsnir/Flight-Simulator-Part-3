using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator.Model
{
    class InfoServer
    {
        private TcpClient client;
        private BinaryReader reader;
        private TcpListener listener;
        public bool stop = false;
        public bool isConnected = false;

        #region Singleton
        private static InfoServer m_Instance = null;
        public static InfoServer Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new InfoServer();
                }
                return m_Instance;
            }
        }
        #endregion

        // This method reset the singelton value to be null
        public void Clear()
        {
            m_Instance = null;
        }


        // This method connects the simulator to the program
        public void Connect(string ip, int port)
        {
            if (isConnected)
                Disconnect();
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            isConnected = true;
            Console.WriteLine("You are connected");
        }

        // This menthod get the data form the simulator, praser the lon and lat data from it and passes
        // it on to the FlightBoardViewModel
        public void StartReading()
        {
            new Task(() =>
            {
                char c;
                string line = "";
                client = listener.AcceptTcpClient();
                reader = new BinaryReader(client.GetStream());
                while (!stop)
                {                
                    line = "";
                    while ((c = reader.ReadChar()) != '\n') line += c;
                    string[] values = line.Split(',');
                
                    Console.WriteLine(values[0] + " " + values[1]);
                    FlightBoardViewModel.Instance.Lat = Convert.ToDouble(values[0]);
                    FlightBoardViewModel.Instance.Lon = Convert.ToDouble(values[1]);
                }
                //return args;
            }).Start();
        }

        // This method disconnect the flight simulator from the program
        public void Disconnect()
        {
            isConnected = false;
            stop = true;
            if (client != null)
                client.Close();
            if (listener != null)
                listener.Stop();
        }
    }
}

using FlightSimulator.Model.Interface;
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

    public class CommandClient
    {
        TcpClient client;
        BinaryWriter writer;
        public bool isConnected = false;

        #region Singleton
        private static CommandClient m_Instance = null;
        public static CommandClient Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CommandClient();
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

        // This method connects the program to the simulator server
        public void Connect(string ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new TcpClient();

            // We try to connect again and again util the connection is made
            while (!client.Connected)
            {
                try { client.Connect(ep); }
                catch (Exception) { }
            }

            Console.WriteLine("You are connected");
            isConnected = true;
            writer = new BinaryWriter(client.GetStream());
        }

        // This method sends a manual command to the simulator server
        public void SendManual(string command)
        {
            if (string.IsNullOrEmpty(command)) return;
            string buffer = command + "\r\n";
            writer.Write(Encoding.ASCII.GetBytes(buffer));
        }

        // This method sends an auto-pilot command to the simulator server
        public void SendAutoPilot(string command)
        {
            if (string.IsNullOrEmpty(command)) return;
            // We split the commands by the \n character and send them to the simulator
            string[] commandList = command.Split('\n');
            foreach (string com in commandList)
            {
                string buffer = com + "\r\n";
                writer.Write(Encoding.ASCII.GetBytes(buffer));
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}



using FlightSimulator.Model;
using FlightSimulator.Properties;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        #region Singleton
        private static FlightBoardViewModel m_Instance = null;
        public static FlightBoardViewModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightBoardViewModel();
                }
                return m_Instance;
            }
        }
        #endregion

        private double lon;
        private double lat;
        private InfoServer server;
        private ICommand connectsCommand;

        public FlightBoardViewModel()
        {
            server = new InfoServer();
        }

        public double Lon
        {

            get { return lon; }

            set
            {
                lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        public double Lat
        {

            get { return lat; }

            set
            {
                lat = value;
                NotifyPropertyChanged("Lat");
            }
        }

        // We create the connect command if it deosn't exist
        public ICommand ConnectsCommand {
            get {
                return connectsCommand ?? (connectsCommand = new CommandHandler(() => OnConnectClick())); }
            }
        
        void OnConnectClick()
        {
            // We check if there is an already activating connection, reset it and reconnect.
            if (server.isConnected)
            {
                // We stop the connection.
                server.stop = true;
                // We reset it.
                CommandClient.Instance.Clear();
                // We let the thread finish.
                System.Threading.Thread.Sleep(1000);
            }
            
            // We the new connection on it on a task
            new Task(() =>
            {
                CommandClient.Instance.Connect(ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightCommandPort);
            }).Start();

            // We connect to the the simulator and start reading data from it
            server.Connect(ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightInfoPort);
            server.StartReading();
        }
    }
}

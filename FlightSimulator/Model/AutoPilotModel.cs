using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class AutoPilotModel
    {
        // This method opens a new task and sends a command to the simulator through the command client class
        public void Send(string command)
        {
            if (CommandClient.Instance.isConnected)
            {
                new Task(delegate ()
                {
                    CommandClient.Instance.SendAutoPilot(command);
                }).Start();
            }
        }
    }
}

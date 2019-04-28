using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    class ManualViewModel
    {
        ManualModel M_model = new ManualModel();

        public double Throttle
        {
            set {
                M_model.Send("set /controls/engines/current-engine/throttle " + Convert.ToString(value));
            }
        }

        public double Rudder
        {
            set
            {
                M_model.Send("set /controls/flight/rudder " + Convert.ToString(value));
            }
        }

        public double Elevator
        {
            set
            {
                M_model.Send("set /controls/flight/elevator " + Convert.ToString(value));                
            }
        }

        public double Aileron
        {
            set
            {
                M_model.Send("set /controls/flight/aileron " + Convert.ToString(value));
            }
        }
    }
}

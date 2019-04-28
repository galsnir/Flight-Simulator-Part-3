using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : INotifyPropertyChanged
    {
        private string commands;
        private Brush background_color;
        private ICommand okButtonCommand;
        private ICommand clearButtonCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        private AutoPilotModel APM_model = new AutoPilotModel();

        public string Commands
        {
            get { return commands; }
            set
            {
                commands = value;
                // If the textbox is not empty we set the background color of the textbox to be pink
                if (!string.IsNullOrEmpty(commands))
                {
                    if(background_color == null)
                    {
                        background_color = Brushes.White;
                    }

                    if (background_color == Brushes.White)
                    {
                        Background_color = Brushes.LightPink;
                    }
                }
                // If the textbox is empty we set the background color of the textbox to be white
                else
                {
                    Background_color = Brushes.White;
                }
            }
        }

        public Brush Background_color
        {
            get
            {
                return background_color;
            }
            set
            {
                background_color = value;
                // If the background_color value has changed we will notify the auto-piot view to make it change the
                // the text box color on the GUI
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Background_color"));
            }
        }


        public ICommand OkButtonCommand
        {
            get
            {
                // We return the command, otherwise we create a new command
                return okButtonCommand ?? (okButtonCommand = new CommandHandler(() =>
                {
                    // We save the string that we will later send to the simulator
                    string inputText = Commands;
                    // We clear the auto-pilot text box
                    Commands = "";
                    // We notify the auto-pilot view about the new change
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Commands));
                    // We reset the background to white
                    background_color = Brushes.White;
                    // We send the commands to the simulator through the auto-pilot model
                    APM_model.Send(inputText);
                }));
            }
        }



        public ICommand ClearCommand
        {
            get
            {
                // We return the command, otherwise we create the command
                return clearButtonCommand ?? (clearButtonCommand = new CommandHandler(() =>
                {
                    // We clear the auto-pilot text box
                    Commands = "";
                    // We reset the background to white
                    background_color = Brushes.White;
                    // We notify the auto-pilot view about the new change
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Commands));
                }));
            }
        }
    }
}

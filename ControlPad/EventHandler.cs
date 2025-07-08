using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ControlPad
{
    public class EventHandler
    {
        private AudioController auCo;
        private ArduinoController ardCo;
        private Dictionary<Control, int> values = new Dictionary<Control, int>();       

        private MainWindow MainWindow;

        public EventHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            auCo = new AudioController();
        }
        public void Update(Dictionary<Control, int> values)
        {
            foreach (var kvp in values)
            {
                Control key = kvp.Key;
                var name = MainWindow.Dispatcher.Invoke(() => key.Name);

                int newValue = kvp.Value;
                this.values.TryGetValue(key, out int oldValue);

                if (name.StartsWith("Slider", StringComparison.OrdinalIgnoreCase) && Math.Abs(oldValue - newValue) > 2)
                {
                    UpdateSlider(key, newValue);
                    auCo.SetProcessVolume("Spotify", 0.5f);
                }
                else if (name.StartsWith("Switch", StringComparison.OrdinalIgnoreCase))
                {
                    UpdateButton(key, newValue);
                }
                this.values[key] = newValue;
            }
        }

        private void UpdateSlider(Control element, int value)
        {
            MainWindow.Dispatcher.Invoke(() => MainWindow.UpdateUISlider((Slider)element, value));
        }
        private void UpdateButton(Control element, int value)
        {
            // Code to update button state based on value
        }
    }
}

using System.Windows.Controls;

namespace ControlPad
{
    public class EventHandler
    {
        private AudioController AudioController;
        private Dictionary<Control, int> values = new Dictionary<Control, int>();       

        private MainWindow MainWindow;

        public EventHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            AudioController = new AudioController();
        }
        public void Update(Dictionary<Control, int> values)
        {
            foreach (var kvp in values)
            {
                Control key = kvp.Key;
                var name = MainWindow.Dispatcher.Invoke(() => key.Name);

                int newValue = kvp.Value;
                this.values.TryGetValue(key, out int oldValue);

                if (name.StartsWith("Slider", StringComparison.OrdinalIgnoreCase) && Math.Abs(oldValue - newValue) > 1)
                {
                    UpdateSlider(key, newValue);
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

            Task.Run(() => AudioController.SetProcessVolume("Spotify", SliderToFloat(value)));
        }
        private void UpdateButton(Control element, int value)
        {
            // Code to update button state based on value
        }
        private float SliderToFloat(int value)
        {
            value -= 2;
            return (float)value / 1020.0f;
        }

    }
}

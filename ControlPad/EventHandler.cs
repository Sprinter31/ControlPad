using System.Windows.Controls;

namespace ControlPad
{
    public class EventHandler
    {
        private AudioController audioController;
        private Dictionary<Control, int> values = new Dictionary<Control, int>();       

        private MainWindow MainWindow;

        public EventHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            audioController = new AudioController();
        }
        public void Update(Dictionary<Control, int> values)
        {
            foreach (var kvp in values)
            {
                Control key = kvp.Key;
                var name = MainWindow.Dispatcher.Invoke(() => key.Name);

                int newValue = kvp.Value;
                this.values.TryGetValue(key, out int oldValue);

                if (name.StartsWith("Slider", StringComparison.OrdinalIgnoreCase) /*&& Math.Abs(oldValue - newValue) > 1*/)
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

        private void UpdateSlider(Control slider, int value)
        {
            MainWindow.Dispatcher.Invoke(() => MainWindow.UpdateUISlider((Slider)slider, value));

            CategorySlider categorySlider = (CategorySlider)slider;
            if (categorySlider.Category != null)
                foreach(string processName in categorySlider.Category.Processes)
                    Task.Run(() => audioController.SetProcessVolume(processName, SliderToFloat(value)));          
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

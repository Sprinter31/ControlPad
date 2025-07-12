using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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

            int.TryParse(MainWindow.Dispatcher.Invoke(() => element.Name).Replace("Slider", ""), out int nr);

            var assignment = DataHandler.SliderAssignments.FirstOrDefault(a => a.SliderNr == nr);

            if (assignment == null) return;

            int categoryId = assignment.CategoryId;

            var category = DataHandler.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null) return;

            var programms = category.Programms;

            foreach (var p in programms)
            {
                Task.Run(() => AudioController.SetProcessVolume(p, SliderToFloat(value)));
            }

        }
        private void UpdateButton(Control element, int value)
        {
            // Code to update button state based on value
        }
        private float SliderToFloat(int value)
        {
            value -= 1;
            return Math.Min((float)value / 1022.0f, 1);
        }

    }
}

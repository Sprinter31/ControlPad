using System.Windows.Controls;

namespace ControlPad
{
    public class EventHandler
    {
        private AudioController AudioController; 
        private HomeUserControl HomeUserControl;
        private List<(CustomSlider Slider, int Value)>? oldSliderValues;
        private List<(CustomButton Button, int Value)>? oldButtonValues;

        public EventHandler(HomeUserControl homeUserControl)
        {
            HomeUserControl = homeUserControl;
            AudioController = new AudioController();
        }

        public void Update(List<(CustomSlider Slider, int Value)> currentSliderValues,
                           List<(CustomButton Button, int Value)> currentButtonValues)
        {
            for (int i = 0; i < currentSliderValues.Count; i++)
            {
                if (oldSliderValues != null && Math.Abs(oldSliderValues[i].Value - currentSliderValues[i].Value) > 1)
                {
                    UpdateSlider(currentSliderValues[i].Slider, currentSliderValues[i].Value);
                }                
            }
            oldSliderValues = currentSliderValues.Select(t => (t.Slider, t.Value)).ToList();

            for (int i = 0; i < currentButtonValues.Count; i++)
            {
                if(oldButtonValues != null)
                {
                    UpdateButton(currentButtonValues[i].Button, currentButtonValues[i].Value, oldButtonValues[i].Value);
                }                
            }
            oldButtonValues = currentButtonValues.Select(t => (t.Button, t.Value)).ToList();
        }

        private void UpdateSlider(CustomSlider slider, int value)
        {
            HomeUserControl.Dispatcher.Invoke(() => HomeUserControl.UpdateUISlider(slider, value));

            if (slider.Category != null)
                foreach(string processName in slider.Category.Processes)
                    Task.Run(() => AudioController.SetProcessVolume(processName, SliderToFloat(value)));          
        }

        private void UpdateButton(CustomButton button, int currentValue, int oldValue)
        {
            if(currentValue != oldValue)
            {
                HomeUserControl.Dispatcher.Invoke(() => HomeUserControl.UpdateUIButtons(button, currentValue == 1));
            }           
        }

        private float SliderToFloat(int value)
        {
            value -= 1;
            return Math.Min((float)value / 1022.0f, 1);
        }
    }
}

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace ControlPad
{
    public class EventHandler
    {
        private AudioController AudioController; 
        private HomeUserControl HomeUserControl;
        private List<(CustomButton Button, int Value)>? oldButtonValues;
        private Dictionary<CustomSlider, int> oldSliderValues = new Dictionary<CustomSlider, int>();

        public EventHandler(HomeUserControl homeUserControl)
        {
            HomeUserControl = homeUserControl;
            AudioController = new AudioController();
        }
        public void Update(List<(CustomSlider Slider, int Value)> currentSliderValues,
                           List<(CustomButton Button, int Value)> currentButtonValues)
        {

            for (int i = 0; i < currentButtonValues.Count; i++)
            {
                if(oldButtonValues != null)
                {
                    ButtonEvent(currentButtonValues[i].Button, currentButtonValues[i].Value, oldButtonValues[i].Value);
                }                
            }
            oldButtonValues = currentButtonValues.Select(t => (t.Button, t.Value)).ToList();



            var updatedSliders = new List<(CustomSlider Slider, int Value)>();

            var snapshot = currentSliderValues;

            foreach (var (slider, currentValue) in snapshot)
            {
                int oldValue = oldSliderValues.TryGetValue(slider, out int val) ? val : 0;

                int diff = Math.Abs(oldValue - currentValue);

                if (diff > 4)
                {
                    SliderEvent(slider, currentValue);
                    updatedSliders.Add((slider, currentValue));
                }
            }

            foreach (var (slider, value) in updatedSliders)
            {
                oldSliderValues[slider] = value;
            }
        }

        private void SliderEvent(CustomSlider slider, int value)
        {
            HomeUserControl.Dispatcher.Invoke(() => HomeUserControl.UpdateUISlider(slider, value));

            if (slider.Category != null)
                foreach(string processName in slider.Category.Processes)
                    Task.Run(() => AudioController.SetProcessVolume(processName, SliderToFloat(value)));          
        }

        private void ButtonEvent(CustomButton button, int currentValue, int oldValue)
        {
            bool IsPressed = currentValue == 1;
            bool IsPressedOld = oldValue == 1;

            if (currentValue != oldValue)
            {
                HomeUserControl.Dispatcher.Invoke(() => HomeUserControl.UpdateUIButton(button, IsPressed));

                if (button.Category != null)
                {
                    foreach (ButtonAction buttonAction in button.Category.ButtonActions)
                    {
                        if (buttonAction.ActionProperty == null && buttonAction.ActionType.Type != EActionType.MuteMainAudio)
                            continue;

                        switch (buttonAction.ActionType.Type)
                        {
                            case EActionType.MuteProcess:
                                {
                                    if (IsPressed && !IsPressedOld)
                                    {                                        
                                        AudioController.MuteProcess(buttonAction.ActionProperty, !AudioController.IsProcessMute(buttonAction.ActionProperty));
                                    }
                                    break;
                                }
                            case EActionType.MuteMainAudio:
                                {
                                    if (IsPressed && !IsPressedOld)
                                    {
                                        AudioController.MuteSystem(!AudioController.IsSystemMute());
                                    }
                                    break;
                                }
                            case EActionType.MuteMic:
                                {
                                    if (IsPressed && !IsPressedOld)
                                    {
                                        AudioController.MuteMic(buttonAction.ActionProperty, !AudioController.IsMicMute(buttonAction.ActionProperty));
                                    }
                                    break;
                                }
                            case EActionType.OpenProcess:
                                {
                                    if (IsPressed && !IsPressedOld)
                                    {
                                        try
                                        {
                                            Process.Start(buttonAction.ActionProperty);
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine(ex);
                                        }
                                    }
                                    break;
                                }
                            case EActionType.OpenWebsite:
                                {
                                    if (IsPressed && !IsPressedOld)
                                    {
                                        try
                                        {
                                            Process.Start(new ProcessStartInfo
                                            {
                                                FileName = buttonAction.ActionProperty,
                                                UseShellExecute = true
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine(ex);
                                        }
                                    }
                                    break;
                                }
                            case EActionType.KeyPress:
                                {
                                    uint.TryParse(buttonAction.ActionProperty, out uint keyCode);
                                    if (IsPressed && !IsPressedOld)
                                    {                                       
                                        KeyController.HoldStart((ushort)keyCode);
                                    }
                                    if(!IsPressed && IsPressedOld)
                                    {
                                        KeyController.HoldStop((ushort)keyCode);
                                    }
                                    break;
                                }
                        }
                    }
                }                  
            }
        }

        private float SliderToFloat(int value)
        {
            value -= 1;
            return Math.Min((float)value / 1022.0f, 1);
        }
    }
}

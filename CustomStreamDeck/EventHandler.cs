using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CustomStreamDeck
{
    public class EventHandler
    {
        private AudioController auCo;
        private ArduinoController ardCo;
        private Dictionary<Control, int> values = new Dictionary<Control, int>();       

        private MainWindow mw;

        public EventHandler(MainWindow mainWindow)
        {
            mw = mainWindow;
        }
        public void Update(Dictionary<Control, int> values)
        {

            if(this.values.Count == values.Count && !this.values.Except(values).Any()) return;

            mw.Dispatcher.Invoke(() => mw.UpdateUISlider(mw.Slider1, values[mw.Slider1]));
            mw.Dispatcher.Invoke(() => mw.UpdateUISlider(mw.Slider2, values[mw.Slider2]));

            /*foreach (var kvp in values)
            {
                Slider slider = (Slider)kvp.Key;
                if (true)
                {
                    
                }
            }*/

            this.values = values;

        }
    }
}

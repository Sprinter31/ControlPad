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
        private Dictionary<Control, int> valuesOld = new Dictionary<Control, int>();        

        private MainWindow mw;

        public EventHandler(MainWindow mainWindow) 
        { 
            mw = mainWindow;
        }

        public void TakeValues(Dictionary<Control, int> values)
        {
            valuesOld = this.values;
            this.values = values;

            CheckChanges();
        }

        private void CheckChanges()
        {
            /*if (values == valuesOld)
                return;*/

            foreach (var kvp in values)
            {
                Slider slider = (Slider)kvp.Key;
                if (!valuesOld.ContainsKey(slider) || values[slider] != valuesOld[slider])
                {
                    mw.Dispatcher.Invoke(() =>  mw.UpdateUISlider(slider, kvp.Value));
                }
            }
        }
    }
}

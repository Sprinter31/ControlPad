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
        private Dictionary<Slider, int> values = new Dictionary<Slider, int>();
        private Dictionary<Slider, int> valuesOld = new Dictionary<Slider, int>();        

        private MainWindow mw;

        public EventHandler(MainWindow mainWindow) 
        { 
            mw = mainWindow;
        }

        public void TakeValues(Dictionary<Slider, int> values)
        {
            valuesOld = this.values;
            this.values = values;

            CheckChanges();
        }

        private void CheckChanges()
        {
            if (values == valuesOld)
                return;

            foreach (var kvp in values)
            {
                Slider slider = kvp.Key;
                int value = kvp.Value;
                if (values[slider] != valuesOld[slider])
                {
                    mw.UpdateUISlider(slider, values[slider]);
                }
            }
        }
    }
}

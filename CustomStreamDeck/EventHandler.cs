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
        private Dictionary<Object, int> values = new Dictionary<Object, int>();
        private Dictionary<Object, int> valuesOld = new Dictionary<Object, int>();        

        private MainWindow mw;

        public EventHandler(MainWindow mainWindow) 
        { 
            mw = mainWindow;
        }

        public void TakeValues(Dictionary<Object, int> values)
        {
            valuesOld = this.values;
            this.values = values;

            CheckChanges();
        }

        private void CheckChanges()
        {
            if (values == valuesOld)
                return;

            foreach (Object obj in values)
            {
                if(obj is Slider slider && values[slider] != valuesOld[slider])
                {
                    mw.UpdateUISlider(slider, values[slider]);
                }
            }
        }
    }
}

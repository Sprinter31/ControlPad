using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPad
{
    public class ButtonAction
    {
        public ActionType ActionType;
        public object? ActionProperty;

        public ButtonAction(ActionType actionType)
        {
            ActionType = actionType;
        }
    }
}

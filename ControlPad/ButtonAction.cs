using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPad
{
    public class ButtonAction
    {
        public ActionType ActionType { get; set; }
        public object? ActionProperty { get; set; }
        public string? ActionPropertyDisplay { get; set; }

        public ButtonAction(ActionType actionType)
        {
            ActionType = actionType;
        }
    }
}

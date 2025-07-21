using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPad
{
    public class ActionType
    {        
        public EActionType Type { get; private set; }
        public string Description { get; private set; }

        public ActionType(EActionType type, string description)
        {
            Type = type;
            Description = description;
        }
    }

    public enum EActionType
    {
        MuteProcess,
        MuteMainAudio,
        MuteMic,
        OpenProcess,
        OpenWebsite,
        KeyPress,
    }
}
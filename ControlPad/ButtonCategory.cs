using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ControlPad
{
    public class ButtonCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ButtonAction> ButtonActions { get; set; } = new List<ButtonAction>();

        public ButtonCategory(string name, int id)
        {
            Name = name;
            Id = id;
        }      

        public override string ToString() => Name;
    }
}

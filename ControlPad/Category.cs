using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ControlPad
{
    public class Category
    {
        public string Name { get; set; }
        public ObservableCollection<string> Programms { get; set; } = new ObservableCollection<string>();
        public Category(string name) 
        { 
            Name = name;
        }

        public override string ToString() => Name;
    }
}

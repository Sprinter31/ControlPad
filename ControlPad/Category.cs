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
    public class Category
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public ObservableCollection<string> Processes { get; set; } = new ObservableCollection<string>();

        public Category(string name, int id)
        {
            Name = name;
            Id = id;
        }      

        public override string ToString() => Name;
    }
}

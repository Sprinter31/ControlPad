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
    public class SliderCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ObservableCollection<AudioStream> AudioStreams { get; set; } = new ObservableCollection<AudioStream>();

        public SliderCategory(string name, int id)
        {
            Name = name;
            Id = id;
        }      

        public override string ToString() => Name;
    }
}

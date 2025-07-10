using System.Collections.ObjectModel;

namespace ControlPad
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> Programms { get; set; } = new ObservableCollection<string>();
        public Category(string name) 
        { 
            Name = name;
        }

        public override string ToString() => Name;
    }
}

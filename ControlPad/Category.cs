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
        public int Id { get; private set; }
        public ObservableCollection<string> Programms { get; set; } = new ObservableCollection<string>();

        public Category(string name)
        { 
            Name = name;
            Id = GetNextId();
        }

        private int GetNextId() // gets the lowest not existing id
        {
            var used = new HashSet<int>(DataHandler.Categories.Select(c => c.Id));
            var usedTemp = new HashSet<int>(DataHandler.CategoriesTemp.Select(c => c.Id));

            for(int i = 0; ; i++)
                if(!used.Contains(i) && !usedTemp.Contains(i))
                    return i;
        }

        public override string ToString() => Name;
    }
}

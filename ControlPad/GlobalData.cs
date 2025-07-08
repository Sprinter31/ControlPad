using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControlPad
{
    public static class GlobalData
    {
        public static ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();

        public static void SaveCategories(string path)
        {
            string json = JsonSerializer.Serialize(Categories, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static void LoadCategories(string path)
        {
            if(!File.Exists(path)) { Debug.WriteLine($"File does not exist: {path}"); return; }

            string json = File.ReadAllText(path);
            var list = JsonSerializer.Deserialize<List<Category>>(json) ?? new List<Category>();
            Categories = new ObservableCollection<Category>(list);
        }
    }
}

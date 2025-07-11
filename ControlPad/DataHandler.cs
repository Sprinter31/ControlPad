using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ControlPad
{
    public static class DataHandler
    {
        public static string CategoryPath { get; } = @"Resources\Categories.json";
        public static string CategorySlidersPath { get; } = @"Resources\CategorySliders.json";
        public static ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public static ObservableCollection<Category> CategoriesTemp { get; set; } = new ObservableCollection<Category>();
        public static List<CategorySlider> CategorySliders { get; set; } = new List<CategorySlider>();

        public static void SaveDataToFile<T>(string path, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static List<T> LoadDataFromFile<T>(string path)
        {
            var list = new List<T>();
            if (!File.Exists(path))
            {
                Debug.WriteLine($"File does not exist: {path}");
            }
            else
            {
                string json = File.ReadAllText(path);
                list = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            
            return list;
        }
    }
}

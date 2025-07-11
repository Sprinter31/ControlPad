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
        public static string CategorySlidersPath { get; } = @"Resources\CategorySliders.txt";
        public static ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public static ObservableCollection<Category> CategoriesTemp { get; set; } = new ObservableCollection<Category>();
        public static CategorySlider[] CategorySliders { get; set; } = new CategorySlider[6];

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

        public static void SaveCategorySliders(string path)
        {
            var lines = CategorySliders.Select((slider, i) => $"Slider{i + 1}.Category.Id is {slider.Category?.Id}");
            File.WriteAllLines(path, lines);
        }

        public static void LoadCategorySliders(string path)
        {
            string[] lines = File.ReadAllLines(path);

            for(int i = 0; i < lines.Length; i++)
            {
                if(int.TryParse(lines[i][^1].ToString(), out int categoryId))
                    CategorySliders[i].Category = Categories.First(c => c.Id == categoryId);
            }
        }
    }
}

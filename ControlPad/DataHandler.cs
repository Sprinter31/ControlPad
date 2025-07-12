using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Documents;

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
            var lines = CategorySliders.Select((slider, i) => $"Slider{i + 1}.Category.Id is: {slider.Category?.Id}");
            File.WriteAllLines(path, lines);
        }

        public static void LoadCategorySliders(string path)
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine($"File does not exist: {path}");
            }
            else
            {
                string[] lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (int.TryParse(lines[i].Split(':')[1].Trim(), out int categoryId))
                        CategorySliders[i].Category = Categories.First(c => c.Id == categoryId);
                }
            }            
        }

        public static void RemoveCategoriesFromSlidersIfTheyGotDeleted()
        {
            foreach (CategorySlider categorySlider in DataHandler.CategorySliders)
                if (categorySlider.Category != null && !DataHandler.Categories.Any(c => c.Id == categorySlider.Category.Id))
                    categorySlider.Category = null;
            DataHandler.SaveCategorySliders(DataHandler.CategorySlidersPath);
        }

        public static int GetNextCategoryId() // gets the lowest not yet existing id
        {
            var used = new HashSet<int>(DataHandler.Categories.Select(c => c.Id));
            var usedTemp = new HashSet<int>(DataHandler.CategoriesTemp.Select(c => c.Id));

            for (int i = 0; ; i++)
                if (!used.Contains(i) && !usedTemp.Contains(i))
                    return i;
        }
    }
}

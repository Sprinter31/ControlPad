using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ControlPad
{
    public static class DataHandler
    {
        public static string CategoriesPath { get; } = @"Resources\Categories.json";
        public static string SliderAssignmentPath { get; } = @"Resources\SliderAssignments.json";
        public static ObservableCollection<SliderAssignments> SliderAssignments { get; set; } = new ObservableCollection<SliderAssignments>();
        public static ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        public static void SaveDataToFile<T>(string path, ObservableCollection<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static ObservableCollection<T> LoadDataFromFile<T>(string path, ObservableCollection<T> data)
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


            ObservableCollection<T> DataReturn;
            DataReturn = new ObservableCollection<T>(list);
            return DataReturn;
        }
        public static int GetNextCategoryId()
        {
            if (Categories == null || Categories.Count == 0)
                return 1;

            return Categories.Max(c => c.Id) + 1;
        }

    }
}

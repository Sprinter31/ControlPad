using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace ControlPad
{
    public static class DataHandler
    {
        public static string SliderCategoriesPath { get; } = @"Resources\SliderCategories.json";
        public static string ButtonCategoriesPath { get; } = @"Resources\ButtonCategories.json";
        public static string CategoryControlsPath { get; } = @"Resources\CategoryControls.txt";
        public static string SettingsPath { get; } = @"Resources\Settings.json";
        public static ObservableCollection<SliderCategory> SliderCategories { get; set; } = new ObservableCollection<SliderCategory>();
        public static ObservableCollection<ButtonCategory> ButtonCategories { get; set; } = new ObservableCollection<ButtonCategory>();
        public static List<(CustomSlider slider, int value)> SliderValues { get; set; } = new();
        public static List<(CustomButton button, int value)> ButtonValues { get; set; } = new();
        

        public static void SaveDataToFile<T>(string path, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals });
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

        public static void SaveCategoryControls(string path)
        {
            var sliderLines = SliderValues.Select((item, i) => $"Slider{i + 1}.Category.Id is: {item.slider.Category?.Id}");
            var buttonLines = ButtonValues.Select((item, i) => $"Switch{i + 1}.Category.Id is: {item.button.Category?.Id}");
            File.WriteAllLines(path, sliderLines.Concat(buttonLines));
        }

        public static void LoadCategoryControls(string path)
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine($"File does not exist: {path}");
            }
            else
            {
                string[] lines = File.ReadAllLines(path);

                for (int i = 0; i < SliderValues.Count; i++)
                {
                    if (int.TryParse(lines[i].Split(':')[1].Trim(), out int sliderCategoryId))
                    {
                        var category = SliderCategories.FirstOrDefault(c => c.Id == sliderCategoryId);
                        SliderValues[i].slider.Category = category;
                    }
                }
                for (int i = 0; i < ButtonValues.Count; i++)
                {
                    if (int.TryParse(lines[i + SliderValues.Count].Split(':')[1].Trim(), out int buttonCategoryId))
                    {
                        var category = ButtonValues[i].button.Category = ButtonCategories.FirstOrDefault(c => c.Id == buttonCategoryId);
                        ButtonValues[i].button.Category = category;
                    }    
                }
            }
        }

        public static int GetNextCategoryId<T>(this IEnumerable<T> items, Func<T, int> idSelector) // gets the lowest not yet existing id
        {
            var used = new HashSet<int>(items.Select(idSelector));
            int candidate = 0;
            while (used.Contains(candidate))
                candidate++;
            return candidate;
        }

        public static void SetSliderTextBlocks()
        {
            foreach ((CustomSlider slider, int) categorySlider in DataHandler.SliderValues)
                if (categorySlider.slider.Category != null)
                    categorySlider.slider.TextBlock.Text = categorySlider.slider.Category.Name;
                else
                    categorySlider.slider.TextBlock.Text = "";
        }

        public static void SetButtonTextBlocks()
        {
            foreach ((CustomButton button, int) categoryButton in DataHandler.ButtonValues)
                if (categoryButton.button.Category != null)
                    categoryButton.button.TextBlock.Text = categoryButton.button.Category?.Name ?? "";
                else
                    categoryButton.button.TextBlock.Text = "";
        }

        public static readonly List<ActionType> ActionTypes = new()
        {
            new ActionType(EActionType.MuteProcess,   "Mute Process"),
            new ActionType(EActionType.MuteMainAudio, "Mute Main Audio Stream"),
            new ActionType(EActionType.MuteMic,       "Mute Microphone"),
            new ActionType(EActionType.OpenProcess,   "Open Process"),
            new ActionType(EActionType.OpenWebsite,   "Open Website"),
            new ActionType(EActionType.KeyPress,      "Key Press"),
        };
    }
}

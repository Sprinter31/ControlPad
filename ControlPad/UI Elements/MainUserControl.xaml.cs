using ControlPad.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlPad
{
    public partial class MainUserControl : UserControl
    {
        private ArduinoController arduinoController;
        public MainUserControl()
        {
            InitializeComponent();
            DataHandler.CategorySliders = new CustomSlider[] { Slider1, Slider2, Slider3, Slider4, Slider5, Slider6 };
            DataHandler.Categories = new ObservableCollection<Category>(DataHandler.LoadDataFromFile<Category>(DataHandler.CategoryPath));
            DataHandler.LoadCategorySliders(DataHandler.CategorySlidersPath);
            DataHandler.SetSliderTextBlocks();

            arduinoController = new ArduinoController(this);
        }

        

        public void UpdateUISlider(Slider slider, int value) => slider.Value = value;

        private void SliderCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is SliderBorder border)
            {
                var dialog = new SelectCategoryPopup(border.CustomSlider);
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    border.CustomSlider.Category = dialog.SelectedCategory;
                    DataHandler.SaveCategorySliders(DataHandler.CategorySlidersPath);
                    DataHandler.SetSliderTextBlocks();
                }
            }
        }

        private void Switch7_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectActionPopup();
            dialog.ShowDialog();
        }

        private void Switch8_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectKeyPopup();
            dialog.ShowDialog();
        }
    }
}

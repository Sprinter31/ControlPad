using ControlPad;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class HomeUserControl : UserControl
    {
        MainWindow mainWindow;
        public HomeUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            CreateControlValues();

            DataHandler.SliderCategories = new ObservableCollection<SliderCategory>(DataHandler.LoadDataFromFile<SliderCategory>(DataHandler.SliderCategoriesPath));
            DataHandler.ButtonCategories = new ObservableCollection<ButtonCategory>(DataHandler.LoadDataFromFile<ButtonCategory>(DataHandler.ButtonCategoriesPath));
            DataHandler.LoadCategoryControls(DataHandler.CategoryControlsPath);
            DataHandler.SetSliderTextBlocks();

            this.mainWindow = mainWindow;
        }
        public void UpdateUISlider(Slider slider, int value) => slider.Value = value;

        private void cb_EditMode_Checked(object sender, RoutedEventArgs e)
        {
            SliderCell1.Visibility = Visibility.Visible;
            SliderCell2.Visibility = Visibility.Visible;
            SliderCell3.Visibility = Visibility.Visible;
            SliderCell4.Visibility = Visibility.Visible;
            SliderCell5.Visibility = Visibility.Visible;
            SliderCell6.Visibility = Visibility.Visible;
        }

        private void cb_EditMode_Unchecked(object sender, RoutedEventArgs e)
        {
            SliderCell1.Visibility = Visibility.Hidden;
            SliderCell2.Visibility = Visibility.Hidden;
            SliderCell3.Visibility = Visibility.Hidden;
            SliderCell4.Visibility = Visibility.Hidden;
            SliderCell5.Visibility = Visibility.Hidden;
            SliderCell6.Visibility = Visibility.Hidden;
        }

        private void SliderCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is SliderBorder border)
            {
                var dialog = new SelectSliderCategoryPopup(border.CustomSlider);
                dialog.Owner = mainWindow;

                if (dialog.ShowDialog() == true)
                {
                    border.CustomSlider.Category = dialog.SelectedCategory;
                    DataHandler.SaveCategoryControls(DataHandler.CategoryControlsPath);
                    DataHandler.SetSliderTextBlocks();
                }
            }
        }

        private void CreateControlValues()
        {
            DataHandler.SliderValues = new()
            {
                (Slider1, 0),
                (Slider2, 0),
                (Slider3, 0),
                (Slider4, 0),
                (Slider5, 0),
                (Slider6, 0)
            };
            DataHandler.ButtonValues = new()
            {
                (Switch1, 0),
                (Switch2, 0),
                (Switch3, 0),
                (Switch4, 0),
                (Switch5, 0),
                (Switch6, 0),
                (Switch7, 0),
                (Switch8, 0),
                (Switch9, 0),
                (Switch10, 0),
                (Switch11, 0)
            };
        }
    }
}

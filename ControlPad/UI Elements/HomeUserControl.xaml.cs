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
    /// <summary>
    /// Interaction logic for HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl
    {
        public HomeUserControl()
        {
            InitializeComponent();

            DataHandler.CategorySliders = new CustomSlider[] { (CustomSlider)Slider1, Slider2, Slider3, Slider4, Slider5, Slider6 };

            DataHandler.Categories = new ObservableCollection<Category>(DataHandler.LoadDataFromFile<Category>(DataHandler.CategoryPath));
            DataHandler.SetSliderTextBlocks();
            DataHandler.LoadCategorySliders(DataHandler.CategorySlidersPath);
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
            /*var dialog = new SelectKeyPopup();
            dialog.Owner = this;
            dialog.ShowDialog();*/
            var dialog = new SelectActionPopup();
            dialog.ShowDialog();
        }
    }
}

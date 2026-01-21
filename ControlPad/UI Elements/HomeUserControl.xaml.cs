using ControlPad;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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

            this.mainWindow = mainWindow;
        }
        public void UpdateUISlider(Slider slider, int value) => slider.Value = value;
        public void UpdateUIButton(CustomButton button, bool isChecked) => button.IsChecked = isChecked;
        public void ChangeText(CustomButton button) => button.TextBlock.Text = button.Category?.Name;

        private void SliderCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is SliderBorder border)
            {
                var dialog = new SelectSliderCategoryPopup(border.CustomSlider);
                dialog.Owner = mainWindow;

                if (dialog.ShowDialog() == true)
                {
                    border.CustomSlider.Category = dialog.SelectedCategory;
                    DataHandler.SaveCategoryControls(DataHandler.GetCategoryControlsPath());
                    DataHandler.SetSliderTextBlocks();
                }
            }
        }

        private void ButtonCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is ButtonBorder border)
            {
                var dialog = new SelectButtonCategoryPopup(border.CustomButton);
                dialog.Owner = mainWindow;

                if (dialog.ShowDialog() == true)
                {
                    border.CustomButton.Category = dialog.SelectedCategory;
                    DataHandler.SaveCategoryControls(DataHandler.GetCategoryControlsPath());
                    DataHandler.SetButtonTextBlocks();
                }
            }
        }

        private void ToggleButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => e.Handled = true;

        public void SetTextBlocks()
        {
            Switch1.TextBlock = Switch1_TextBlock;
            Switch2.TextBlock = Switch2_TextBlock;
            Switch3.TextBlock = Switch3_TextBlock;
            Switch4.TextBlock = Switch4_TextBlock;
            Switch5.TextBlock = Switch5_TextBlock;
            Switch6.TextBlock = Switch6_TextBlock;
            Switch7.TextBlock = Switch7_TextBlock;
            Switch8.TextBlock = Switch8_TextBlock;
            Switch9.TextBlock = Switch9_TextBlock;
            Switch10.TextBlock = Switch10_TextBlock;
            Switch11.TextBlock = Switch11_TextBlock;
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

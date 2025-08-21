using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class SelectSliderCategoryPopup : FluentWindow
    {
        public SliderCategory SelectedCategory { get; set; }
        private CustomSlider categorySlider;
        public SelectSliderCategoryPopup(CustomSlider categorySlider)
        {
            InitializeComponent();
            this.categorySlider = categorySlider;
            cb_Categories.ItemsSource = DataHandler.SliderCategories;
            if(categorySlider?.Category?.Id != null)
                cb_Categories.SelectedItem = DataHandler.SliderCategories.First(c => c.Id == categorySlider?.Category?.Id);
        }       

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Categories.SelectedItem is SliderCategory selectedCategory)
            {
                SelectedCategory = selectedCategory;

                foreach (var sliderValue in DataHandler.SliderValues)
                {
                    var slider = sliderValue.slider;
                    if (slider.Name == categorySlider.Name) continue;

                    if (slider.Category?.Id == selectedCategory.Id)
                    {
                        slider.Category = null;
                    }
                }
                DataHandler.SetSliderTextBlocks();

                DialogResult = true;
            }
            else if(cb_Categories.SelectedItem == null)
            {
                categorySlider.Category = null;
                DialogResult = true;
            }  
        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e) => cb_Categories.SelectedItem = null;

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

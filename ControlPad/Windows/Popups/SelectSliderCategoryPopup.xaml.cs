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
            SetDropDown();            
        }       

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if(cb_Categories.SelectedItem is SliderCategory selectedCategory)
            {
                SelectedCategory = selectedCategory;
                DialogResult = true;
            }
            else if(cb_Categories.SelectedItem == null)
            {
                categorySlider.Category = null;
                DialogResult = true;
            }            
        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e) => cb_Categories.SelectedItem = null;

        private void SetDropDown()
        {
            var usedIds = DataHandler.CategorySliders.Select(s => s.Category?.Id).ToHashSet();

            var currentCatId = categorySlider?.Category?.Id;

            var availableCategories = DataHandler.SliderCategories.Where(c => !usedIds.Contains(c.Id) || c.Id == currentCatId).ToList();

            cb_Categories.ItemsSource = availableCategories;

            if (currentCatId != null)
                cb_Categories.SelectedItem = availableCategories.First(c => c.Id == currentCatId);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

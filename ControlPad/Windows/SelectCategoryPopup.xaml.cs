using System.Windows;

namespace ControlPad.Windows
{
    public partial class SelectCategoryPopup : Window
    {
        public Category SelectedCategory { get; set; }
        public SelectCategoryPopup(CategorySlider categorySlider)
        {
            InitializeComponent();

            var usedIds = DataHandler.CategorySliders.Select(s => s.Category?.Id).ToHashSet();

            var currentCatId = categorySlider?.Category?.Id;

            var availableCategories = DataHandler.Categories.Where(c => !usedIds.Contains(c.Id) || c.Id == currentCatId).ToList();

            cb_Categories.ItemsSource = availableCategories;

            if (currentCatId != null)
                cb_Categories.SelectedItem = availableCategories.First(c => c.Id == currentCatId);
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if(cb_Categories.SelectedItem is Category selectedCategory)
            {
                SelectedCategory = selectedCategory;
                DialogResult = true;
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
    }
}

using System.Windows;

namespace ControlPad.Windows
{
    public partial class SelectCategoryPopup : Window
    {
        public Category SelectedCategory { get; set; }
        public CategorySlider CategorySlider { get; set; }
        public SelectCategoryPopup()
        {
            InitializeComponent();
            if(CategorySlider?.Category != null)
                cb_Categories.SelectedItem = CategorySlider.Category;
            cb_Categories.ItemsSource = DataHandler.Categories;
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

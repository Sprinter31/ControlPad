using System.Windows;

namespace ControlPad.Windows
{
    public partial class SelectCategoryPopup : Window
    {
        public CategorySlider sliderToAssignCategoryTo;
        public SelectCategoryPopup()
        {
            InitializeComponent();
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if(cb_Categories.SelectedItem is Category selectedCategory)
            {
                sliderToAssignCategoryTo.Category = selectedCategory;
                DialogResult = true;
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
    }
}

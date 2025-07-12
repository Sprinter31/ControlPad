using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;

namespace ControlPad.Windows
{
    public partial class SelectCategoryPopup : FluentWindow
    {
        public Category SelectedCategory { get; set; }
        private CustomSlider categorySlider;
        public SelectCategoryPopup(CustomSlider categorySlider)
        {
            InitializeComponent();
            this.categorySlider = categorySlider;
            ButtonImage.Source = new BitmapImage(new Uri(@"\Resources\x.png", UriKind.Relative));
            SetDropDown();            
        }       

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if(cb_Categories.SelectedItem is Category selectedCategory)
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

            var availableCategories = DataHandler.Categories.Where(c => !usedIds.Contains(c.Id) || c.Id == currentCatId).ToList();

            cb_Categories.ItemsSource = availableCategories;

            if (currentCatId != null)
                cb_Categories.SelectedItem = availableCategories.First(c => c.Id == currentCatId);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

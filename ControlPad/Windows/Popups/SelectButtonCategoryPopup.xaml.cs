using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class SelectButtonCategoryPopup : FluentWindow
    {
        public ButtonCategory? SelectedCategory { get; set; }
        private CustomButton categoryButton;
        public SelectButtonCategoryPopup(CustomButton categoryButton)
        {
            InitializeComponent();
            this.categoryButton = categoryButton;
            SetDropDown();            
        }       

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if(cb_Categories.SelectedItem is ButtonCategory selectedCategory)
            {
                SelectedCategory = selectedCategory;
                DialogResult = true;
            }
            else if(cb_Categories.SelectedItem == null)
            {
                categoryButton.Category = null;
                DialogResult = true;
            }            
        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e) => cb_Categories.SelectedItem = null;

        private void SetDropDown()
        {
            var usedIds = DataHandler.ButtonValues.Select(s => s.button.Category?.Id).ToHashSet();

            var currentCatId = categoryButton?.Category?.Id;

            var availableCategories = DataHandler.ButtonCategories.Where(c => !usedIds.Contains(c.Id) || c.Id == currentCatId).ToList();

            cb_Categories.ItemsSource = availableCategories;

            if (currentCatId != null)
                cb_Categories.SelectedItem = availableCategories.First(c => c.Id == currentCatId);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

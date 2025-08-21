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
            cb_Categories.ItemsSource = DataHandler.ButtonCategories;
            if (categoryButton?.Category?.Id != null)
                cb_Categories.SelectedItem = DataHandler.ButtonCategories.First(c => c.Id == categoryButton?.Category?.Id);
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if(cb_Categories.SelectedItem is ButtonCategory selectedCategory)
            {
                SelectedCategory = selectedCategory;

                foreach (var buttonValue in DataHandler.ButtonValues)
                {
                    var button = buttonValue.button;
                    if (button.Name == categoryButton.Name) continue;

                    if (button.Category?.Id == selectedCategory.Id)
                    {
                        button.Category = null;
                    }
                }
                DataHandler.SetSliderTextBlocks();

                DialogResult = true;
            }
            else if(cb_Categories.SelectedItem == null)
            {
                categoryButton.Category = null;
                DialogResult = true;
            }            
        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e) => cb_Categories.SelectedItem = null;

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

using System.Windows;

namespace CustomStreamDeck
{
    /// <summary>
    /// Interaction logic for CategoryDialog.xaml
    /// </summary>
    public partial class CategoryDialog : Window
    {
        public CategoryDialog()
        {
            InitializeComponent();
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.Categories.Add(new List<string>());
        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            if (lb_Categories.SelectedIndex == -1)
                MessageBox.Show("Nothing selected", "Custom Stream Deck", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e)
        {
            if (lb_Categories.SelectedIndex == -1)          
                MessageBox.Show("Nothing selected", "Custom Stream Deck", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

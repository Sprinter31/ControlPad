using ControlPad.Windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;


namespace ControlPad
{
    public partial class ManageCategoriesWindow : FluentWindow
    {
        public ManageCategoriesWindow()
        {
            InitializeComponent();
            DataHandler.CategoriesTemp = new ObservableCollection<Category>(
                DataHandler.Categories
                .Select(c => new Category(c.Name, c.Id)
                {
                    Processes = new ObservableCollection<string>(c.Processes)
                }));
            lb_Categories.ItemsSource = DataHandler.CategoriesTemp;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateCategoryPopup();
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();
            string name = "";

            if (result == true)
            {
                name = dialog.CategoryName;
            }           

            if (string.IsNullOrEmpty(name)) return;

            DataHandler.CategoriesTemp.Add(new Category(name, DataHandler.GetNextCategoryId()));
        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            int index = lb_Categories.SelectedIndex;

            if (index == -1) return;

            var dialog = new EditCategoryWindow(this, index);
            dialog.Owner = this;
            dialog.ShowDialog();
        }       

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.Categories = DataHandler.CategoriesTemp;
            DataHandler.CategoriesTemp = new ObservableCollection<Category>();
            DataHandler.SaveDataToFile(DataHandler.CategoryPath, DataHandler.Categories.ToList());
            DataHandler.RemoveCategoriesFromSlidersIfTheyGotDeleted();
            this.Close();
        }

        private void lb_Categories_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Delete) DeleteAtSelected();
        }

        private void DeleteAtSelected()
        {
            int index = lb_Categories.SelectedIndex;
            if (index == -1) return;

            DataHandler.CategoriesTemp.RemoveAt(index);
        }

        public void RefreshListBox() => lb_Categories.Items.Refresh();
        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e) => DeleteAtSelected();
        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

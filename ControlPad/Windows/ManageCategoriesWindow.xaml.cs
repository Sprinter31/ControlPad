using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ControlPad.Windows;


namespace ControlPad
{
    public partial class ManageCategoriesWindow : Window
    {
        public ObservableCollection<Category> categoriesTemp = new ObservableCollection<Category>();
        public ManageCategoriesWindow()
        {
            InitializeComponent();

            categoriesTemp = new ObservableCollection<Category>(
                DataHandler.Categories
                .Select(c => new Category(c.Name)
                {
                    Id = c.Id,
                    Programms = new ObservableCollection<string>(c.Programms)
                }));
            lb_Categories.ItemsSource = categoriesTemp;
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

            int newId = DataHandler.GetNextCategoryId();

            var newCategory = new Category(name)
            {
                Id = newId
            };

            categoriesTemp.Add(newCategory);        
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
            DataHandler.Categories = categoriesTemp;
            DataHandler.SaveDataToFile(DataHandler.CategoriesPath, DataHandler.Categories);
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

            categoriesTemp.RemoveAt(index);
        }

        public void RefreshListBox() => lb_Categories.Items.Refresh();
        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e) => DeleteAtSelected();
        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

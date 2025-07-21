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
            DataHandler.SliderCategoriesTemp = new ObservableCollection<SliderCategory>(
                DataHandler.SliderCategories
                .Select(c => new SliderCategory(c.Name, c.Id)
                {
                    Processes = new ObservableCollection<string>(c.Processes)
                }));
            lb_Categories.ItemsSource = DataHandler.SliderCategoriesTemp;
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

            DataHandler.SliderCategoriesTemp.Add(new SliderCategory(name, DataHandler.GetNextCategoryId()));
        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            int index = lb_Categories.SelectedIndex;

            if (index == -1) return;

            var dialog = new EditCategoryWindow(index);
            dialog.Owner = this;
            dialog.ShowDialog();
        }       

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.SliderCategories = DataHandler.SliderCategoriesTemp;
            DataHandler.SliderCategoriesTemp = new ObservableCollection<SliderCategory>();
            DataHandler.SaveDataToFile(DataHandler.CategoryPath, DataHandler.SliderCategories.ToList());
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

            DataHandler.SliderCategoriesTemp.RemoveAt(index);
        }

        public void RefreshListBox() => lb_Categories.Items.Refresh();
        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e) => DeleteAtSelected();
        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

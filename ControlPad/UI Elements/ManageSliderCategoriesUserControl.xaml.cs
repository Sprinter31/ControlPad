using ControlPad.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlPad
{
    public partial class ManageSliderCategoriesUserControl : UserControl
    {
        MainWindow mainWindow;
        public ManageSliderCategoriesUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            DataHandler.SliderCategoriesTemp = new ObservableCollection<SliderCategory>(
                DataHandler.SliderCategories
                .Select(c => new SliderCategory(c.Name, c.Id)
                {
                    Processes = new ObservableCollection<string>(c.Processes)
                }));
            lb_Categories.ItemsSource = DataHandler.SliderCategories;
            this.mainWindow = mainWindow;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateCategoryPopup();
            dialog.Owner = mainWindow;

            string name = "";

            if (dialog.ShowDialog() == true)
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
            dialog.ShowDialog();
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.SliderCategories = DataHandler.SliderCategoriesTemp;
            DataHandler.SliderCategoriesTemp = new ObservableCollection<SliderCategory>();
            DataHandler.SaveDataToFile(DataHandler.CategoryPath, DataHandler.SliderCategories.ToList());
            DataHandler.RemoveCategoriesFromSlidersIfTheyGotDeleted();
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
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

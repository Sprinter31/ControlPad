using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
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
    public partial class ManageButtonCategoriesUserControl : UserControl
    {
        MainWindow mainWindow;
        public ManageButtonCategoriesUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            lb_Categories.ItemsSource = DataHandler.ButtonCategories;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            var dialog = new CreateSliderCategoryPopup() { Owner = mainWindow };

            if (dialog.ShowDialog() == true)
            {
                name = dialog.CategoryName;
            }

            if (!string.IsNullOrEmpty(name))
            {
                DataHandler.ButtonCategories.Add(new ButtonCategory(name, DataHandler.ButtonCategories.GetNextCategoryId(c => c.Id)));
                DataHandler.SaveDataToFile(DataHandler.ButtonCategoriesPath, DataHandler.ButtonCategories.ToList());
            }
        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditButtonCategoryWindow(lb_Categories.SelectedIndex);
            dialog.Owner = mainWindow;
            dialog.ShowDialog();
            DataHandler.SaveDataToFile(DataHandler.ButtonCategoriesPath, DataHandler.ButtonCategories.ToList());
        }

        private void DeleteAtSelected()
        {
            int index = lb_Categories.SelectedIndex;
            if (index == -1) return;            

            DataHandler.ButtonCategories.RemoveAt(index);
            DataHandler.SaveDataToFile(DataHandler.ButtonCategoriesPath, DataHandler.ButtonCategories.ToList());
        }

        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e) => DeleteAtSelected();

        private void lb_Categories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) DeleteAtSelected();
        }
    }
}

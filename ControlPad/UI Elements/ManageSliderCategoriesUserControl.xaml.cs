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
            this.mainWindow = mainWindow;
            lb_Categories.ItemsSource = DataHandler.SliderCategories;           
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
                DataHandler.SliderCategories.Add(new SliderCategory(name, DataHandler.SliderCategories.GetNextCategoryId(c => c.Id)));
                DataHandler.SaveDataToFile(DataHandler.SliderCategoriesPath, DataHandler.SliderCategories.ToList());
            }
        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            int index = lb_Categories.SelectedIndex;

            if (index == -1) return;

            var dialog = new EditSliderCategoryWindow(index);
            dialog.ShowDialog();
            DataHandler.SetSliderTextBlocks();
            DataHandler.SaveDataToFile(DataHandler.SliderCategoriesPath, DataHandler.SliderCategories.ToList());
        }

        

        private void DeleteAtSelected()
        {
            int index = lb_Categories.SelectedIndex;
            if (index == -1) return;
           
            var sliderToRemoveCategoryFrom = DataHandler.CategorySliders.FirstOrDefault(c => c.Category?.Id == DataHandler.SliderCategories[index].Id);
            if (sliderToRemoveCategoryFrom != null)
            {
                sliderToRemoveCategoryFrom.Category = null;
                DataHandler.SetSliderTextBlocks();
            }

            DataHandler.SliderCategories.RemoveAt(index);
            DataHandler.SaveDataToFile(DataHandler.SliderCategoriesPath, DataHandler.SliderCategories.ToList());
            DataHandler.SaveCategorySliders(DataHandler.CategorySlidersPath);
        }

        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e) => DeleteAtSelected();
        private void lb_Categories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) DeleteAtSelected();
        }
    }
}

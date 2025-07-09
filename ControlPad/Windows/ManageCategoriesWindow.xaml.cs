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
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
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
                GlobalData.Categories
                .Select(c => new Category(c.Name)
                {
                    Programms = new ObservableCollection<string>(c.Programms)
                }));
            lb_Categories.ItemsSource = categoriesTemp;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProcessSelectPopup();
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string process = dialog.SelectedProcessName;
                MCW.categoriesTemp[indexOfCategory].Programms.Add(process);
            }
            string name = Interaction.InputBox("Enter a name for the new category", "Create category").Trim(); // ugly, can be designed if needed

            if (string.IsNullOrEmpty(name)) return;

            categoriesTemp.Add(new Category(name));        
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
            GlobalData.Categories = categoriesTemp;
            GlobalData.SaveCategories(GlobalData.CategoryPath);
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

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
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ManageCategoriesWindow()
        {
            InitializeComponent();
            categories = GlobalData.Categories;
            lb_Categories.ItemsSource = categories;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            string name = Interaction.InputBox("Enter a name for the new category", "Create category").Trim(); // ugly, can be designed if needed

            if (string.IsNullOrEmpty(name)) return;

            categories.Add(new Category(name));        
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
            GlobalData.Categories = categories;
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

            categories.RemoveAt(index);
        }

        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e) => DeleteAtSelected();
        private void btn_Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}

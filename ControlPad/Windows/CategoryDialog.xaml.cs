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
    /// <summary>
    /// Interaction logic for CategoryDialog.xaml
    /// </summary>
    public partial class CategoryDialog : Window
    {
        private const string JsonPath = @"Resources\Categories.json";
        public CategoryDialog()
        {
            InitializeComponent();
            GlobalData.LoadCategories(JsonPath);
            lb_Categories.ItemsSource = GlobalData.Categories;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {
            string name = Interaction.InputBox("Enter a name for the new category", "Create category").Trim(); // ugly, can be designed if needed

            if (string.IsNullOrEmpty(name)) { /*MessageBox.Show("Please enter a valid name", "Control Pad");*/ return; }
            GlobalData.Categories.Add(new Category(name));
            
        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            if (lb_Categories.SelectedIndex == -1) return;

            var dialog = new EditCategoryWindow();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e)
        {
            DeleteAtSelected();
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.SaveCategories(JsonPath);
            this.Close();
        }

        private void lb_Categories_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Delete)
                DeleteAtSelected();
        }

        private void DeleteAtSelected()
        {
            int index = lb_Categories.SelectedIndex;
            if (index == -1) return;

            GlobalData.Categories.RemoveAt(index);
        }
    }
}

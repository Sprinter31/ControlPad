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
using System.Windows.Shapes;

namespace ControlPad.Windows
{
    public partial class EditCategoryWindow : Window
    {
        private int indexOfCategory;
        ManageCategoriesWindow MCW;

        public EditCategoryWindow(ManageCategoriesWindow MCW, int indexOfCategory)
        {
            InitializeComponent();
            this.indexOfCategory = indexOfCategory;
            this.MCW = MCW;

            tb_CategoryName.Text = GlobalData.Categories[indexOfCategory].Name;
            lb_Processes.ItemsSource = MCW.categoriesTemp[indexOfCategory].Programms;
        }

        private void btn_AddProcess_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProcessSelectPopup();
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string process = dialog.SelectedProcessName;
                MCW.categoriesTemp[indexOfCategory].Programms.Add(process);
            }               
        }

        private void btn_RemoveProcess_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            MCW.categoriesTemp[indexOfCategory].Name = tb_CategoryName.Text.Trim();
            MCW.RefreshListBox();
            this.Close();
        }
    }
}

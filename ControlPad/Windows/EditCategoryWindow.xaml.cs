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
        private ObservableCollection<string> programms;

        public EditCategoryWindow(int indexOfCategory)
        {
            InitializeComponent();
            this.indexOfCategory = indexOfCategory;

            tb_CategoryName.Text = GlobalData.Categories[indexOfCategory].Name;
            programms = GlobalData.Categories[indexOfCategory].Programms;
            lb_Processes.ItemsSource = programms;
        }

        private void btn_AddProcess_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProcessSelectPopup();
            dialog.Owner = this;
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string process = dialog.SelectedProcessName;
                programms.Add(process);
            }               
        }

        private void btn_RemoveProcess_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.Categories[indexOfCategory].Programms = programms;
            GlobalData.Categories[indexOfCategory].Name = tb_CategoryName.Text.Trim();
            this.Close();
        }
    }
}

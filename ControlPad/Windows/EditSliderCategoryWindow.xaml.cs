using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad.Windows
{
    public partial class EditCategoryWindow : FluentWindow
    {
        private int indexOfCategory;

        public EditCategoryWindow(int indexOfCategory)
        {
            InitializeComponent();
            this.indexOfCategory = indexOfCategory;

            tb_CategoryName.Text = DataHandler.SliderCategoriesTemp[indexOfCategory].Name;
            lb_Processes.ItemsSource = DataHandler.SliderCategoriesTemp[indexOfCategory].Processes;
        }

        private void btn_AddProcess_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectProcessPopup() { Owner = this };

            if (dialog.ShowDialog() == true)
            {
                DataHandler.SliderCategoriesTemp[indexOfCategory].Processes.Add(dialog.SelectedProcessName);
            }               
        }

        private void btn_RemoveProcess_Click(object sender, RoutedEventArgs e)
        {
            int index = lb_Processes.SelectedIndex;
            if (index == -1) return;
            DataHandler.SliderCategoriesTemp[indexOfCategory].Processes.RemoveAt(index);
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.SliderCategoriesTemp[indexOfCategory].Name = tb_CategoryName.Text.Trim();
            this.Close();
        }
    }
}

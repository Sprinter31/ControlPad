using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad
{
    public partial class EditSliderCategoryWindow : FluentWindow
    {
        private int indexOfCategory;

        public EditSliderCategoryWindow(int indexOfCategory)
        {
            InitializeComponent();
            this.indexOfCategory = indexOfCategory;

            tb_CategoryName.Text = DataHandler.SliderCategories[indexOfCategory].Name;
            lb_Processes.ItemsSource = DataHandler.SliderCategories[indexOfCategory].Processes;
        }

        private void btn_AddProcess_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectProcessPopup() { Owner = this };

            if (dialog.ShowDialog() == true)
            {
                DataHandler.SliderCategories[indexOfCategory].Processes.Add(dialog.SelectedProcessName);
            }               
        }

        private void btn_RemoveProcess_Click(object sender, RoutedEventArgs e)
        {
            int index = lb_Processes.SelectedIndex;
            if (index == -1) return;
            DataHandler.SliderCategories[indexOfCategory].Processes.RemoveAt(index);
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.SliderCategories[indexOfCategory].Name = tb_CategoryName.Text.Trim();
            this.Close();
        }
    }
}

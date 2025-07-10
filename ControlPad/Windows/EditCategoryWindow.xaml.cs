using System.Windows;

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

            tb_CategoryName.Text = MCW.categoriesTemp[indexOfCategory].Name;
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
            int index = lb_Processes.SelectedIndex;
            if (index == -1) return;
            MCW.categoriesTemp[indexOfCategory].Programms.RemoveAt(index);
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            MCW.categoriesTemp[indexOfCategory].Name = tb_CategoryName.Text.Trim();
            MCW.RefreshListBox();
            this.Close();
        }
    }
}

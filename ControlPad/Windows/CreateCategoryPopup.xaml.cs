using System.Windows;
using Wpf.Ui.Controls;

namespace ControlPad.Windows
{
    public partial class CreateCategoryPopup : FluentWindow
    {
        public string CategoryName { get; set; } = string.Empty;
        public CreateCategoryPopup()
        {
            InitializeComponent();
        }
        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = tb_CategoryName.Text.Trim();
            DialogResult = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

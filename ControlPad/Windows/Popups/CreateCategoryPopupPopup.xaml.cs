using System.Windows;
using System.Windows.Forms;
using Wpf.Ui.Controls;

namespace ControlPad
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
            if (!string.IsNullOrWhiteSpace(tb_CategoryName.Text))
            {
                CategoryName = tb_CategoryName.Text.Trim();
                DialogResult = true;
            } 
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FluentWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CategoryName = tb_CategoryName.Text.Trim();
                DialogResult = true;
            }
        }
    }
}

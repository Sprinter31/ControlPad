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

namespace ControlPad.Windows
{
    /// <summary>
    /// Interaction logic for CreateCategoryPopup.xaml
    /// </summary>
    public partial class CreateCategoryPopup : Window
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

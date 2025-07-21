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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlPad
{
    public partial class ManageButtonCategoriesUserControl : UserControl
    {
        MainWindow mainWindow;
        public ManageButtonCategoriesUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btn_CreateCat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_EditCat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditButtonCategoryWindow();
            dialog.Owner = mainWindow;
            dialog.ShowDialog();
        }

        private void btn_DeleteCat_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

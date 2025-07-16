using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace ControlPad.Windows
{
    public partial class SelectFunctionPopup : FluentWindow
    {
        public SelectFunctionPopup()
        {
            InitializeComponent();          
        }

        private double currentHeight = 0;
        private void AddControl_Click(object sender, RoutedEventArgs e)
        {
            var newControl = new ButtonFunction();

            newControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double controlHeight = newControl.DesiredSize.Height;

            if (controlHeight == 0)
                controlHeight = 100; 

            if (currentHeight + controlHeight <= MaxHeight)
            {
                ControlContainer.Children.Add(newControl);
                currentHeight += controlHeight;
            }
        }
    }
}

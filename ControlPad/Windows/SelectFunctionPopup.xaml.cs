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

namespace ControlPad.Windows
{
    /// <summary>
    /// Interaction logic for SelectFunctionPopup.xaml
    /// </summary>
    public partial class SelectFunctionPopup : Window
    {
        public SelectFunctionPopup()
        {
            InitializeComponent();
            
            
        }

        private double currentHeight = 0;

        private void AddControl_Click(object sender, RoutedEventArgs e)
        {
            var newControl = new ButtonFunction(); // Dein UserControl

            // Optional: Vor dem Hinzufügen berechnen, wie viel Höhe es brauchen wird
            newControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double controlHeight = newControl.DesiredSize.Height;

            // Fallback falls DesiredSize noch 0 ist (z. B. beim ersten Mal)
            if (controlHeight == 0)
                controlHeight = 100; // Annahme oder feste Höhe

            if (currentHeight + controlHeight <= MaxHeight)
            {
                ControlContainer.Children.Add(newControl);
                currentHeight += controlHeight;
            }
            else
            {
                System.Windows.MessageBox.Show("Maximale Höhe erreicht.");
            }
        }
    }
}

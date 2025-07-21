using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlPad
{
    public class CustomButton : Wpf.Ui.Controls.Button
    {
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register(
                nameof(Category),
                typeof(ButtonCategory),
                typeof(CustomButton),
                new PropertyMetadata(null));

        public ButtonCategory? Category
        {
            get => (ButtonCategory?)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }
    }
}

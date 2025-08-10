using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlPad
{
    public class ButtonBorder : Border
    {
        public static readonly DependencyProperty ButtonProperty =
        DependencyProperty.Register(
            nameof(CustomButton),
            typeof(CustomButton),
            typeof(ButtonBorder),
            new FrameworkPropertyMetadata(null));

        public CustomButton CustomButton
        {
            get => (CustomButton)GetValue(ButtonProperty);
            set => SetValue(ButtonProperty, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlPad
{
    public class CustomButton : System.Windows.Controls.Primitives.ToggleButton
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

        public static readonly DependencyProperty TextBlockProperty =
            DependencyProperty.Register(
                nameof(TextBlock),
                typeof(TextBlock),
                typeof(CustomButton),
                new PropertyMetadata(null));

        public TextBlock TextBlock
        {
            get => (TextBlock)GetValue(TextBlockProperty);
            set => SetValue(TextBlockProperty, value);
        }

        static CustomButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CustomButton),
                new FrameworkPropertyMetadata(typeof(CustomButton)));
        }
    }
}

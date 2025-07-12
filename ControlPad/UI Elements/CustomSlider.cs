using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlPad
{
    public class CustomSlider : Slider
    {
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register(
                nameof(Category),
                typeof(Category),
                typeof(CustomSlider),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextBlockProperty =
            DependencyProperty.Register(
                nameof(TextBlock),
                typeof(TextBlock),
                typeof(CustomSlider),
                new PropertyMetadata(null));

        public Category? Category
        {
            get => (Category?)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public TextBlock TextBlock
        {
            get => (TextBlock)GetValue(TextBlockProperty);
            set => SetValue(TextBlockProperty, value);
        }
    }
}

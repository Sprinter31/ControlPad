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

        public Category? Category
        {
            get => (Category?)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public CustomSlider()
        {
            
        }
    }
}

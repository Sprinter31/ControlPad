using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlPad
{
    public class SliderBorder : Border
    {
        public static readonly DependencyProperty SliderProperty =
        DependencyProperty.Register(
            nameof(CategorySlider),
            typeof(CustomSlider),
            typeof(SliderBorder),
            new FrameworkPropertyMetadata(null));

        public CustomSlider CategorySlider
        {
            get => (CustomSlider)GetValue(SliderProperty);
            set => SetValue(SliderProperty, value);
        }
    }
}

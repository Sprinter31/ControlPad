using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPad
{
    public class SliderAssignment
    {
        public int CategoryId { get; private set; }

        public CategorySlider? CategorySlider { get; private set; }

        public SliderAssignment(CategorySlider categorySlider, int categoryId)
        {
            CategoryId = categoryId;
            CategorySlider = categorySlider;
        }
    }
}

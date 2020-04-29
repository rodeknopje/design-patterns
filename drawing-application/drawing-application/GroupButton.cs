using System;
using System.Collections.Generic;
using System.Text;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class GroupButton : ShapeButton
    {
        public GroupButton(CustomShape shape) : base(shape)
        {
            MouseRightButtonDown += (a, b) => OnRightClick();
        }

        private void OnRightClick()
        {
            
        }
    }
}

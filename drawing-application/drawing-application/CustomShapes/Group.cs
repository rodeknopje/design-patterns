using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace drawing_application.CustomShapes
{
    class Group : CustomShape
    {
        private List<CustomShape> childeren = new List<CustomShape>();

        protected override void DrawShape(out List<Point> coords)
        {
            coords = null;

            if (childeren.Count == 0)
            {

            }
        }
    }
}

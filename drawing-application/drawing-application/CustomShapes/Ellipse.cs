using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace drawing_application.CustomShapes
{
    class Ellipse : ShapeGroup
    {
        protected override void DrawShape(out List<Point> coords)
        {
            coords = new List<Point>();

            for (int i = 0; i < 65; i++)
            {
                var x =  (Width  * .5) + -Math.Cos(i*.1) * (Width  * .5) - StrokeThickness / 2;
                var y =  (Height * .5) +  Math.Sin(i*.1) * (Height * .5) - StrokeThickness / 2;

                coords.Add(new Point(x, y));
            }
        }
    }
}

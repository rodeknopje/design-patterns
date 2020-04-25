using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace drawing_application.CustomShapes
{
    public class Ellipse : CustomShape
    {
        protected override void DrawShape(out List<Point> coords)
        {
            coords = new List<Point>();

            var x_radius = (Width  * .5);
            var y_radius = (Height * .5);

            for (var i = 0; i < 65; i++)
            {
                var x =  x_radius + -Math.Cos(i*.1) * x_radius - StrokeThickness;
                var y =  y_radius +  Math.Sin(i*.1) * y_radius - StrokeThickness;

                coords.Add(new Point(x, y));
            }
        }
    }
}

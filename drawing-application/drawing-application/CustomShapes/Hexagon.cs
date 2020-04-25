using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using drawing_application.CustomShapes;

namespace drawing_application.CustomShapes
{
    public class Hexagon : CustomShape
    {
        protected override void DrawShape(out List<Point> coords)
        {
            coords = new List<Point>
            {
                new Point(0.25,1),
                new Point(0.75,1),
                new Point(1,.5),
                new Point(.75,0),
                new Point(.25,0),
                new Point(0,.5),
            }
            .Select(i => new Point(i.X * Width - StrokeThickness, i.Y * Height - StrokeThickness)).ToList();
        }
    }
}

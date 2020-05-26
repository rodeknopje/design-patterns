using System;
using System.Collections.Generic;
using System.Windows;
using drawing_application.Strategies;

namespace drawing_application.CustomShapes
{
    public class Ellipse : CustomShape
    {
        public Ellipse() : base(new EllipseStrategy())
        {
                
        }

        protected override void DrawShape(out List<Point> coords)
        {
            // initialize the coordinates.
            coords = new List<Point>();
            // calculate the radius.
            var xRadius = (Width  * .5);
            var yRadius = (Height * .5);

            for (var i = 0; i < 65; i++)
            {
                // calculate the next coordinate.
                var x =  xRadius + -Math.Cos(i*.1) * xRadius - StrokeThickness;
                var y =  yRadius +  Math.Sin(i*.1) * yRadius - StrokeThickness;
                // add it to the coordinates list.
                coords.Add(new Point(x, y));
            }
        }
    }
}

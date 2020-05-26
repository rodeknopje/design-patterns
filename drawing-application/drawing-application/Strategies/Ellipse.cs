using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace drawing_application.Strategies
{
    public class Ellipse : IStrategyShape
    {
        public List<Point> Draw(CustomShape shape)
        {
            // initialize the coordinates.
            var coords = new List<Point>();
            // calculate the radius.
            var xRadius = (shape.Width  * .5);
            var yRadius = (shape.Height * .5);

            for (var i = 0; i < 65; i++)
            {
                // calculate the next coordinate.
                var x = xRadius + -Math.Cos(i * .1)  * xRadius - shape.StrokeThickness;
                var y = yRadius +  Math.Sin(i * .1)  * yRadius - shape.StrokeThickness;
                // add it to the coordinates list.
                coords.Add(new Point(x, y));
            }

            return coords;
        }
    }
}

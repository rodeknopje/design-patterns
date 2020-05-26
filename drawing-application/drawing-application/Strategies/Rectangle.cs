using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Windows;
using drawing_application.CustomShapes;

namespace drawing_application.Strategies
{
    public class Rectangle : IStrategyShape
    {
        public List<Point> Draw(CustomShape shape)
        {
            return new List<Point> 
            {
                    new Point(0,0),
                    new Point(1,0),
                    new Point(1,1),
                    new Point(0,1),
           }
           .Select(i => new Point(i.X * shape.Width - shape.StrokeThickness, i.Y * shape.Height - shape.StrokeThickness)).ToList();
        }


    }
}

using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace drawing_application.Strategies
{
    public class StarStrategy : IStrategyShape
    {
        public List<Point> Draw(CustomShape shape)
        {
            return  new List<Point> 
            {
                    new Point(.5f,0),
                    new Point(.625f,.4f),
                    new Point(1,.4f),
                    new Point(.69f,.625f),
                    new Point(.8f,1),
                    new Point(.5f,.775f),
                    new Point(.2f,1),
                    new Point(.31f,.625f),
                    new Point(0,.4f),
                    new Point(.375f,.4f),
            }
            .Select(i => new Point(i.X * shape.Width - shape.StrokeThickness, i.Y * shape.Height - shape.StrokeThickness)).ToList();
        }
    }
}

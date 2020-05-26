using System.Collections.Generic;
using System.Linq;
using System.Windows;
using drawing_application.Strategies;

namespace drawing_application.CustomShapes
{
    public class Rectangle : CustomShape
    {

        public Rectangle(): base(new RectangleStrategy())
        {
                
        }
        protected override void DrawShape(out List<Point> coords)
        {

            coords = new List<Point>
            {
                new Point(0,0),
                new Point(1,0),
                new Point(1,1),
                new Point(0,1),
            }
            .Select(i=>new Point(i.X*Width-StrokeThickness,i.Y*Height-StrokeThickness)).ToList();
        }
    }
}

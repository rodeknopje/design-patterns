using System.Collections.Generic;

using System.Linq;
using System.Windows;

namespace drawing_application.CustomShapes
{
    public class Rectangle : ShapeGroup
    {
        protected override void DrawShape(out List<Point> coords)
        {
            coords = new List<Point>
            {
                new Point(0,0),
                new Point(1,0),
                new Point(1,1),
                new Point(0,1),
            }
            .Select(i=>i= new Point(i.X*Width,i.Y*Height)).ToList();
        }
    }
}

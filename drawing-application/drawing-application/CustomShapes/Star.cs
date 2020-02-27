using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace drawing_application.CustomShapes
{
    class Star : ShapeGroup
    {
        protected override void DrawShape(out List<Point> coords)
        { 
            coords =  new List<Point>
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
            .Select(i=>i=new Point(i.X*Width,i.Y*Height)).ToList();
        }

    }
}
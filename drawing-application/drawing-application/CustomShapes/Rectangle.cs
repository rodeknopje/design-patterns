using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace drawing_application.CustomShapes
{
    class Rectangle : ShapeGroup
    {

        protected override List<(float x, float y)> InitializeCoords()
        {
            return new List<(float x, float y)>
            {
                (0,0),
                (1,0),
                (1,1),
                (0,1),
            };
        }

        protected override Geometry InitializeGeometry()
        {
            StreamGeometry geom = new StreamGeometry();

            using (var gc = geom.Open())
            {
                gc.BeginFigure(new Point(Width * coords[0].x, Height * coords[0].y), true, true);

                for (int i = 1; i < coords.Count; i++)
                {
                    gc.LineTo(new Point(Width * coords[i].x, Height * coords[i].y), true, false);
                };
            }
            return geom;
        }
    }
}

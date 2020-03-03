using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.CustomShapes
{
    public abstract class CustomShape : Shape
    {


        protected Geometry DefineGeometry()
        {
            DrawShape(out coords);
            
            StreamGeometry geom = new StreamGeometry();

            using (var gc = geom.Open())
            {
                gc.BeginFigure(new Point(coords[0].X, coords[0].Y), true, true);

                for (int i = 1; i < coords.Count; i++)
                {
                    gc.LineTo(new Point(coords[i].X, coords[i].Y), true, true);
                }
            }
            return geom;
        }

        protected abstract void DrawShape(out List<Point> coords);

        private List<Point> coords;
        protected override Geometry DefiningGeometry => DefineGeometry();
    }
}

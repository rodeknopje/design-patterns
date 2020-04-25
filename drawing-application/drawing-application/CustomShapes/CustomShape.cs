using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.CustomShapes
{
    public abstract class CustomShape : Shape
    {
        public Transform originTransform { get; protected set; }

        public virtual void UpdateOriginTransform()
        {
            originTransform = new Transform(Canvas.GetLeft(this),Canvas.GetTop(this),Width,Height);
        }

        public virtual void Move(Point offset)
        {
            Canvas.SetLeft(this, offset.X + originTransform.x);
            Canvas.SetTop (this, offset.Y + originTransform.y);     
        }



        protected Geometry DefineGeometry()
        {
            DrawShape(out coords);
            
            var geom = new StreamGeometry();

            if (coords == null)
            {
                return geom;
            }

            using (var gc = geom.Open())
            {
                gc.BeginFigure(new Point(coords[0].X, coords[0].Y), true, true);

                for (var i = 1; i < coords.Count; i++)
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

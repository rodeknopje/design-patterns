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
        public Transform orginTransform { get; private set; }

        public virtual void UpdateOrginTransform()
        {
            orginTransform = new Transform(Canvas.GetLeft(this),Canvas.GetTop(this),Width,Height);
        }

        public virtual void Move(Point offset)
        {
            Canvas.SetLeft(this, offset.X + orginTransform.x);
            Canvas.SetTop (this, offset.Y + orginTransform.y);     
        }

        public virtual void Scale(Transform diff)
        {
            //Width  = orginTransform.width  * diff.width;
            //Height = orginTransform.heigth * diff.heigth;

            //var xpercent = Canvas.GetLeft(this) - Canvas.GetLeft(MainWindow.ins.selection.outline) * diff.x;
            //var ypercent = Canvas.GetTop(this) - Canvas.GetTop(MainWindow.ins.selection.outline) * diff.y;

            //Canvas.SetLeft(this, xpercent);
            //Canvas.SetTop(this, ypercent);

            //Canvas.SetLeft(this, orginPos.X * diff.x);
        }

        protected Geometry DefineGeometry()
        {
            DrawShape(out coords);
            
            StreamGeometry geom = new StreamGeometry();

            if (coords == null)
            {
                return geom;
            }

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

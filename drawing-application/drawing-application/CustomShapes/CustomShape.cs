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
        public Point orginPos   { get; private set; }
        public Point orginScale { get; private set; }
        
        public virtual void UpdateOrginPos()
        {
            orginPos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
        }

        public virtual void UpdateOrginScale()
        {
            orginScale = new Point(Width, Height);
        }

        public virtual void Move(Point offset)
        {
            Canvas.SetLeft(this, offset.X + orginPos.X);
            Canvas.SetTop (this, offset.Y + orginPos.Y);
        }

        public virtual void Scale(Point diff)
        {
            throw new NotImplementedException();
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

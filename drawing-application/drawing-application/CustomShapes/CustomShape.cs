using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.CustomShapes
{
    public abstract class CustomShape : Shape
    {
        // the coordinates of connection points.
        private List<Point> coords;

        // the transform before any operation is performed on this shape.
        public Transform OriginTransform { get; protected set; }

        // abstract method which needs to calculate the coords.
        protected abstract void DrawShape(out List<Point> coords);

        public virtual void UpdateOriginTransform()
        {
            // update the transform to their current position and size
            OriginTransform = new Transform(Canvas.GetLeft(this),Canvas.GetTop(this),Width,Height);
        }

        public virtual void Move(Point offset)
        {
            // move the shape based on their origin transform plus an offset.
            Canvas.SetLeft(this, offset.X + OriginTransform.x);
            Canvas.SetTop (this, offset.Y + OriginTransform.y);     
        }



        protected Geometry DefineGeometry()
        {
            // calculate the new coords.
            DrawShape(out coords);
            
            // create a new stream geometry to connect the coords.
            var geom = new StreamGeometry();

            // if the coords are null return, happens with groups.
            if (coords == null)
            {
                return geom;
            }

            // open de stream geometry.
            using (var gc = geom.Open())
            {
                // begin the stream with the first point in the coords list.
                gc.BeginFigure(new Point(coords[0].X, coords[0].Y), true, true);
                // loop through the rest of the points.
                for (var i = 1; i < coords.Count; i++)
                {
                    // connect them.
                    gc.LineTo(new Point(coords[i].X, coords[i].Y), true, true);
                }
            }
            // return the geometry.
            return geom;
        }

        // calculate the geometry when asked for it.
        protected override Geometry DefiningGeometry => DefineGeometry();
    }
}

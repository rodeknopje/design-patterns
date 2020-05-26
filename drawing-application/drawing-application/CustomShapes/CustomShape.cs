using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using drawing_application.Strategies;
using drawing_application.Visitors;

namespace drawing_application.CustomShapes
{
    public abstract class CustomShape : Shape, IVisitable
    {
        // the coordinates of connection points.
        private List<Point> coords;

        // the transform before any operation is performed on this shape.
        public Transform OriginTransform { get; protected set; }

        // abstract method which needs to calculate the coords.
        protected abstract void DrawShape(out List<Point> coords);

        private readonly IStrategyShape iStrategyShape;

        protected CustomShape()
        {
            iStrategyShape = new HexagonStrategy();
            // set the width and the height
            Width = 0; Height = 0;
            // make the shape transparent.
            Fill = Brushes.Transparent;
            // set the color.
            Stroke = new SolidColorBrush(Color.FromRgb(236, 240, 241));
            // set the stroke thickness.
            StrokeThickness = 2.5;
        }


        public virtual void UpdateOriginTransform()
        {
            // update the transform to their current position and size
            OriginTransform = new Transform(Canvas.GetLeft(this),Canvas.GetTop(this),Width,Height);
        }

        public virtual void Move(Point offset)
        {
            // Move the shape based on their origin transform plus an offset.
            Canvas.SetLeft(this, offset.X + OriginTransform.x);
            Canvas.SetTop (this, offset.Y + OriginTransform.y);     
        }

        public virtual void SetActive(bool state)
        {
            if (state)
            {
            
                MainWindow.ins.drawCanvas.Children.Add(this);
            }
            else
            {
                MainWindow.ins.drawCanvas.Children.Remove(this);
            }
        }


        private Geometry DefineGeometry()
        {
            // calculate the new coords.
            //DrawShape(out coords);
            coords = iStrategyShape.Draw(this);
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

        public virtual string ToString(int level)
        {
            return $"{new string(' ', level*2)}{GetType().Name} {(int)Canvas.GetLeft(this)} {(int)Canvas.GetTop(this)} {(int)Width} {(int)Height}";
        }

        public virtual string Accept(IVisitor iVisitor)
        {
            return iVisitor.Visit(this);
        }
    }

}

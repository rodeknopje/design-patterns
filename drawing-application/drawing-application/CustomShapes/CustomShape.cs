using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using drawing_application.Decorators;
using drawing_application.Strategies;
using drawing_application.Visitors;

namespace drawing_application.CustomShapes
{
    public  class CustomShape : Shape, IVisitable
    {

        // the transform before any operation is performed on this shape.
        protected Transform OriginTransform;

        // abstract method which needs to calculate the coords.
        private readonly IStrategyShape iStrategyShape;



        public CustomShape(IStrategyShape iStrategyShape)
        {
            this.iStrategyShape = iStrategyShape;
            // set the width and the height
            Width = 0; Height = 0;
            // make the shape transparent.
            Fill = Brushes.Transparent;
            // set the color.
            Stroke = new SolidColorBrush(Color.FromRgb(236, 240, 241));
            // set the stroke thickness.
            StrokeThickness = 2.5;
        }


        public override string ToString()
        {
            return iStrategyShape.GetType().Name;
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
            var coords = iStrategyShape?.Draw(this);
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
        protected override Geometry DefiningGeometry =>  DefineGeometry();

        public virtual string Accept(IVisitor iVisitor)
        {
            return iVisitor.Visit(this);
        }

  
        public virtual double GetLeft()
        {
            return Canvas.GetLeft(this);
        }

        public virtual double GetTop()
        {
            return Canvas.GetTop(this);
        }

        public virtual double GetWidth()
        {
            return Width;
        }

        public virtual double GetHeight()
        {
            return Height;
        }

        public virtual void SetLeft(double left)
        {
            Canvas.SetLeft(this, left);
        }
        public virtual void SetTop(double top)
        {
            Canvas.SetTop(this, top);
        }

        public virtual void SetWidth(double width)
        {
            Width = width;
        }

        public virtual void SetHeight(double height)
        {
            Height = height;
        }

        public virtual Transform GetOriginTransform()
        {
            return OriginTransform;
        }
    }

}

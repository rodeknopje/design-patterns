using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;


namespace drawing_application
{
    public partial class MainWindow : Window
    {

        List<Shape> shapelist = new List<Shape>();

        shapes shape_style;

        Point shape_orgin;

        Shape shape;

        public MainWindow()
        {
            InitializeComponent();
            // initialze the shape buttons.
            select_rectangle.Click += (a,b) => shape_style = shapes.rectangle;
            select_ellipse.Click   += (a,b) => shape_style = shapes.ellipse;
            // initialize the clear buttons.
            select_clear.Click += (a,b) => draw_canvas.Children.Clear();
        }


        private void Canvas_Mousedown(object sender, MouseButtonEventArgs e)
        {
            // check to prevent double clicks.
            if (shape != null)
            {
                return;
            }
            // create a point variable to store coordinates
            shape_orgin = e.GetPosition(draw_canvas);

            // create a new shape based on the selected shape.
            if (shape_style == shapes.rectangle)
            {
                shape = new Rectangle
                {
                    Width   = 0,
                    Height  = 0,
                    Fill    = Brushes.Transparent,
                    Stroke  = Brushes.White,
                    StrokeThickness = 2.5
                };
            }
            else
            {
                shape = new Ellipse
                {
                    Width   = 0,
                    Height  = 0,
                    Fill    = Brushes.Transparent,
                    Stroke  = Brushes.White,
                    StrokeThickness = 2.5
                };
            }


            // set the position of the shape.
            Canvas.SetLeft(shape, shape_orgin.X);
            Canvas.SetTop (shape, shape_orgin.Y);
            // add it to the canvas.
            draw_canvas.Children.Add(shape);

            shapelist.Add(shape);
        }

        private void ShapeSelect(Shape shape)
        {
            shape.Stroke = Brushes.Green;
        }
        private void canvas_lmbdown(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(draw_canvas);

            foreach (var _shape in shapelist)
            {
                if (point.X > Canvas.GetLeft(_shape) && point.X < Canvas.GetLeft(_shape) + _shape.Width &&
                    point.Y > Canvas.GetTop(_shape)  && point.Y < Canvas.GetTop(_shape)  + _shape.Width)
                {
                    _shape.Stroke = Brushes.Green;

                    return;
                }
            }
        }

        private void Canvas_Mousemove(object sender, MouseEventArgs e)

        {
            // if the program is not drawing anything.
            if (shape == null)
            {
                return;
            }
            // get the offset from the orgin point.
            var x_offset = e.GetPosition(draw_canvas).X - shape_orgin.X;
            var y_offset = e.GetPosition(draw_canvas).Y - shape_orgin.Y;
            // if the x offset is greater than zero.
            if (x_offset > 0)
            {
                // set the width to the offset.
                shape.Width = x_offset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetLeft(shape, x_offset + shape_orgin.X);

                shape.Width = -x_offset;
            }
            if (y_offset > 0)
            {
                shape.Height = y_offset;
            }
            else
            {
                Canvas.SetTop(shape, y_offset + shape_orgin.Y);

                shape.Height = -y_offset;
            }

        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
            shape = null;

        }
    }


    public enum shapes
    {
        rectangle,
        ellipse,
    }

}

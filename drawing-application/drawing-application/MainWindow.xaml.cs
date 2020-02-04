using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace drawing_application
{
    public partial class MainWindow : Window
    {
        shapes shape_style;

        Shape shape;

        public MainWindow()
        {
            InitializeComponent();
            // initialze the shape buttons.
            select_rectangle.Click +=(a,b)=>shape_style = shapes.rectangle;
            select_ellipse.Click   +=(a,b)=>shape_style = shapes.ellipse;
            // initialize the clear buttons.
            select_clear.Click +=(a,b)=> draw_canvas.Children.Clear();
        }


        private void Canvas_Mousedown(object sender, MouseButtonEventArgs e)
        {
            // check to prevent double clicks.
            if (shape != null)
            {
                return;
            }
            // create a point variable to store coordinates
            var point = e.GetPosition(draw_canvas);


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
            Canvas.SetLeft(shape, point.X);
            Canvas.SetTop (shape, point.Y);
            // add it to the canvas.
            draw_canvas.Children.Add(shape);
        }

        private void Canvas_Mousemove(object sender, MouseEventArgs e)
        {
            // if the program is not drawing anything.
            if (shape == null)
            {
                return;
            }
            // get the coordinates of the mouse, relative to the shape.
            var x_pos = e.GetPosition(shape).X;
            var y_pos = e.GetPosition(shape).Y;


            if (x_pos > 0)
            {
                shape.Width = e.GetPosition(shape).X;
            }
            if (y_pos > 0)
            {
                shape.Height = e.GetPosition(shape).Y;
            }
            
        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            //is_drawing = false;
            shape = null;
        }
    }


    public enum shapes
    {
        rectangle,
        ellipse,
    }

}

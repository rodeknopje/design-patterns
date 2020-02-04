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
        shapes current_shape;

        bool is_drawing = false;

        Vector start_pos = new Vector(0, 0);

        Shape shape;


        public MainWindow()
        {
            InitializeComponent();

        }

        private void select_rectangle_Click(object sender, RoutedEventArgs e)
        {
            current_shape = shapes.rectangle;
        }

        private void select_ellipse_Click(object sender, RoutedEventArgs e)
        {
            current_shape = shapes.ellipse;
        }

        private void Canvas_Mousedown(object sender, MouseButtonEventArgs e)
        {
            if (is_drawing == true)
            {
                return;
            }

            Trace.WriteLine(e.GetPosition(draw_canvas).X + "-" + e.GetPosition(draw_canvas).Y);

            start_pos = new Vector(e.GetPosition(draw_canvas).X, e.GetPosition(draw_canvas).Y);

            is_drawing = true;


            if (current_shape == shapes.rectangle)
            {
                shape = new Rectangle
                {
                    Width   = 0,
                    Height  = 0,
                    Fill    = Brushes.White,
                    Stroke  = Brushes.Black,
                };
            }
            else
            {
                shape = new Ellipse
                {
                    Width   = 0,
                    Height  = 0,
                    Fill    = Brushes.White,
                    Stroke  = Brushes.Black,
                };
            }


            Canvas.SetLeft(shape, start_pos.X);
            Canvas.SetTop(shape, start_pos.Y);

            draw_canvas.Children.Add(shape);
        }

        private void Canvas_Mousemove(object sender, MouseEventArgs e)
        {
            if (is_drawing)
            {
                shape.Width  = e.GetPosition(shape).X;
                shape.Height = e.GetPosition(shape).Y;
            }
        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            is_drawing = false;
        }
    }


    public enum shapes
    {
        ellipse,
        rectangle
    }

}

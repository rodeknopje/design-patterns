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

        Rectangle selection_outline;

        Shape selected;

        public MainWindow()
        {
            InitializeComponent();
            // initialze the shape buttons.
            select_rectangle.Click += (a, b) => shape_style = shapes.rectangle;
            select_ellipse.Click += (a, b) => shape_style = shapes.ellipse;
            // initialize the clear buttons.
            select_clear.Click += (a, b) => draw_canvas.Children.Clear();
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
                    Width = 0,
                    Height = 0,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.White,
                    StrokeThickness = 2.5,
                };
            }
            else
            {
                shape = new Ellipse
                {
                    Width = 0,
                    Height = 0,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.White,
                    StrokeThickness = 2.5
                };
            }


            // set the position of the shape.
            Canvas.SetLeft(shape, shape_orgin.X);
            Canvas.SetTop(shape, shape_orgin.Y);
            // add it to the canvas.
            draw_canvas.Children.Add(shape);

            shapelist.Add(shape);
        }

        private void ShapeSelect(Shape shape)
        {
            shape.Stroke = Brushes.Green;
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
            selected = shape;
            // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
            shape = null;

            InstantiateSelectionOutline();
        }

        private void InstantiateSelectionOutline()
        {
            // if there is already something selected
            if (selection_outline != null)
            {
                // remove the current selection outline.
                draw_canvas.Children.Remove(selection_outline);
            }

            // assign the selection outline.
            selection_outline = new Rectangle
            {
                Fill = Brushes.Transparent,
                Stroke = Brushes.Yellow,
                StrokeThickness =  2f,
                StrokeDashArray = {5,5}
                
            };
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(selection_outline, Canvas.GetLeft(selected) -selection_outline.StrokeThickness*2 );
            Canvas.SetTop(selection_outline,  Canvas.GetTop(selected)  -selection_outline.StrokeThickness*2);
            // set the width and heigth to be the same as the selected shape.
            selection_outline.Width  = selected.Width  + selection_outline.StrokeThickness * 4;
            selection_outline.Height = selected.Height + selection_outline.StrokeThickness * 4;
            // add the selection outline to the draw_canvas.
            draw_canvas.Children.Add(selection_outline);

            selection_outline.MouseEnter += (a,b) => Mouse.OverrideCursor = Cursors.Hand;
            selection_outline.MouseLeave += (a,b) => Mouse.OverrideCursor = Cursors.Arrow;
        }
    }


    public enum shapes
    {
        rectangle,
        ellipse,
    }

}

using drawing_application.Commands;
using drawing_application.CustomShapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application
{

    public class Selection
    {
        Group group;

        public CustomShape shape;

        private Shape outline;

        private Shape handle;

        public Selection()
        {
            group = new Group();
            // assign the selection outline.
            outline = new System.Windows.Shapes.Rectangle
            {
                Fill   = Brushes.Transparent,
                Stroke = Brushes.White,
                StrokeThickness = 2f,
                StrokeDashArray = { 5, 5 }
            };
            // create the handle.
            handle = new System.Windows.Shapes.Ellipse
            {
                Width  = 20,
                Height = 20,      
                Stroke = Brushes.White,
                Fill   = Brushes.Gray,
                StrokeThickness = 2,
            };

            // when the selection outline is clicked.
            outline.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.SizeAll;
            outline.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;

            // give the cursor a different icon, when hovering over the outlne.
            handle.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.Hand;
            handle.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;

            // when the selection outline is clicked.
            outline.MouseLeftButtonDown += (a, b) => new StartMoveCommand(b.GetPosition(MainWindow.ins.draw_canvas)).Execute();
            // when the handle is clicked.
            handle.MouseDown += (a, b) => new StartResizeCommand(b.GetPosition(MainWindow.ins.draw_canvas)).Execute();
        }


        public void Add(CustomShape shape)
        {
            group.AddChild(shape);
        }


        public (double x, double y, double width, double heigth) CalculateTransform()
        {
            // Create a transform tuple.
            (double x, double y, double width, double heigth) transform = (double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);

            // loop through all childeren in the selection.
            foreach (var shape in group.GetChilderen())
            {
                // get their x and y coords.
                var x = Canvas.GetLeft(shape);
                var y = Canvas.GetTop (shape);

                // check if this is the lowes x so far.
                if (x < transform.x)
                {
                    // is so assign it.
                    transform.x = x;
                }
                // check if this is the lowes x so far.
                if (y < transform.y)
                {
                    // is so assign it.
                    transform.y = y;
                }

                // calculate the width and assign it.
                var width = shape.Width + x;
                // calculate the heigth and assign it.
                var heigth = shape.Height + y;

                // check if its to biggest so far.
                if (width > transform.width)
                {
                    // if so assign it
                    transform.width = width;
                }
                // check if its to biggest so far.
                if (heigth > transform.heigth)
                {
                    // if so assign it
                    transform.heigth = heigth;
                }
            }
            // calculate the width of the final transform.
            transform.width -= transform.x;
            // calculate the heigth of the final transform.
            transform.heigth -= transform.y;
            // return the transform
            return transform;
        }


        public void DrawOutline()
        {
            var transform = CalculateTransform();

            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(outline, Canvas.GetLeft(shape) - outline.StrokeThickness * 2);
            Canvas.SetTop (outline,  Canvas.GetTop(shape) - outline.StrokeThickness * 2);

            // set the width and heigth to be the same as the selected shape.
            outline.Width  = shape.Width  + outline.StrokeThickness * 4;
            outline.Height = shape.Height + outline.StrokeThickness * 4;

            // move the resize handle it to the bottum right.
            Canvas.SetLeft(handle, Canvas.GetLeft(outline) + outline.Width  - handle.Width  / 2);
            Canvas.SetTop (handle,  Canvas.GetTop(outline) + outline.Height - handle.Height / 2);
        }

        public void ToggleOutline(bool state)
        {
            if (state)
            {
                MainWindow.ins.draw_canvas.Children.Add(outline);
                MainWindow.ins.draw_canvas.Children.Add(handle);
            }
            else
            {
                MainWindow.ins.draw_canvas.Children.Remove(outline);
                MainWindow.ins.draw_canvas.Children.Remove(handle);
            }
        }
    }
}

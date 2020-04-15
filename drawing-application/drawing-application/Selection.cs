using drawing_application.Commands;
using drawing_application.CustomShapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace drawing_application
{

    public class Selection : Group
    {
        public Rectangle outline { get; private set; }

        public Ellipse handle { get; private set; }


        public Selection()
        {
            // assign the selection outline.
            outline = new Rectangle
            {
                Fill   = Brushes.Transparent,
                Stroke = Brushes.White,
                StrokeThickness = 2f,
                StrokeDashArray = { 5, 5 }
            };
            // create the handle.
            handle = new Ellipse
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
            handle.MouseLeftButtonDown += (a, b) => new StartResizeCommand(b.GetPosition(MainWindow.ins.draw_canvas)).Execute();
        }

        public override void Move(Point offset)
        {
            // move the group handle and outline.
            base.Move   (offset);
            handle.Move (offset);
            outline.Move(offset);
        }


        public void Merge()
        {
            var merged = new Group();

            foreach (var child in GetChilderen())
            {
                merged.AddChild(child);
            }
        }


        public void Select(CustomShape shape)
        {
            if (GetAllShapes().Contains(shape) == false)
            {
                AddChild(shape);
            }
            // toggle the outline on.
            ToggleOutline(true);
        }



        public override void RemoveChild(CustomShape shape)
        {
            base.RemoveChild(shape);
            // if there are still childeren
            if(GetChilderen().Count > 0)
            {
                // togle the outline.
                ToggleOutline(true);
            }
            
        }

        public Transform GetTransform()
        {
            return orginTransform;
        }

        public void ApplyOutlineOffset(Point offset)
        {
            // move the handle.
            handle.Move(offset);

            // if the outline is on the right side.
            if (offset.X + orginTransform.width > 0 )
            {
                // apply the offset to the width.
                outline.Width = orginTransform.width + offset.X;
            }
            // if the outline is on the top side.
            if(offset.Y + orginTransform.heigth > 0)
            {
                // apply the offset to the heigth.
                outline.Height = orginTransform.heigth + offset.Y;
            }
            // if the outline is on the left side.
            if(offset.X + orginTransform.width <= 0)
            {
                // apply the offset to the x position
                Canvas.SetLeft(outline, offset.X + orginTransform.x + orginTransform.width);
                // inverse the offset and substract the width to make the widt face the original x position
                outline.Width = -(offset.X + orginTransform.width);
            }
            // if the outline is on the bottum side.
            if (offset.Y + orginTransform.heigth <= 0)
            {
                // apply the offset to the y position
                Canvas.SetTop(outline, offset.Y + orginTransform.y + orginTransform.heigth);
                // inverse the offset and substract the heigth to make the widt face the original y position
                outline.Height = -(offset.Y + orginTransform.heigth);
            }
        }

        public void CalculateTransform()
        {
            // Create a transform tuple.
            Transform transform = new Transform(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);

            // loop through all childeren in the selection.
            foreach (var shape in GetAllShapes())
            {
                // get their x and y coords.
                var x = Canvas.GetLeft(shape);
                var y = Canvas.GetTop (shape);

                // check if this is the lowest x so far.
                if (x < transform.x)
                {
                    // is so assign it.
                    transform.x = x;
                }
                // check if this is the lowest y so far.
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
            // assign the transform to the orgin transform.
            orginTransform = transform;
        }


        public void DrawOutline()
        {
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(outline, orginTransform.x);
            Canvas.SetTop (outline, orginTransform.y);

            // set the width and heigth to be the same as the selected shape.
            outline.Width  = orginTransform.width ;
            outline.Height = orginTransform.heigth;

            // move the resize handle it to the bottum right.
            Canvas.SetLeft(handle, Canvas.GetLeft(outline) + outline.Width  - handle.Width  / 2);
            Canvas.SetTop (handle,  Canvas.GetTop(outline) + outline.Height - handle.Height / 2);
        }

        public void ToggleOutline(bool state)
        {
            MainWindow.ins.draw_canvas.Children.Remove(outline);
            MainWindow.ins.draw_canvas.Children.Remove(handle);

            if (state)
            {
                // calculate the transform of the outline.
                CalculateTransform();
                // draw it for the first time.
                DrawOutline();
                // update the orgin pos and scale of the outlnie.
                outline.UpdateOrginTransform();
                // and also for the handle.
                handle.UpdateOrginTransform();
                // update the orgin pos and scale of the outlnie.
                UpdateOrginTransform();
                // instantiate it.
                MainWindow.ins.draw_canvas.Children.Add(outline);
                MainWindow.ins.draw_canvas.Children.Add(handle);
            }
            else
            {
                // remove it.
                MainWindow.ins.draw_canvas.Children.Remove(outline);
                MainWindow.ins.draw_canvas.Children.Remove(handle);
            }
        }
    }
}

public struct Transform 
{
    public double x;
    public double y;
    public double width;
    public double heigth;

    public Transform(double x, double y, double width, double heigth)
    {
        this.x = x;
        this.y = y;

        this.width  = width;
        this.heigth = heigth;
    }
}
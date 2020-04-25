using drawing_application.Commands;
using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace drawing_application
{

    public class Selection : Group
    {
        public Rectangle outline { get; }

        public Ellipse handle { get; }

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

            // give the cursor a different icon, when hovering over the outline.
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

            foreach (var child in GetChildren())
            {
                merged.AddChild(child);
            }
        }

        public void Select(List<CustomShape> shapes)
        {
            shapes.ForEach(AddChild);
            // toggle the outline on.
            ToggleOutline(true);
        }

        public override void Clear()
        {
            base.Clear();

            ToggleOutline(false);
        }

        public override void RemoveChild(CustomShape shape)
        {
            base.RemoveChild(shape);
            // remove or update the outline bases on if there are children in the group.
            ToggleOutline(GetChildren().Any());
        }

        public Transform GetTransform()
        {
            return OriginTransform;
        }

        public void ApplyOutlineOffset(Point offset)
        {
            // move the handle.
            handle.Move(offset);

            // if the outline is on the right side.
            if (offset.X + OriginTransform.width > 0 )
            {
                // apply the offset to the width.
                outline.Width = OriginTransform.width + offset.X;
            }
            // if the outline is on the top side.
            if(offset.Y + OriginTransform.heigth > 0)
            {
                // apply the offset to the heigth.
                outline.Height = OriginTransform.heigth + offset.Y;
            }
            // if the outline is on the left side.
            if(offset.X + OriginTransform.width <= 0)
            {
                // apply the offset to the x position
                Canvas.SetLeft(outline, offset.X + OriginTransform.x + OriginTransform.width);
                // inverse the offset and substract the width to make the widt face the original x position
                outline.Width = -(offset.X + OriginTransform.width);
            }
            // if the outline is on the bottum  side.
            if (offset.Y + OriginTransform.heigth <= 0)
            {
                // apply the offset to the y position
                Canvas.SetTop(outline, offset.Y + OriginTransform.y + OriginTransform.heigth);
                // inverse the offset and substract the heigth to make the widt face the original y position
                outline.Height = -(offset.Y + OriginTransform.heigth);
            }
        }

        public void CalculateTransform()
        {
            // Create a transform tuple.
            Transform transform = new Transform(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);

            // loop through all childrenin the selection.
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
            // calculate the height of the final transform.
            transform.heigth -= transform.y;
            // assign the transform to the origin transform.
            OriginTransform = transform;
        }


        public void DrawOutline()
        {
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(outline, OriginTransform.x);
            Canvas.SetTop (outline, OriginTransform.y);

            // set the width and heigth to be the same as the selected shape.
            outline.Width  = OriginTransform.width ;
            outline.Height = OriginTransform.heigth;

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
                // update the origin pos and scale of the outline.
                outline.UpdateOriginTransform();
                // and also for the handle.
                handle.UpdateOriginTransform();
                // update the origin pos and scale of the outline.
                UpdateOriginTransform();
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
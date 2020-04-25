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
        private static Selection _instance;

        // the outline to display when shapes are selected.
        public readonly Rectangle outline;
        // the handle to display when shapes are selected.
        public readonly Ellipse handle;


        public static Selection GetInstance()
        {
            return _instance ?? new Selection();
        }

        private Selection()
        {
            _instance = this;

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
            outline.MouseLeftButtonDown += (a, b) => new StartMoveCommand(b.GetPosition(MainWindow.ins.drawCanvas)).Execute();
            // when the handle is clicked.
            handle.MouseLeftButtonDown += (a, b) => new StartResizeCommand(b.GetPosition(MainWindow.ins.drawCanvas)).Execute();
        }

        public override void Move(Point offset)
        {
            // Move the group handle and outline.
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
            // add the given shapes to the group.
            shapes.ForEach(AddChild);
            // toggle the outline on.
            ToggleOutline(true);
        }

        public override void Clear()
        {
            // clear the children.
            base.Clear();
            // toggle the outline to false.
            ToggleOutline(false);
        }

        public override void RemoveChild(CustomShape shape)
        {
            // remove the given shape.
            base.RemoveChild(shape);
            // remove or update the outline bases on if there are children in the group.
            ToggleOutline(GetChildren().Any());
        }

        public Transform GetTransform() => OriginTransform;

        public void ApplyOutlineOffset(Point offset)
        {
            // Move the handle.
            handle.Move(offset);

            // if the outline is on the right side.
            if (offset.X + OriginTransform.width > 0 )
            {
                // apply the offset to the width.
                outline.Width = OriginTransform.width + offset.X;
            }
            // if the outline is on the top side.
            if(offset.Y + OriginTransform.height > 0)
            {
                // apply the offset to the height.
                outline.Height = OriginTransform.height + offset.Y;
            }
            // if the outline is on the left side.
            if(offset.X + OriginTransform.width <= 0)
            {
                // apply the offset to the x position
                Canvas.SetLeft(outline, offset.X + OriginTransform.x + OriginTransform.width);
                // inverse the offset and subtract the width to make the width face the original x position
                outline.Width = -(offset.X + OriginTransform.width);
            }
            // if the outline is on the bottom  side.
            if (offset.Y + OriginTransform.height <= 0)
            {
                // apply the offset to the y position
                Canvas.SetTop(outline, offset.Y + OriginTransform.y + OriginTransform.height);
                // inverse the offset and subtract the height to make the width face the original y position
                outline.Height = -(offset.Y + OriginTransform.height);
            }
        }

        public void CalculateTransform()
        {
            // Create a transform tuple.
            Transform transform = new Transform(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);

            // loop through all children in the selection.
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
                // calculate the height and assign it.
                var height = shape.Height + y;

                // check if its to biggest so far.
                if (width > transform.width)
                {
                    // if so assign it
                    transform.width = width;
                }
                // check if its to biggest so far.
                if (height > transform.height)
                {
                    // if so assign it
                    transform.height = height;
                }
            }
            // calculate the width of the final transform.
            transform.width -= transform.x;
            // calculate the height of the final transform.
            transform.height -= transform.y;
            // assign the transform to the origin transform.
            OriginTransform = transform;
        }


        public void DrawOutline()
        {
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(outline, OriginTransform.x);
            Canvas.SetTop (outline, OriginTransform.y);

            // set the width and height to be the same as the selected shape.
            outline.Width  = OriginTransform.width ;
            outline.Height = OriginTransform.height;

            // Move the Resize handle it to the bottom right.
            Canvas.SetLeft(handle, Canvas.GetLeft(outline) + outline.Width  - handle.Width  / 2);
            Canvas.SetTop (handle,  Canvas.GetTop(outline) + outline.Height - handle.Height / 2);
        }

        public void ToggleOutline(bool state)
        {
            MainWindow.ins.drawCanvas.Children.Remove(outline);
            MainWindow.ins.drawCanvas.Children.Remove(handle);

            if (state)
            {
                // calculate the transform of the outline.
                CalculateTransform();
                // Draw it for the first time.
                DrawOutline();
                // update the origin pos and scale of the outline.
                outline.UpdateOriginTransform();
                // and also for the handle.
                handle.UpdateOriginTransform();
                // update the origin pos and scale of the outline.
                UpdateOriginTransform();
                // instantiate it.
                MainWindow.ins.drawCanvas.Children.Add(outline);
                MainWindow.ins.drawCanvas.Children.Add(handle);
            }
            else
            {
                // remove it.
                MainWindow.ins.drawCanvas.Children.Remove(outline);
                MainWindow.ins.drawCanvas.Children.Remove(handle);
            }
        }
    }
}

public struct Transform 
{
    public double x;
    public double y;
    public double width;
    public double height;

    public Transform(double x, double y, double width, double height)
    {
        this.x = x;
        this.y = y;

        this.width  = width;
        this.height = height;
    }
}
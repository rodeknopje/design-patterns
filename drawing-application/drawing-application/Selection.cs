﻿using drawing_application.Commands;
using drawing_application.CustomShapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace drawing_application
{

    public class Selection
    {
        Group group;

        public Rectangle outline { get; private set; }

        public Ellipse handle { get; private set; }

        private Transform transform;

        public Selection()
        {
            group = new Group();
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

        public void Move(Point offset)
        {
            // move the group handle and outline.
            group.Move  (offset);
            handle.Move (offset);
            outline.Move(offset);
        }

        public void Scale(Transform transform)
        {
            // scale the group and outline.
            group.Scale  (transform);
            outline.Scale(transform);
        }

        public void MoveHandle(Point offset)       
        {
            handle.Move(offset);
        }

        public void Merge()
        {
            var merged = new Group();

            foreach (var child in group.GetChilderen())
            {
                merged.AddChild(child);
            }

        }


        public void Select(CustomShape shape)
        {
            if (group.GetAllShapes().Contains(shape) == false)
            {
                group.AddChild(shape);
            }
        }

        public void Remove(CustomShape shape)
        {
            group.RemoveChild(shape);
        }

        public Group GetGroup()
        {
            return group;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public Transform CalculateTransform()
        {
            // Create a transform tuple.
            transform = new Transform(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);

            // loop through all childeren in the selection.
            foreach (var shape in group.GetAllShapes())
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
                if (heigth > transform.height)
                {
                    // if so assign it
                    transform.height = heigth;
                }
            }
            // calculate the width of the final transform.
            transform.width -= transform.x;
            // calculate the heigth of the final transform.
            transform.height -= transform.y;
            // return the transform
            return transform;
        }


        public void DrawOutline()
        {
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(outline, transform.x);
            Canvas.SetTop (outline, transform.y);

            // set the width and heigth to be the same as the selected shape.
            outline.Width  = transform.width ;
            outline.Height = transform.height;

            // move the resize handle it to the bottum right.
            Canvas.SetLeft(handle, Canvas.GetLeft(outline) + outline.Width  - handle.Width  / 2);
            Canvas.SetTop (handle,  Canvas.GetTop(outline) + outline.Height - handle.Height / 2);
        }

        public void ToggleOutline(bool state)
        {
            if (state)
            {
                // calculate the transform of the outline.
                CalculateTransform();
                // draw it for the first time.
                DrawOutline();
                // update the orgin pos and scale of the outlnie.
                outline.UpdateOrginPos();
                outline.UpdateOrginScale();
                // and also for the handle.
                handle.UpdateOrginPos();
                handle.UpdateOrginScale();
                // update the orgin pos and scale of the outlnie.
                group.UpdateOrginPos();
                group.UpdateOrginScale();
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
    public double height;

    public Transform(double x, double y, double width, double heigth)
    {
        this.x = x;
        this.y = y;

        this.width  = width;
        this.height = heigth;
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using System;

namespace drawing_application
{
    public partial class MainWindow : Window
    {
        // ID of each shape.
        int ID;
        // list with all the shapes in it.
        List<Shape> shapelist = new List<Shape>();
        // the selected shape style can be recktangle, circle or null.
        shapes? shape_style = shapes.rectangle;

        // the point where the mouse started when dragging.
        Point? mouse_orgin;
        // the point where the shape started when dragging.
        Point? shape_orgin;
        //
        Point? shape_scale;

        // the currently selected shape.
        Shape shape_selected;
        // the shape that is currently being drawn.
        Shape shape_drawn;

        // the rectangle you see around shapes when they are selected.
        Shape selection_outline;

        Shape handle;
        // the current state of the program.
        states state;


        public MainWindow()
        {
            InitializeComponent();

            // initialze the shape buttons.
            select_rectangle.Click += (a, b) =>
            {
                shape_style = shapes.rectangle; 
                draw_canvas.Children.Remove(selection_outline);
                draw_canvas.Children.Remove(handle);
                SwitchState(states.none);
            };
            select_ellipse.Click   += (a, b) =>
            { 
                shape_style = shapes.ellipse; 
                draw_canvas.Children.Remove(selection_outline);
                draw_canvas.Children.Remove(handle);
                SwitchState(states.none);
            };

            // initialize the clear buttons.
            select_clear.Click += (a, b) =>
            {
                draw_canvas.Children.Clear();
                selection_row.Children.Clear();
                ID = 0;
                SwitchState(states.none);
            };

            SwitchState(states.none);
        }

        private void Canvas_Mousedown(object sender, MouseButtonEventArgs e)
        {

            // check to prevent double clicks.  and if a shape style is selected.
            if (state != states.none)
            {
                return;
            }
            SwitchState(states.draw);
            // create a point variable to store coordinates
            mouse_orgin = e.GetPosition(draw_canvas);
            // create a random for the colours.
            Random r = new Random();
            // create a new shape based on the selected shape.
            shape_drawn = (Shape)Activator.CreateInstance(shape_style == shapes.rectangle ? typeof(Rectangle) : typeof(Ellipse));
            {
                shape_drawn.Width  = 0;
                shape_drawn.Height = 0;
                shape_drawn.Fill = Brushes.Transparent;
                shape_drawn.Stroke = new SolidColorBrush(Color.FromRgb((byte)r.Next(50, 200), (byte)r.Next(50, 200), (byte)r.Next(50, 200)));
                shape_drawn.StrokeThickness = 2.5;
            }

            // set the position of the shape.
            Canvas.SetLeft(shape_drawn, mouse_orgin.Value.X);
            Canvas.SetTop(shape_drawn, mouse_orgin.Value.Y);
            // add it to the canvas.
            draw_canvas.Children.Add(shape_drawn);
            // add the new shape to the shapelist.
            shapelist.Add(shape_drawn);


        }

        private void Canvas_Mousemove(object sender, MouseEventArgs e)
        {
            // if the program is drawing.
            if (state == states.draw)
            {
                // get the offset from the orgin point.
                var x_offset = e.GetPosition(draw_canvas).X - mouse_orgin.Value.X;
                var y_offset = e.GetPosition(draw_canvas).Y - mouse_orgin.Value.Y;
                // if the x offset is greater than zero.
                if (x_offset > 0)
                {
                    // set the width to the offset.
                    shape_drawn.Width = x_offset;
                }
                else
                {
                    // otherwise set the left 
                    Canvas.SetLeft(shape_drawn, x_offset + mouse_orgin.Value.X);
                    // inverse the offset to make it positive.
                    shape_drawn.Width = -x_offset;
                }
                if (y_offset > 0)
                {
                    // set the width to the offset.
                    shape_drawn.Height = y_offset;
                }
                else
                {
                    // otherwise set the left 
                    Canvas.SetTop(shape_drawn, y_offset + mouse_orgin.Value.Y);
                    // inverse the offset to make it positive.
                    shape_drawn.Height = -y_offset;
                }
            }
            // if an existing shape is being moved.
            else if (state == states.move)
            {
                // get the offset from the orgin point.
                var x_offset = e.GetPosition(draw_canvas).X - mouse_orgin.Value.X;
                var y_offset = e.GetPosition(draw_canvas).Y - mouse_orgin.Value.Y;

                // add the mouse offset to the shape offset to move the selection outline.
                Canvas.SetLeft(selection_outline, shape_orgin.Value.X + x_offset);
                Canvas.SetTop(selection_outline, shape_orgin.Value.Y + y_offset);
                // add the mouse offset to the shape offset to move the shape outline.
                Canvas.SetLeft(shape_selected, shape_orgin.Value.X + x_offset + selection_outline.StrokeThickness * 2);
                Canvas.SetTop(shape_selected, shape_orgin.Value.Y + y_offset + selection_outline.StrokeThickness * 2);

                Canvas.SetLeft(handle, Canvas.GetLeft(selection_outline) + selection_outline.Width - handle.Width / 2);
                Canvas.SetTop(handle, Canvas.GetTop(selection_outline) + selection_outline.Height - handle.Height / 2);


            }
            else if (state == states.resize)
            {
                // get the offset from the orgin point.
                var x_offset = e.GetPosition(draw_canvas).X - mouse_orgin.Value.X;
                var y_offset = e.GetPosition(draw_canvas).Y - mouse_orgin.Value.Y;

                Canvas.SetLeft(handle, shape_orgin.Value.X + x_offset);
                Canvas.SetTop(handle,  shape_orgin.Value.Y + y_offset);

                var width  = shape_scale.Value.X + x_offset;
                var heigth  = shape_scale.Value.Y + y_offset;

                if (width > 0)
                {
                    shape_selected.Width  = width;
                    selection_outline.Width  = width + selection_outline.StrokeThickness * 4;

                }
                else 
                {
                    Canvas.SetLeft(shape_selected, shape_orgin.Value.X + x_offset );
                    shape_selected.Width = -width;

                    Canvas.SetLeft(selection_outline, shape_orgin.Value.X + x_offset - selection_outline.StrokeThickness * 2);
                    selection_outline.Width = -width + selection_outline.StrokeThickness * 4;
                }
                if (heigth > 0)
                {
                    shape_selected.Height = heigth;
                    selection_outline.Height = heigth + selection_outline.StrokeThickness * 4;
                }
                else
                {
                    Canvas.SetTop(shape_selected, shape_orgin.Value.Y + y_offset );
                    shape_selected.Height = -heigth;

                    Canvas.SetTop(selection_outline, shape_orgin.Value.Y + y_offset - selection_outline.StrokeThickness * 2);
                    selection_outline.Height = -heigth + selection_outline.StrokeThickness * 4;
                }



                debug_text.Text = $"({width},{heigth})";







            }
        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            // if the taks was to draw a new shape.
            if (state == states.draw)
            {
                // add it to the selection row.
                AddToSelectionRow(shape_drawn);
                // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
                shape_drawn = null;

                SwitchState(states.none);
            }
            // if the task was to move an existing shape.
            else if (state == states.move)
            {
                SwitchState(states.select);
            }
            else if (state == states.resize)
            {
                SwitchState(states.select);

                Canvas.SetLeft(handle, Canvas.GetLeft(selection_outline) + selection_outline.Width - handle.Width / 2);
                Canvas.SetTop(handle, Canvas.GetTop(selection_outline) + selection_outline.Height - handle.Height / 2);

            }

        }

        private void AddToSelectionRow(Shape _shape)
        {
            // create a new textbox
            TextBlock textbox = new TextBlock
            {
                // assign the correct text
                Text = $"{(shape_style == shapes.rectangle ? "square" : "circle")} ({ID++})",

                Margin = new Thickness(2.5),
                FontSize = 20,
            };
            // create a new border
            Border border = new Border
            {
                BorderThickness = new Thickness(0, 0, 0, 1),

                BorderBrush = Brushes.Black,
                Background = Brushes.LightGray,
            };

            // add the border and shape to the scrollview
            selection_row.Children.Add(textbox);
            selection_row.Children.Add(border);

            // TEMP
            textbox.MouseDown += (a, b) =>
            {
                SelectShape(_shape);
                textbox.Background = Brushes.LightBlue;
            };
        }

        private void SelectShape(Shape _shape)
        {
            SwitchState(states.select);

            shape_style = null;

            shape_selected = _shape;
            // if there is already something selected
            if (selection_outline != null)
            {
                // remove the current selection outline.
                draw_canvas.Children.Remove(selection_outline);
                draw_canvas.Children.Remove(handle);
            }

            // assign the selection outline.
            selection_outline = new Rectangle
            {
                Fill = Brushes.Transparent,
                Stroke = Brushes.White,
                StrokeThickness = 2f,
                StrokeDashArray = { 5, 5 }

            };
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(selection_outline, Canvas.GetLeft(_shape) - selection_outline.StrokeThickness * 2);
            Canvas.SetTop(selection_outline, Canvas.GetTop(_shape) - selection_outline.StrokeThickness * 2);
            // set the width and heigth to be the same as the selected shape.
            selection_outline.Width =  _shape.Width + selection_outline.StrokeThickness * 4;
            selection_outline.Height = _shape.Height + selection_outline.StrokeThickness * 4;
            // add the selection outline to the draw_canvas.
            draw_canvas.Children.Add(selection_outline);


            selection_outline.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.Hand;
            selection_outline.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;

            // when the selection outline is clicked.
            selection_outline.MouseDown += (a, b) =>
            {
                SwitchState(states.move);
                // set the mouse orgin.
                mouse_orgin = b.GetPosition(draw_canvas);
                // set the shape orgin.
                shape_orgin = new Point(Canvas.GetLeft(_shape), Canvas.GetTop(_shape));
            };

            handle = new Ellipse
            {
                Width = 20,
                Height = 20,
                StrokeThickness = 2,
                Stroke = Brushes.White,
                Fill   = Brushes.Gray,
                
            };

            Canvas.SetLeft(handle, Canvas.GetLeft(selection_outline) + selection_outline.Width - handle.Width/2);
            Canvas.SetTop(handle,  Canvas.GetTop(selection_outline) + selection_outline.Height - handle.Height/2);

            draw_canvas.Children.Add(handle);

            handle.MouseDown += (a, b) =>
            {
                SwitchState(states.resize);
                // set the mouse orgin.
                mouse_orgin = b.GetPosition(draw_canvas);
                // set the shape orgin.
                shape_orgin = new Point(Canvas.GetLeft(handle), Canvas.GetTop(handle));

                shape_scale = new Point(shape_selected.Width, shape_selected.Height);

            };
        }

        private void SwitchState(states _state)
        {
            state = _state;
            debug_text.Text = $"state = {state.ToString()}";
        }
    }


    public enum shapes
    {
        rectangle,
        ellipse,
    }

    public enum states
    {
        none,
        draw,
        select,
        move,
        resize,
    }

}

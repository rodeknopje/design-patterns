using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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
        shapes shape_style = shapes.rectangle;

        // the point where the mouse started when dragging.
        Point mouse_orgin;
        // the point where the shape started when dragging.
        Point orgin_position;
        // the scale of the shape before resizing.
        Point orgin_scale;

        // the currently selected shape.
        Shape shape_selected;
        // the shape that is currently being drawn.
        Shape shape_drawn;

        // the rectangle you see around shapes when they are selected.
        Shape selection_outline;

        Shape handle;
        // the current state of the program.
        states state;

        SaveLoadManager saveload;

        public MainWindow()
        {
            InitializeComponent();

            saveload = new SaveLoadManager();

            // initialize the method for the shape buttons
            Action<shapes> click_shape = (_shape) =>            
            {
                shape_style = _shape;
                draw_canvas.Children.Remove(selection_outline);
                draw_canvas.Children.Remove(handle);
                SwitchState(states.none);
            };

            // initialze the methods to the shape buttons.
            select_rectangle.Click += (a, b) => click_shape(shapes.rectangle);         
            select_ellipse.Click   += (a, b) => click_shape(shapes.ellipse);
            

            // initialize the clear buttons.
            select_clear.Click += (a, b) =>
            {
                draw_canvas.Children.Clear();
                selection_row.Children.Clear();
                ID = 0;
                SwitchState(states.none);
                saveload.ClearFile();
            };

            SwitchState(states.none);
        }

        private void Canvas_Mousedown(object sender, MouseButtonEventArgs e)
        {

            // if the state is not none then return.
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
                shape_drawn.Stroke = new SolidColorBrush(Color.FromRgb(255, 110, 199));
                //ape_drawn.Stroke = Brushes.neo
                shape_drawn.StrokeThickness = 2.5;
            }

            // set the position of the shape.
            Canvas.SetLeft(shape_drawn, mouse_orgin.X);
            Canvas.SetTop(shape_drawn, mouse_orgin.Y);
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
                var x_offset = e.GetPosition(draw_canvas).X - mouse_orgin.X;
                var y_offset = e.GetPosition(draw_canvas).Y - mouse_orgin.Y;
                // if the x offset is greater than zero.
                if (x_offset > 0)
                {
                    // set the width to the offset.
                    shape_drawn.Width = x_offset;
                }
                else
                {
                    // otherwise set the left 
                    Canvas.SetLeft(shape_drawn, x_offset + mouse_orgin.X);
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
                    Canvas.SetTop(shape_drawn, y_offset + mouse_orgin.Y);
                    // inverse the offset to make it positive.
                    shape_drawn.Height = -y_offset;
                }
            }
            // if an existing shape is being moved.
            else if (state == states.move)
            {
                // get the offset from the orgin point.
                var x_offset = e.GetPosition(draw_canvas).X - mouse_orgin.X;
                var y_offset = e.GetPosition(draw_canvas).Y - mouse_orgin.Y;

                // add the mouse offset to the shape offset to move the selection outline.
                Canvas.SetLeft(selection_outline, orgin_position.X + x_offset);
                Canvas.SetTop(selection_outline,  orgin_position.Y + y_offset);
                // add the mouse offset to the shape offset to move the shape outline.
                Canvas.SetLeft(shape_selected, orgin_position.X + x_offset + selection_outline.StrokeThickness * 2);
                Canvas.SetTop(shape_selected,  orgin_position.Y + y_offset + selection_outline.StrokeThickness * 2);
                // adjust the position of the resize handle.
                Canvas.SetLeft(handle, Canvas.GetLeft(selection_outline) + selection_outline.Width - handle.Width  / 2);
                Canvas.SetTop(handle,  Canvas.GetTop(selection_outline) + selection_outline.Height - handle.Height / 2);


            }
            else if (state == states.resize)
            {
                // get the offset from the orginal mouse position.
                var x_offset = e.GetPosition(draw_canvas).X - mouse_orgin.X;
                var y_offset = e.GetPosition(draw_canvas).Y - mouse_orgin.Y;
                // add the offset to the handle's position.
                Canvas.SetLeft(handle, orgin_position.X + x_offset);
                Canvas.SetTop(handle,  orgin_position.Y + y_offset);
                // calculate the new width and heigth by adding the offset to the orginal scale.
                var width   = orgin_scale.X + x_offset;
                var heigth  = orgin_scale.Y + y_offset;

                // if the new width is positive. 
                if (width >= 0)
                {
                    // assign selected shape width with the new width.
                    shape_selected.Width  = width;
                    // assign selection outline width with the new width.
                    selection_outline.Width  = width + selection_outline.StrokeThickness * 4;

                }
                // if the new width is negative.
                else 
                {
                    // assign the left from the selected shape with the orignal possition minus the offset.
                    Canvas.SetLeft(shape_selected, orgin_position.X + x_offset );
                    // assign the width from the selected shape with the inversed width, so it becomes the line on the right side.
                    shape_selected.Width = -width;

                    // assign the left from the selection outline with the orignal possition minus the offset.
                    Canvas.SetLeft(selection_outline, orgin_position.X + x_offset - selection_outline.StrokeThickness * 2);
                    // assign the width from the selection outline with the inversed width, so it becomes the line on the right side.
                    selection_outline.Width = -width + selection_outline.StrokeThickness * 4;
                }
                if (heigth >= 0)
                {
                    // assign selected shape heigth with the new width.
                    shape_selected.Height = heigth;
                    // assign selection outline heigth with the new width.
                    selection_outline.Height = heigth + selection_outline.StrokeThickness * 4;
                }
                else
                {
                    // assign the top from the selected shape with the orignal possition minus the offset
                    Canvas.SetTop(shape_selected, orgin_position.Y + y_offset );
                    // assign the top from the selected shape with the inversed width, so it becomes the line on the right side.
                    shape_selected.Height = -heigth;

                    // assign the top from the selection outline with the orignal possition minus the offset.
                    Canvas.SetTop(selection_outline, orgin_position.Y + y_offset - selection_outline.StrokeThickness * 2);
                    // assign the top from the selection outline with the inversed width, so it becomes the line on the right side.
                    selection_outline.Height = -heigth + selection_outline.StrokeThickness * 4;
                }
            }
        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            // if the taks was to draw a new shape.
            if (state == states.draw)
            {
                saveload.WriteShapeToFIle($"{shape_style} {Canvas.GetLeft(shape_drawn)} {Canvas.GetTop(shape_drawn)} {shape_drawn.Width} {shape_drawn.Height}");
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

                // move the resize handle to the bottum right.
                Canvas.SetLeft(handle, Canvas.GetLeft(selection_outline) + selection_outline.Width  - handle.Width  / 2);
                Canvas.SetTop(handle,  Canvas.GetTop(selection_outline)  + selection_outline.Height - handle.Height / 2);
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
                Background  = Brushes.LightGray,
            };

            // add the border and shape to the scrollview
            selection_row.Children.Add(textbox);
            selection_row.Children.Add(border);

            // if the textbox is clicked
            textbox.MouseDown += (a, b) =>
            {
                // select the give shape.
                SelectShape(_shape);
            };
        }

        private void SelectShape(Shape _shape)
        {
            SwitchState(states.select);


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

            // give the cursor a different icon, when hovering over the outlne.
            selection_outline.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.SizeAll;
            selection_outline.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;

            // when the selection outline is clicked.
            selection_outline.MouseDown += (a, b) =>
            {
                SwitchState(states.move);
                // set the mouse orgin.
                mouse_orgin = b.GetPosition(draw_canvas);
                // set the shape orgin.
                orgin_position = new Point(Canvas.GetLeft(_shape), Canvas.GetTop(_shape));
            };

            // create the handle.
            handle = new Ellipse
            {
                Width  = 20,
                Height = 20,
                StrokeThickness = 2,
                Stroke = Brushes.White,
                Fill = Brushes.Gray,

            };

            // move it to the bottum right.
            Canvas.SetLeft(handle, Canvas.GetLeft(selection_outline) + selection_outline.Width - handle.Width/2);
            Canvas.SetTop(handle,  Canvas.GetTop(selection_outline) + selection_outline.Height - handle.Height/2);

            // add it to the draw canvas.
            draw_canvas.Children.Add(handle);

            // when the handle is clicked.
            handle.MouseDown += (a, b) =>
            {
                SwitchState(states.resize);
                // set the mouse orgin.
                mouse_orgin = b.GetPosition(draw_canvas);
                // set the shape orgin.
                orgin_position = new Point(Canvas.GetLeft(handle), Canvas.GetTop(handle));

                orgin_scale = new Point(shape_selected.Width, shape_selected.Height);

            };

            // give the cursor a different icon, when hovering over the outlne.
            handle.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.Hand;
            handle.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;

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

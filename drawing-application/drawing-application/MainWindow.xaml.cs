using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using drawing_application.Commands;

namespace drawing_application
{
    public partial class MainWindow : Window
    {
        // singleton of this class.
        public static MainWindow ins; 

        // ID of each shape.
        public int ID;
        // list with all the shapes in it.
        public List<Shape> shapelist = new List<Shape>();
        // the selected shape style can be recktangle, circle or null.
        public shapes shape_style = shapes.rectangle;

        // the point where the mouse started when dragging.
        public Point mouse_orgin;
        // the point where the shape started when dragging.
        public Point orgin_position;
        // the scale of the shape before resizing.
        public Point orgin_scale;

        // the currently selected shape.
        public Shape shape_selected;
        // the shape that is currently being drawn.
        public Shape shape_drawn;

        // the rectangle you see around shapes when they are selected.
        public Shape selection_outline;
        // the handle which you drag to resize shapes.
        public Shape handle;

        // the current state of the program.
        public states state;

        public SaveLoadManager saveload = new SaveLoadManager();

        public MainWindow()
        {
            InitializeComponent();

            // initialize the singleton.
            ins ??= this;
      
            // initialze the methods to the shape buttons.
            button_rectangle.Click += (a, b) => new ChangeShapeStyleCommand(shapes.rectangle).Execute();         
            button_ellipse.Click   += (a, b) => new ChangeShapeStyleCommand(shapes.ellipse).Execute();
            
            // initialize the clear buttons.
            button_clear.Click += (a, b) => new ClearCommand().Execute();
            // set the current state to none.
            SwitchState(states.none);
            // load the saved shapes.
            saveload.LoadProgramState();
            // call the save programs state when the application stops.
            Closed += (a, b) => saveload.SaveProgramState();
        }

        private void Canvas_Mousedown(object sender, MouseButtonEventArgs e)
        {
            //  if the game in not doing anything else.
            if (state == states.none)
            {
                // start a new drawing.
                new StartDrawCommand(e.GetPosition(draw_canvas)).Execute();
            }

        }

        private void Canvas_Mousemove(object sender, MouseEventArgs e)
        {
            // when the mouse moves do something depending on the state of the program.
            switch (state)
            {
                case states.draw:   new DrawCommand(e.GetPosition(draw_canvas)).Execute();    break;
                case states.move:   new MoveCommand(e.GetPosition(draw_canvas)).Execute();    break;
                case states.resize: new ResizeCommand(e.GetPosition(draw_canvas)).Execute();  break;
            }
        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            // when the mouse up event is fired do something depending on the state of the program.
            switch (state)
            {
                case states.draw:   new StopDrawCommand().Execute();    break;
                case states.move:   new StopMoveCommand().Execute();    break;
                case states.resize: new StopResizeCommand().Execute();  break;
            }
        }

        public void AddToSelectionRow(Shape _shape)
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

            // if the textbox is clicked then select the curren shape
            textbox.MouseDown += (a, b) => new SelectShapeCommand(_shape).Execute();
        }


        public void SwitchState(states _state)
        {
            state = _state;
            debug_text.Text = $"state:{state.ToString()}";
        }

        public Shape CreateShape(shapes style)
        {
            // create a new shape based on the selected shape.
            var shape = (Shape)System.Activator.CreateInstance(style==shapes.rectangle?typeof(Rectangle):typeof(Ellipse));
            {
                shape.Width     = 0;
                shape.Height    = 0;
                shape.Fill      = Brushes.Transparent;
                shape.Stroke    = new SolidColorBrush(Color.FromRgb(255, 110, 199));
                shape.StrokeThickness = 2.5;
            }
            return shape;
        }

        public void DeleteSelectionItems()
        {
            draw_canvas.Children.Remove(selection_outline);
            draw_canvas.Children.Remove(handle);
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

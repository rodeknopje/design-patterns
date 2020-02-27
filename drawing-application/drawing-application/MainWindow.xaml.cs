using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using drawing_application.Commands;
using System.Linq;
using System.Reflection;
using drawing_application.CustomShapes;

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
        // the selected shape style can be recktangle or circle

        // the point where the mouse started when dragging.
        public Point mouse_orgin;
        // the point where the shape started when dragging.
        public Point orgin_position;
        // the scale of the shape before resizing.
        public Point orgin_scale;

        public Point orgin_pos_handle;

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

        public CmdManager cmd_manager = new CmdManager();

        public int style_index;

        public System.Type[] styles;
        

        public MainWindow()
        {
            InitializeComponent();

            // initialize the singleton.
            ins ??= this;
            // get all types that derrive from customshape.
            styles = Assembly.GetAssembly(typeof(ShapeGroup)).GetTypes().Where(T=>T.IsSubclassOf(typeof(ShapeGroup))).ToArray();          

            // initialze the methods to the shape buttons.
            button_rectangle.Click += (a, b) => new ChangeShapeStyleCommand(0).Execute();         
            button_ellipse.Click   += (a, b) => new ChangeShapeStyleCommand(1).Execute();
            
            // initialize the clear buttons.
            button_clear.Click += (a, b) => cmd_manager.InvokeCMD(new ClearCommand());
            // set the current state to none.
            SwitchState(states.none);
            // load the saved shapes.
            saveload.LoadProgramState();
            // call the save programs state when the application stops.
            Closed += (a, b) => saveload.SaveProgramState();

            KeyDown += (a, b) => 
            {
                if (b.Key == Key.Z) if (Keyboard.IsKeyDown(Key.LeftCtrl)) cmd_manager.Undo();
                if (b.Key == Key.R) if (Keyboard.IsKeyDown(Key.LeftCtrl)) cmd_manager.Redo();
            };

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
                case states.draw:   new DrawCommand  (e.GetPosition(draw_canvas)).Execute(); break;
                case states.move:   new MoveCommand  (e.GetPosition(draw_canvas)).Execute(); break;
                case states.resize: new ResizeCommand(e.GetPosition(draw_canvas)).Execute(); break;
            }
        }

        private void Canvas_Mouseup(object sender, MouseButtonEventArgs e)
        {
            // when the mouse up event is fired do something depending on the state of the program.
            switch (state)
            {
                case states.draw:   cmd_manager.InvokeCMD(new StopDrawCommand  ()); break;
                case states.move:   cmd_manager.InvokeCMD(new StopMoveCommand  ()); break;
                case states.resize: cmd_manager.InvokeCMD(new StopResizeCommand()); break;
            }
        }

        public Button AddToSelectionRow(Shape _shape)
        {
            // create a new textbox
            Button button = new Button
            {
                // assign the correct text
                Content = $"{_shape.GetType().Name} ({ID++})",
                Margin = new Thickness(1),
                FontSize = 20,
            };
            // if the textbox is clicked then select the curren shape
            button.Click += (a, b) => new SelectShapeCommand(_shape).Execute();

            return button;
        }

        public void SwitchState(states state)
        {
            this.state = state;

            debug_text.Text = $"state:{this.state.ToString()}";
        }

        public Shape CreateShape(int index)
        {
            // create a new shape based on the selected shape.
            var shape = (Shape)System.Activator.CreateInstance(styles[index]);
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

    public enum states
    {
        none,
        draw,
        select,
        move,
        resize,
    }

}

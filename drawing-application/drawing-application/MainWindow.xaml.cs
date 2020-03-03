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
        public Point orgin_mouse;
        // the point where the shape started when dragging.
        public Point orgin_position;
        // the scale of the shape before resizing.
        public Point orgin_scale;

        public Point orgin_pos_handle;

        // the currently selected shape.
        public CustomShape shape_selected;
        // the shape that is currently being drawn.
        public CustomShape shape_drawn;

        // the rectangle you see around shapes when they are selected.
        public Shape selection_outline;
        // the handle which you drag to resize shapes.
        public Shape handle;

        // the current state of the program.
        public states state;

        public SaveLoadManager saveload = new SaveLoadManager();
       
        public CmdManager cmd_manager = new CmdManager();

        // all types that derrive from ShapeGroup.
        public System.Type[] styles;
        // the current index of the styles array.
        public int style_index;
        

        public MainWindow()
        {
            InitializeComponent();

            // initialize the singleton.
            ins ??= this;
            // get all types that derrive from customshape.
            styles = Assembly.GetAssembly(typeof(CustomShape)).GetTypes().Where(T=>T.IsSubclassOf(typeof(CustomShape))&& T != typeof(Group)).ToArray();

            // make te style button a toggle for the shape buttons.
            button_style.Click += (a, b) => style_select.Visibility = style_select.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;                     
            // initialize the clear button.
            button_clear.Click += (a, b) => cmd_manager.InvokeCMD(new ClearCommand());
            // set the current state to none.
            SwitchState(states.none);
            // load the saved shapes.
            saveload.LoadProgramState();
            // call the save programs state when the application stops.
            Closed += (a, b) => saveload.SaveProgramState();
            // Initialize the buttons
            InitializeStyleButtons();

            // bind the undo and redo actions to their conrresponsing buttons
            button_undo.Click += (a, b) => cmd_manager.Undo();
            button_redo.Click += (a, b) => cmd_manager.Redo();
            // als bind it to the cntrl+z and contrl+r keys
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

        public Button CreateSelectButton(CustomShape _shape)
        {
            // create a new textbox
            Button button = new Button
            {
                // assign the correct text
                Content = $"{_shape.GetType().Name} ({ID++})",
                Margin = new Thickness(1),
                HorizontalContentAlignment = HorizontalAlignment.Left,
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

        public CustomShape CreateShape(int index)
        {
            // create a new shape based on the selected shape.
            var shape = (CustomShape)System.Activator.CreateInstance(styles[index]);
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

        private void InitializeStyleButtons()
        {
            // for all different styles.
            for (int i = 0; i < styles.Length; i++)
            {
                // define the index to which the button should switch.
                int index = i;
                // initialize the buttons.
                var button = new Button
                {
                    
                    Content = styles[index].Name,
                    Height = 30,
                    BorderThickness = new Thickness(1,0,1,1)
                    
                };

                // when the button is clicked
                button.Click += (a, b) =>
                {
                    // make the style button display the current style.
                    button_style.Content = styles[index].Name;
                    // collapse the syle select element.
                    style_select.Visibility = Visibility.Collapsed;
                    // switch the style.
                    new ChangeShapeStyleCommand(index).Execute();
                };               
                // add this button to the style select stackpanel.
                style_select.Children.Add(button);
            }
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

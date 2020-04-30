using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using drawing_application.Commands;
using System.Linq;
using System.Reflection;
using drawing_application.CustomShapes;


namespace drawing_application
{
    public partial class MainWindow
    {
        // singleton of this class.
        public static MainWindow ins;
        // the point where the mouse started when dragging.
        public Point mouseOrigin;
        // the shape that is currently being drawn.
        public CustomShape shapeDrawn;
        // the current state of the program.
        public States state;
        // the save load manager which can save and load the program state.
        public SaveLoadManager saveLoad = new SaveLoadManager();
        // command manager which can undo and redo commands.
        public CommandManager commandManager = new CommandManager();
        // all types that derive from custom shape.
        public System.Type[] styles;
        // the current index of the styles array.
        public int styleIndex;


        public MainWindow()
        {
            InitializeComponent();
            // initialize the singleton.
            ins ??= this;
            // get all types that derive from custom shape.
            styles = Assembly.GetAssembly(typeof(CustomShape)).GetTypes().Where(T=>T.IsSubclassOf(typeof(CustomShape))).ToArray();
            // make te style button a toggle for the shape buttons.
            buttonStyle.Click += (a, b) => stylesDisplay.Visibility = stylesDisplay.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;                     
            // initialize the clear button.
            buttonClear.Click += (a, b) => commandManager.InvokeCommand(new ClearCommand());
            // set the current newState to None.
            SwitchState(States.None);

            Hierarchy.GetInstance().SetStackPanel(selectionDisplay);
            // load the saved shapes.
            //saveLoad.LoadProgramState();
            // call the save programs newState when the application stops.
            Closed += (a, b) => saveLoad.SaveProgramState();
            // Initialize the buttons
            InitializeStyleButtons();
            // bind the undo and redo actions to their corresponding  buttons
            buttonUndo.Click += (a, b) => commandManager.Undo();
            buttonRedo.Click += (a, b) => commandManager.Redo();
            // als bind it to the control+z and contrl+r keys
            KeyDown += (a, b) => 
            {
                switch (b.Key)
                {
                    case Key.Z: if (Keyboard.IsKeyDown(Key.LeftCtrl)) commandManager.Undo(); break;
                    case Key.R: if (Keyboard.IsKeyDown(Key.LeftCtrl)) commandManager.Redo(); break;

                    case Key.M: commandManager.InvokeCommand(new SwitchGroupCommand(Hierarchy.GetInstance().GetTopGroup()));  break;
                    
                    case Key.J:

                        if (Keyboard.IsKeyDown(Key.LeftCtrl) && Selection.GetInstance().GetChildren().Count > 0)
                        {
                            commandManager.InvokeCommand(new MergeCommand());
                        }
                        break;
                }
            };
            // go to back to Draw newState when rmb is pressed.
            MouseRightButtonDown += (a, b) => 
            {
                if (state == States.Select)
                {
                    new ChangeShapeStyleCommand(styleIndex).Execute(); 

                    Selection.GetInstance().Clear();

                    Hierarchy.GetInstance().DeselectAllButtons();
                }
            };
            // click the first shape button
            ((Button)stylesDisplay.Children[0]).RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
        }

        private void CanvasLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //  if the game in not doing anything else.
            if (state == States.None)
            {
                // start a new drawing.
                new StartDrawCommand(e.GetPosition(drawCanvas)).Execute();
            }
        }

        private void CanvasMouseDown(object sender, MouseEventArgs e)
        {
            // when the mouse moves do something depending on the newState of the program.
            switch (state)
            {
                case States.Move:   new MoveCommand  (e.GetPosition(drawCanvas)).Execute(); break;
                case States.Draw:   new DrawCommand  (e.GetPosition(drawCanvas)).Execute(); break;
                case States.Resize: new ResizeCommand(e.GetPosition(drawCanvas)).Execute(); break;
            }
        }

        private void CanvasLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // when the mouse up event is fired do something depending on the newState of the program.
            switch (state)
            {
                case States.Move:   commandManager.InvokeCommand(new StopMoveCommand  (e.GetPosition(drawCanvas))); break;
                case States.Draw:   commandManager.InvokeCommand(new StopDrawCommand  ()); break;
                case States.Resize: commandManager.InvokeCommand(new StopResizeCommand()); break;
            }
        }




        public void SwitchState(States newState)
        {
            // set the state to the new state.
            state = newState;
            // update the debug text
            debugText.Text = $"state:{state.ToString()}";
        }

        public CustomShape CreateShape(int index)
        {
            // create a new shape based on the selected shape.
            return (CustomShape)System.Activator.CreateInstance(styles[index]);
        }

        private void InitializeStyleButtons()
        {
            // for all different styles.
            for (var i = 0; i < styles.Length; i++)
            {
                // skip if the style is a group, since we don't want to Draw groups directly
                if (styles[i].IsSubclassOf(typeof(Group)) || styles[i] == typeof(Group))
                {
                    continue;
                }

                // define the index to which the button should switch.
                var index = i;
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
                    buttonStyle.Content = styles[index].Name;
                    // collapse the style Select element.
                    stylesDisplay.Visibility = Visibility.Collapsed;
                    // switch the style.
                    new ChangeShapeStyleCommand(index).Execute();
                };               
                // add this button to the style Select stack panel.
                stylesDisplay.Children.Add(button);
            }
        }
    }

    public enum States
    {
        None,
        Draw,
        Select,
        Move,
        Resize,
    }
}

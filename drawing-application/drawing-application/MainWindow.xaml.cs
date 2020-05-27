
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using drawing_application.Commands;
using drawing_application.Buttons;
using drawing_application.CustomShapes;


namespace drawing_application
{
    public partial class MainWindow
    {
        // singleton of this class.
        public static MainWindow ins;
        // the current state of the program.
        private States state;
        // the current index of the styles array.
        public int styleIndex;
        // the point where the mouse started when dragging.
        public Point mouseOrigin;
        // the shape that is currently being drawn.
        public CustomShape shapeDrawn;
        


        public MainWindow()
        {
            InitializeComponent();
            // initialize the singleton.
            ins ??= this;
            // make te style button a toggle for the shape buttons.
            buttonStyle.Click += (a, b) => stylesDisplay.Visibility = stylesDisplay.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;                     
            // initialize the clear button.
            buttonClear.Click += (a, b) => CommandManager.GetInstance().InvokeCommand(new ClearCommand());
            // set the current newState to None.
            SwitchState(States.None);
            // assign the stack panel from the hierarchy.
            Hierarchy.GetInstance().SetStackPanel(selectionDisplay);
            // Initialize the buttons
            InitializeStyleButtons();
            // bind the undo and redo actions to their corresponding  buttons
            buttonUndo.Click += (a, b) => CommandManager.GetInstance().Undo();
            buttonRedo.Click += (a, b) => CommandManager.GetInstance().Redo();
            // als bind it to the control+z and contrl+r keys
            KeyDown += (a, b) => 
            {
                switch (b.Key)
                {
                    case Key.Z: if (Keyboard.IsKeyDown(Key.LeftCtrl)) CommandManager.GetInstance().Undo(); break;
                    case Key.R: if (Keyboard.IsKeyDown(Key.LeftCtrl)) CommandManager.GetInstance().Redo(); break;

                    case Key.M: CommandManager.GetInstance().InvokeCommand(new SwitchGroupCommand(Hierarchy.GetInstance().GetTopGroup()));  break;
                    
                    case Key.J:

                        if (Keyboard.IsKeyDown(Key.LeftCtrl) && Selection.GetInstance().GetChildren().Count > 0)
                        {
                            CommandManager.GetInstance().InvokeCommand(new MergeCommand());
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

        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //  if the game in not doing anything else.
            if (state == States.None)
            {
                // start a new drawing.
                new StartDrawCommand(e.GetPosition(drawCanvas)).Execute();
            }
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            // when the mouse moves do something depending on the newState of the program.
            switch (state)
            {
                case States.Move:   new MoveCommand  (e.GetPosition(drawCanvas)).Execute(); break;
                case States.Draw:   new DrawCommand  (e.GetPosition(drawCanvas)).Execute(); break;
                case States.Resize: new ResizeCommand(e.GetPosition(drawCanvas)).Execute(); break;
            }
        }

        private new void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // when the mouse up event is fired do something depending on the newState of the program.
            switch (state)
            {
                case States.Move:   CommandManager.GetInstance().InvokeCommand(new StopMoveCommand  (e.GetPosition(drawCanvas))); break;
                case States.Draw:   CommandManager.GetInstance().InvokeCommand(new StopDrawCommand  ()); break;
                case States.Resize: CommandManager.GetInstance().InvokeCommand(new StopResizeCommand()); break;
            }
        }

        public void SwitchState(States newState)
        {
            // set the state to the new state.
            state = newState;
            // update the debug text
            debugText.Text = $"state: {state.ToString()}";
        }

        private void InitializeStyleButtons()
        {
            // get all the the custom shape types.
            var styles = Utility.GetInstance().GetShapeTypes();
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


}

using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using drawing_application.Commands;
using Point = System.Drawing.Point;

namespace drawing_application
{
    public class Controller
    {
        private readonly Hierarchy hierarchy;

        private readonly Canvas drawCanvas;

        private readonly SaveLoadManager saveLoadManager;

        private readonly CommandManager commandManager;

        private readonly StackPanel stylePanel;

        private Point mouseOrigin;

        private States state;


        public Controller(StackPanel buttonPanel, StackPanel stylePanel, Canvas drawCanvas, Button clearButton, Button styleButton , Button redoButton, Button undoButton, Action onClose, Action<Key> onKeyDown)
        {
            this.stylePanel = stylePanel;

            this.drawCanvas = drawCanvas;




            InitializeShapeStylePanel(styleButton);

            InitializeShortcutActions(onKeyDown);

            InitializeCanvasActions();
        }



        private void InitializeCanvasActions()
        {

            drawCanvas.MouseLeftButtonDown += (a, b) =>
            {
                //  if the game in not doing anything else.
                if (state == States.None)
                {
                    // start a new drawing.
                    new StartDrawCommand(b.GetPosition(drawCanvas)).Execute();
                }
            };

            drawCanvas.MouseMove += (a, b) =>
            {
                // when the mouse moves do something depending on the newState of the program.
                switch (state)
                {
                    case States.Move:   new MoveCommand  (b.GetPosition(drawCanvas)).Execute(); break;
                    case States.Draw:   new DrawCommand  (b.GetPosition(drawCanvas)).Execute(); break;
                    case States.Resize: new ResizeCommand(b.GetPosition(drawCanvas)).Execute(); break;
                }
            };

            drawCanvas.MouseLeftButtonUp += (a, b) =>
            {
                // when the mouse up event is fired do something depending on the newState of the program.
                switch (state)
                {
                    case States.Move:   commandManager.InvokeCommand(new StopMoveCommand  (b.GetPosition(drawCanvas))); break;
                    case States.Draw:   commandManager.InvokeCommand(new StopDrawCommand  ()); break;
                    case States.Resize: commandManager.InvokeCommand(new StopResizeCommand()); break;
                }
            };


        }

        private void InitializeShortcutActions(Action<Key> onKeyDown)
        {
            onKeyDown += (key) =>
            {
                switch (key)
                {
                    case Key.Z: if 
                        (Keyboard.IsKeyDown(Key.LeftCtrl)) commandManager.Undo(); break;
                    case Key.R: if 
                        (Keyboard.IsKeyDown(Key.LeftCtrl)) commandManager.Redo(); break;
                    case Key.M: 
                        commandManager.InvokeCommand(new SwitchGroupCommand(Hierarchy.GetInstance().GetTopGroup())); break;
                    case Key.J:
                        if (Keyboard.IsKeyDown(Key.LeftCtrl) && Selection.GetInstance().GetChildren().Count > 0)
                        {
                            commandManager.InvokeCommand(new MergeCommand());
                        }
                        break;
                }
            };





        }

        private void InitializeShapeStylePanel(Button styleButton)
        {
            // make te style button a toggle for the shape buttons.
            styleButton.Click += (a, b) => stylePanel.Visibility = stylePanel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
           

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
                    // set the text on the button to be the name of the class.
                    Content = styles[index].Name,
                    // set the height.
                    Height = 30,
                    // set the border thickness.
                    BorderThickness = new Thickness(1, 0, 1, 1)
                };

                // when the button is clicked
                button.Click += (a, b) =>
                {
                    // make the style button display the current style.
                    styleButton.Content = styles[index].Name;
                    // collapse the style Select element.
                    stylePanel.Visibility = Visibility.Collapsed;
                    // switch the style.
                    new ChangeShapeStyleCommand(index).Execute();
                };
                // add this button to the style Select stack panel.
                stylePanel.Children.Add(button);
            }

            // click the first shape button
            ((Button)stylePanel.Children[0]).RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
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

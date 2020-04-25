using drawing_application.CustomShapes;
using System.Windows.Controls;
using System.Collections.Generic;

namespace drawing_application.Commands
{
    public class StopDrawCommand : Command
    {
        // the shape that which is drawn
        private readonly CustomShape shape;
        // the button corresponding to the shape.
        private readonly SelectButton button;

        public StopDrawCommand() 
        {
            // assign the shape of this command with the drawn shape.
            shape = m.shapeDrawn;
            // remove the drawn_shape from the canvas.
            m.drawCanvas.Children.Remove(shape);
            // create a button based on this shape.
            button = m.CreateSelectButton(shape);
        }

        // alternate constructor for loading shapes from the save file.
        public StopDrawCommand(int index, IReadOnlyList<int> posData)
        {                      
            // create a new shape based on the index.
            shape = m.CreateShape(index);
            // set the position of the shape based on the given data.
            Canvas.SetLeft(shape, posData[0]);
            Canvas.SetTop (shape, posData[1]);
            // set the dimensions of the shape based on the given data.
            shape.Width  = posData[2];
            shape.Height = posData[3];
            // create a button based on this shape.
            button = m.CreateSelectButton(shape);
        }

        public override void Execute()
        {
            // add the shape to the canvas.
            m.drawCanvas.Children.Add(shape);
            // add the button to the selection row.
            m.selectionDisplay.Children.Add(button);
            // switch to the None state.
            m.SwitchState(States.None);
        }

        public override void Undo()
        {
            // disable the selection outline.
            Selection.GetInstance().ToggleOutline(false);
            // remove the shape from the canvas.
            m.drawCanvas.Children.Remove(shape);
            // remove the button from the selection row.
            m.selectionDisplay.Children.Remove(button);
            // switch to the None state.
            m.SwitchState(States.None);

        }
    }
}

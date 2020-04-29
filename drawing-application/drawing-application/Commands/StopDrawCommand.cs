using drawing_application.CustomShapes;
using System.Windows.Controls;
using System.Collections.Generic;

namespace drawing_application.Commands
{
    public class StopDrawCommand : Command
    {
        // the minimal size a shape needs to be.
        private const float MinSize = 1;
        // the shape that which is drawn
        private readonly CustomShape shape;
        // the button corresponding to the shape.
        //private readonly ShapeButton button;

        public StopDrawCommand() 
        {
            // assign the shape of this command with the drawn shape.
            shape = m.shapeDrawn;
            // remove the drawn_shape from the canvas.
            m.drawCanvas.Children.Remove(shape);
            // if any dimension is lower than the min size the it to the min size.
            shape.Width  = shape.Width  < MinSize ? MinSize : shape.Width;
            shape.Height = shape.Height < MinSize ? MinSize : shape.Height;

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
        }

        public override void Execute()
        {
            // add the button to the selection row.
            Hierarchy.GetInstance().AddToHierarchy(shape);
            // switch to the None state.
            m.SwitchState(States.None);
        }

        public override void Undo()
        {
            // disable the selection outline.
            Selection.GetInstance().ToggleOutline(false);
            // remove the shapes from the hierarchy.
            Hierarchy.GetInstance().RemoveFromHierarchy(shape);
            // switch to the None state.
            m.SwitchState(States.None);
        }
    }
}

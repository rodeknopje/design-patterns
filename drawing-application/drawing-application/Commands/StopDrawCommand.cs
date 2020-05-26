using drawing_application.CustomShapes;

namespace drawing_application.Commands
{
    public class StopDrawCommand : Command
    {
        // the minimal size a shape needs to be.
        private const float MinSize = 1;
        // the shape that which is drawn
        private readonly CustomShape shape;

        public StopDrawCommand() 
        {
            // assign the shape of this command with the drawn shape.
            shape = m.shapeDrawn;
            // remove the drawn_shape from the canvas.
            //m.drawCanvas.Children.Remove(shape);
            shape.SetActive(false);
            // if any dimension is lower than the min size the it to the min size.
            shape.Width  = shape.Width  < MinSize ? MinSize : shape.Width;
            shape.Height = shape.Height < MinSize ? MinSize : shape.Height;

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

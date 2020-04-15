using drawing_application.CustomShapes;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace drawing_application.Commands
{
    class SelectShapeCommand : Command
    {
        CustomShape shape;

        public SelectShapeCommand(CustomShape shape)
        {
            this.shape = shape;
        }


        public override void Execute()
        {
            // switch to select state.
            m.SwitchState(states.select);
            // deselect the shape.
            m.selection.ToggleOutline(false);
            // assign the selected shape.
            m.selection.Select(shape);

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

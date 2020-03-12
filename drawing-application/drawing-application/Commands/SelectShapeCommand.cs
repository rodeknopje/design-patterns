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
            m.selection.ToggleOutline(false);
            // switch to select state.
            m.SwitchState(states.select);
            // assign the selected shape.
            m.selection.Add(shape);
            // toggle the outline on.
            m.selection.ToggleOutline(true);
            // draw the outline
            m.selection.DrawOutline();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

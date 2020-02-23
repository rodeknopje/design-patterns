using System;

namespace drawing_application.Commands
{
    class StopDrawCommand : Command
    {
        public override void Execute()
        {
            // add it to the selection row.
            m.AddToSelectionRow(m.shape_drawn);
            // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
            m.shape_drawn = null;

            m.SwitchState(states.none);
        }

        public override void Undo()
        {

        }
    }
}

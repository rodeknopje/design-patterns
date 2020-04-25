using System;
using System.Windows;

namespace drawing_application.Commands
{
    public class StartResizeCommand : Command
    {
        // the mouse position when the Resize started.
        private readonly Point mousePos;

        public StartResizeCommand(Point mousePos)
        {
            // assign the mouse pos.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // switch to the Resize state.
            M.SwitchState(States.Resize);
            // set the mouse origin.
            M.mouseOrigin = mousePos;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

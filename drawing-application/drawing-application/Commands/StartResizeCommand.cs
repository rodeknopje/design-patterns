using System;
using System.Windows;

namespace drawing_application.Commands
{
    public class StartResizeCommand : Command
    {
        // the mouse position when the resize started.
        private readonly Point mousePos;

        public StartResizeCommand(Point mousePos)
        {
            // assign the mouse pos.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // switch to the resize state.
            M.SwitchState(states.resize);
            // set the mouse origin.
            M.orgin_mouse = mousePos;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

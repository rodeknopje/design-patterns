using System;
using System.Windows;

namespace drawing_application.Commands
{
    public class StartMoveCommand : Command
    {
        // the current mouse position.
        private readonly Point mousePos;

        public StartMoveCommand(Point mousePos)
        {
            // assign the mouse position.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // switch to the Move state.
            m.SwitchState(States.Move);
            // set the mouse origin.
            m.mouseOrigin = mousePos;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace drawing_application.Commands
{
    class StopMoveCommand : Command
    {
        public override void Execute()
        {
            m.state = states.none;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

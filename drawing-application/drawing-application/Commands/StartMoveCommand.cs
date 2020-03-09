using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StartMoveCommand : Command
    {
        Point mouse_pos;

        public StartMoveCommand(Point mouse_pos)
        {
            this.mouse_pos = mouse_pos;
        }

        public override void Execute()
        {
            m.SwitchState(states.move);
            // set the mouse orgin.
            m.orgin_mouse = mouse_pos;
            // set the shape orgin.
            m.shape_selected.UpdateOrginPos();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

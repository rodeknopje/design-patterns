using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
            m.mouse_orgin = mouse_pos;
            // set the shape orgin.
            m.orgin_position = new Point(Canvas.GetLeft(m.shape_selected), Canvas.GetTop(m.shape_selected));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

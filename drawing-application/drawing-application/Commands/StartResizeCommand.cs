using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StartResizeCommand : Command
    {
        Point mouse_pos;

        public StartResizeCommand(Point mouse_pos)
        {
            this.mouse_pos = mouse_pos;
        }

        public override void Execute()
        {
            m.SwitchState(states.resize);
            // set the mouse orgin.
            m.mouse_orgin = mouse_pos;
            // set the handle orgin.
            m.orgin_pos_handle = new Point(Canvas.GetLeft(m.handle), Canvas.GetTop(m.handle));
            // save the shape orgin.
            m.orgin_position = new Point(Canvas.GetLeft(m.shape_selected), Canvas.GetTop(m.shape_selected));
            // set the scale orgin.
            m.orgin_scale = new Point(m.shape_selected.Width, m.shape_selected.Height);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

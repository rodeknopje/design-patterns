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
            m.orgin_mouse = mouse_pos;
            // set the handle orgin.
            m.orgin_pos_handle = new Point(Canvas.GetLeft(m.handle), Canvas.GetTop(m.handle));
            // save the position orgin.
            m.shape_selected.UpdateOrginPos();
            // set the scale orgin.
            m.shape_selected.UpdateOrginScale();

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

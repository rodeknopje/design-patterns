using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopResizeCommand : Command
    {
        public override void Execute()
        {
            m.SwitchState(states.select);
            // move the resize handle to the bottum right.
            Canvas.SetLeft(m.handle, Canvas.GetLeft(m.selection_outline) + m.selection_outline.Width - m.handle.Width / 2);
            Canvas.SetTop(m.handle, Canvas.GetTop(m.selection_outline) + m.selection_outline.Height - m.handle.Height / 2);
        }

        public override void Undo()
        {

        }
    }
}

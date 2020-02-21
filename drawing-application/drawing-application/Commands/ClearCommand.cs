using System;

namespace drawing_application.Commands
{
    class ClearCommand : Command
    {
        public override void Execute()
        {
            m.draw_canvas.Children.Clear();
            m.selection_row.Children.Clear();
            m.ID = 0;
            m.SwitchState(states.none);
            m.saveload.ClearFile();
        }

        public override void Undo()
        {
            
        }
    }
}

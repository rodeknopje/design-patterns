using System;

namespace drawing_application.Commands
{
    class ClearCommand : Command
    {
        public override void Execute()
        {   
            // remove all shapes.
            m.draw_canvas.Children.Clear();
            // remove all buttons in the selection row.
            m.selection_row.Children.Clear();
            // set the id to zero.
            m.ID = 0;
            // program state is none.
            m.SwitchState(states.none);
            // clear the save file.
            m.saveload.ClearFile();
        }

        public override void Undo()
        {
            
        }
    }
}

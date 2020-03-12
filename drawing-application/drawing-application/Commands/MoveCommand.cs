using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class MoveCommand : Command
    {
        Point mouse_pos;

        public MoveCommand(Point mouse_pos)
        {
            this.mouse_pos = mouse_pos;
        }

        public override void Execute()
        {
            // calculate the mouse offset
            var x_offset = mouse_pos.X - m.orgin_mouse.X;
            var y_offset = mouse_pos.Y - m.orgin_mouse.Y;

            // move the shape based on the offset.
            m.selection.Move(new Point(x_offset, y_offset));
            // draw the outline.
            // m.selection.DrawOutline();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

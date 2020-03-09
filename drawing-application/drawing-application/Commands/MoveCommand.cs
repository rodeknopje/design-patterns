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
            m.shape_selected.Move(new Point(x_offset, y_offset));

            // add the mouse offset to the shape offset to move the selection outline.
            Canvas.SetLeft(m.selection_outline, Canvas.GetLeft(m.shape_selected) - m.selection_outline.StrokeThickness * 2);
            Canvas.SetTop(m.selection_outline , Canvas.GetTop(m.shape_selected)  - m.selection_outline.StrokeThickness * 2);

            // adjust the position of the resize handle.
            Canvas.SetLeft(m.handle, Canvas.GetLeft(m.selection_outline) + m.selection_outline.Width - m.handle.Width / 2);
            Canvas.SetTop (m.handle, Canvas.GetTop(m.selection_outline) + m.selection_outline.Height - m.handle.Height / 2);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

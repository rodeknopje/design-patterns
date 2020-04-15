
using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopMoveCommand : Command
    {
        // the shape binded to this command.
        List<CustomShape>  shapes;

        Point offset;

        public StopMoveCommand(Point mouse_pos)
        {
            // assign the shape.
            shapes = m.selection.GetAllShapes();
            // calculate the mouse offset
            var x_offset = mouse_pos.X - m.orgin_mouse.X;
            var y_offset = mouse_pos.Y - m.orgin_mouse.Y;
            // assign the offset.
            offset = new Point(x_offset, y_offset);

            foreach (var shape in shapes)
            {
                // set the shape to their orgin pos.
                Canvas.SetLeft(shape, shape.orginTransform.x);
                Canvas.SetTop (shape, shape.orginTransform.y);
            }

        }

        public override void Execute()
        {
            foreach (var shape in shapes)
            {
                // set the shape to his orginal position.
                shape.Move(offset);
                // update their orgin position
                shape.UpdateOrginTransform();
            }
            // toggle the outline off.
            m.selection.ToggleOutline(false);
            // switch to select state.
            m.SwitchState(states.select);
        }

        public override void Undo()
        {
            foreach (var shape in shapes)
            {
                // set the shape to his orginal position.
                shape.Move(new Point(-offset.X, -offset.Y));
                // update their orgin position
                shape.UpdateOrginTransform();
            }
            // toggle the outlnie off.
            m.selection.ToggleOutline(false);
            // switch to select state.
            m.SwitchState(states.select);
        }
    }
}

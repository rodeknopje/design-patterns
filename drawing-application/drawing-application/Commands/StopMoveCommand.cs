
using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class StopMoveCommand : Command
    {
        // the shape bounded to this command.
        private readonly List<CustomShape> shapes;
        // offset of the mouse movement.
        private readonly Point offset;

        public StopMoveCommand(Point mouse_pos)
        {
            // assign the shape.
            shapes = m.selection.GetAllShapes();
            
            // assign the offset
            offset = new Point
            {
                // calculate the mouse offset
                X = mouse_pos.X - m.orgin_mouse.X,
                Y = mouse_pos.Y - m.orgin_mouse.Y,
            };

            foreach (var shape in shapes)
            {
                // set the shape to their origin pos.
                Canvas.SetLeft(shape, shape.OriginTransform.x);
                Canvas.SetTop (shape, shape.OriginTransform.y);
            }

        }

        public override void Execute()
        {
            foreach (var shape in shapes)
            {
                // set the shape to his original position.
                shape.Move(offset);
                // update their origin position
                shape.UpdateOriginTransform();
            }
            // Deselect all the shapes, because we can only select non selected shapes.
            shapes.ForEach(m.selection.RemoveChild);
            // select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }

        public override void Undo()
        {
            foreach (var shape in shapes)
            {
                // set the shape to his original position.
                shape.Move(new Point(-offset.X, -offset.Y));
                // update their origin position
                shape.UpdateOriginTransform();
            }
            // Deselect all the shapes, because we can only select non selected shapes.
            shapes.ForEach(m.selection.RemoveChild);
            // select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }
    }
}

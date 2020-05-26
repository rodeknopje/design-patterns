
using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Windows;
using drawing_application.Visitors;

namespace drawing_application.Commands
{
    public class StopMoveCommand : Command
    {
        // the shapes bounded to this command.
        private readonly List<CustomShape> shapes = new List<CustomShape>();
        // offset of the mouse movement.
        private readonly Point offset;

        public StopMoveCommand(Point mouse_pos)
        {
            // add the currently selected children to the shapes list.
            Selection.GetInstance().GetChildren().ForEach(shapes.Add);

            // assign the offset
            offset = new Point
            {
                // calculate the mouse offset
                X = mouse_pos.X - m.mouseOrigin.X,
                Y = mouse_pos.Y - m.mouseOrigin.Y,
            };

        }

        public override void Execute()
        {
            var moveVisitor = new MoveVisitor(offset);

            foreach (var shape in shapes)
            {
                // set the shape to his original position.
                //shape.Move(offset);
                //// update their origin position
                //shape.UpdateOriginTransform();

                shape.Accept(moveVisitor);
            }
            // Deselect all the shapes, because we can only Select non selected shapes.
            shapes.ForEach(Selection.GetInstance().RemoveChild);
            // Select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }

        public override void Undo()
        {
            var moveVisitor = new MoveVisitor(new Point(-offset.X, -offset.Y));

            foreach (var shape in shapes)
            {
                // set the shape to his original position.
                //shape.Move(new Point(-offset.X, -offset.Y));
                //// update their origin position
                //shape.UpdateOriginTransform();

                shape.Accept(moveVisitor);
            }
            // Deselect all the shapes, because we can only Select non selected shapes.
            shapes.ForEach(Selection.GetInstance().RemoveChild);
            
            // Select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }
    }
}

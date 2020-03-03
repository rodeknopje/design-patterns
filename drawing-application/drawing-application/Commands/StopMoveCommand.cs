
using drawing_application.CustomShapes;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopMoveCommand : Command
    {
        // the shape binded to this command.
        CustomShape shape;
        // the orginal pos of this shape.
        Point orgin_pos;

        Point new_pos;

        public StopMoveCommand()
        {
            // assign the shape.
            shape = m.shape_selected;
            // assign the orgin pos.
            orgin_pos = m.orgin_position;
            // assign the new pos.
            new_pos = new Point(Canvas.GetLeft(shape),Canvas.GetTop(shape));
        }

        public override void Execute()
        {
            // set the shape to his orginal position.
            Canvas.SetLeft(shape, new_pos.X);
            Canvas.SetTop (shape, new_pos.Y);

            new SelectShapeCommand(shape).Execute();
        }

        public override void Undo()
        {
            // set the shape to his orginal position.
            Canvas.SetLeft(shape, orgin_pos.X);
            Canvas.SetTop (shape, orgin_pos.Y);
            // select it.
            new SelectShapeCommand(shape).Execute();
        }
    }
}

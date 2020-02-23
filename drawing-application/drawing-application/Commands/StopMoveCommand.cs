﻿
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class StopMoveCommand : Command
    {
        // the shape binded to this command.
        Shape shape;
        // the orginal pos of this shape.
        Point orgin_pos;

        public StopMoveCommand()
        {
            // assign the shape.
            shape = m.shape_selected;
            // assign the orgin pos.
            orgin_pos = m.orgin_position;
        }

        public override void Execute()
        {
            m.SwitchState(states.select);
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

﻿
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
            shape = m.selection.shape;
            // assign the orgin pos.
            orgin_pos = m.selection.shape.orginPos;
            // assign the new pos.
            new_pos = new Point(Canvas.GetLeft(shape),Canvas.GetTop(shape));
        }

        public override void Execute()
        {
            // set the shape to his orginal position.
            Canvas.SetLeft(shape, new_pos.X);
            Canvas.SetTop (shape, new_pos.Y);
            // toggle the outline off.
            m.selection.ToggleOutline(false);
            // select it.
            new SelectShapeCommand(shape).Execute();
        }

        public override void Undo()
        {
            // set the shape to his orginal position.
            Canvas.SetLeft(shape, orgin_pos.X);
            Canvas.SetTop (shape, orgin_pos.Y);
            // toggle the outlnie off.
            m.selection.ToggleOutline(false);
            // select it.
            new SelectShapeCommand(shape).Execute();
        }
    }
}

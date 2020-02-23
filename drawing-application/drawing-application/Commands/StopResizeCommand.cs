﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class StopResizeCommand : Command
    {
        Shape shape;

        Point orgin_scale;

        Point orgin_position;

        Point new_scale;

        Point new_position;

        public StopResizeCommand()
        {
            shape          = m.shape_selected;
            orgin_scale    = m.orgin_scale;
            orgin_position = m.orgin_position;

            new_scale    = new Point(shape.Width, shape.Height);

            new_position = new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));

        }

        public override void Execute()
        {
            //m.SwitchState(states.select);

            shape.Width  = new_scale.X;
            shape.Height = new_scale.Y;

            Canvas.SetLeft(shape, new_position.X);
            Canvas.SetTop (shape, new_position.Y);

            new SelectShapeCommand(shape).Execute();
        }

        public override void Undo()
        {
            shape.Width  = orgin_scale.X;
            shape.Height = orgin_scale.Y;

            Canvas.SetLeft(shape, orgin_position.X );
            Canvas.SetTop (shape, orgin_position.Y);

            new SelectShapeCommand(shape).Execute();
        }
    }
}

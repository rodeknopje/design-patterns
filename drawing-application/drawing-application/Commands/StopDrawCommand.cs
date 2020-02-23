﻿using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class StopDrawCommand : Command
    {
        Shape shape;

        public StopDrawCommand() 
        {
            // remove the drawn_shape from the canvas.
            m.draw_canvas.Children.Remove(m.shape_drawn);
            // assign the shape of this command with the drawn shape.
            shape = m.shape_drawn;
        }

        // alternate constructor for loading shapes from the savefile.
        public StopDrawCommand(shapes style, int[] pos_data)
        {                      
            shape = m.CreateShape(style);

            Canvas.SetLeft(shape, pos_data[0]);
            Canvas.SetTop (shape, pos_data[1]);

            shape.Width  = pos_data[2];
            shape.Height = pos_data[3];
        }

        public override void Execute()
        {
            // add the shape of this command to the canvas.
            m.draw_canvas.Children.Add(shape);
            // add it to the selection row.
            m.AddToSelectionRow(shape);
            // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
            m.shape_drawn = null;

            m.SwitchState(states.none);
        }

        public override void Undo()
        {

        }
    }
}

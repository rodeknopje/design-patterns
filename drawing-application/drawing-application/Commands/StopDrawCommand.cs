﻿using drawing_application.CustomShapes;
using System;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopDrawCommand : Command
    {
        CustomShape shape;
        Button button;

        public StopDrawCommand() 
        {
            // assign the shape of this command with the drawn shape.
            shape = m.shape_drawn;
            // remove the drawn_shape from the canvas.
            m.draw_canvas.Children.Remove(shape);

            button = m.CreateSelectButton(shape);
        }

        // alternate constructor for loading shapes from the savefile.
        public StopDrawCommand(int index, int[] pos_data)
        {                      
            shape = m.CreateShape(index);

            Canvas.SetLeft(shape, pos_data[0]);
            Canvas.SetTop (shape, pos_data[1]);

            shape.Width  = pos_data[2];
            shape.Height = pos_data[3];
            
            button = m.CreateSelectButton(shape);
        }

        public override void Execute()
        {
            // add the shape of this command to the canvas.
            m.draw_canvas.Children.Add(shape);
            // add it to the selection row.
            m.selection_row.Children.Add(button);

            m.SwitchState(states.none);
        }

        public override void Undo()
        {
            m.selection.ToggleOutline(false);

            m.draw_canvas.Children.Remove(shape);

            m.selection_row.Children.Remove(button);

            m.SwitchState(states.none);

        }
    }
}

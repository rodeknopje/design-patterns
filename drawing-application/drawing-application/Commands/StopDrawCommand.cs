using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class StopDrawCommand : Command
    {
        Shape shape;
        Button button;

        public StopDrawCommand() 
        {
            // assign the shape of this command with the drawn shape.
            shape = m.shape_drawn;
            // remove the drawn_shape from the canvas.
            m.draw_canvas.Children.Remove(shape);

            button = m.AddToSelectionRow(shape);
        }

        // alternate constructor for loading shapes from the savefile.
        public StopDrawCommand(shapes style, int[] pos_data)
        {                      
            shape = m.CreateShape(style);

            Canvas.SetLeft(shape, pos_data[0]);
            Canvas.SetTop (shape, pos_data[1]);

            shape.Width  = pos_data[2];
            shape.Height = pos_data[3];
            
            button = m.AddToSelectionRow(shape);
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
            m.DeleteSelectionItems();

            m.draw_canvas.Children.Remove(shape);

            m.selection_row.Children.Remove(button);

            m.SwitchState(states.none);

        }
    }
}

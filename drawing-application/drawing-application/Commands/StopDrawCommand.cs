using System;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopDrawCommand : Command
    {
        public StopDrawCommand() { }

        public StopDrawCommand(shapes style, int[] pos_data)
        {
            m.shape_drawn = m.CreateShape(style);

            Canvas.SetLeft(m.shape_drawn, pos_data[0]);
            Canvas.SetTop (m.shape_drawn, pos_data[1]);

            m.shape_drawn.Width  = pos_data[2];
            m.shape_drawn.Height = pos_data[3];

            m.draw_canvas.Children.Add(m.shape_drawn);
        }

        public override void Execute()
        {
            // add it to the selection row.
            m.AddToSelectionRow(m.shape_drawn);
            // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
            m.shape_drawn = null;

            m.SwitchState(states.none);
        }

        public override void Undo()
        {

        }
    }
}

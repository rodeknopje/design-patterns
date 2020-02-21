using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class DrawCommand : Command
    {
        Point mouse_pos;

        public DrawCommand(Point mouse_pos)
        {
            this.mouse_pos = mouse_pos;
        }

        public override void Execute()
        {
            // get the offset from the orgin point.
            var x_offset = mouse_pos.X - m.mouse_orgin.X;
            var y_offset = mouse_pos.Y - m.mouse_orgin.Y;
            // if the x offset is greater than zero.
            if (x_offset > 0)
            {
                // set the width to the offset.
                m.shape_drawn.Width = x_offset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetLeft(m.shape_drawn, x_offset + m.mouse_orgin.X);
                // inverse the offset to make it positive.
                m.shape_drawn.Width = -x_offset;
            }
            if (y_offset > 0)
            {
                // set the width to the offset.
                m.shape_drawn.Height = y_offset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetTop(m.shape_drawn, y_offset + m.mouse_orgin.Y);
                // inverse the offset to make it positive.
                m.shape_drawn.Height = -y_offset;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

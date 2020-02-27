using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class StartDrawCommand : Command
    {
        Point orgin;
        public StartDrawCommand(Point orgin)
        {
            this.orgin = orgin;
        }

        public override void Execute()
        {
            m.style_select.Visibility = Visibility.Collapsed;

            m.SwitchState(states.draw);
            // create a point variable to store coordinates
            m.orgin_mouse = orgin;
            // create a new shape based on the selected shape.
            m.shape_drawn = m.CreateShape(m.style_index);

            // set the position of the shape.
            Canvas.SetLeft(m.shape_drawn, orgin.X);
            Canvas.SetTop(m.shape_drawn, orgin.Y);
            // add it to the canvas.
            m.draw_canvas.Children.Add(m.shape_drawn);
            // add the new shape to the shapelist.
            m.shapelist.Add(m.shape_drawn);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

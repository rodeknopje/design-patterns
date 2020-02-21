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
            m.SwitchState(states.draw);
            // create a point variable to store coordinates
            m.mouse_orgin = orgin;
            // create a new shape based on the selected shape.
            m.shape_drawn = (Shape)Activator.CreateInstance(m.shape_style == shapes.rectangle ? typeof(Rectangle) : typeof(Ellipse));
            {
                m.shape_drawn.Width = 0;
                m.shape_drawn.Height = 0;
                m.shape_drawn.Fill = Brushes.Transparent;
                m.shape_drawn.Stroke = new SolidColorBrush(Color.FromRgb(255, 110, 199));
                m.shape_drawn.StrokeThickness = 2.5;
            }

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

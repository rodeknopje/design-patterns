using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class StartDrawCommand : Command
    {
        // the mouse position of the mouse when the draw started.
        private readonly Point mousePos;
        public StartDrawCommand(Point mousePos)
        {
            // assign the mouse position.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // collapse the style selection.
            M.style_select.Visibility = Visibility.Collapsed;
            // switch to the draw state.
            M.SwitchState(states.draw);
            // set the mouse origin in the main.
            M.orgin_mouse = mousePos;
            // create a new shape based on the selected shape.
            M.shape_drawn = M.CreateShape(M.style_index);

            // set x position of the shape equal to the mouse x
            Canvas.SetLeft(M.shape_drawn, mousePos.X);
            // set y position of the shape equal to the mouse y
            Canvas.SetTop(M.shape_drawn, mousePos.Y);
            // add it to the canvas.
            M.draw_canvas.Children.Add(M.shape_drawn);
            // add the new shape to the shape list.
            M.shapelist.Add(M.shape_drawn);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

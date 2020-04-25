using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class StartDrawCommand : Command
    {
        // the mouse position of the mouse when the Draw started.
        private readonly Point mousePos;
        public StartDrawCommand(Point mousePos)
        {
            // assign the mouse position.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // collapse the style selection.
            M.stylesDisplay.Visibility = Visibility.Collapsed;
            // switch to the Draw state.
            M.SwitchState(States.Draw);
            // set the mouse origin in the main.
            M.mouseOrigin = mousePos;
            // create a new shape based on the selected shape.
            M.shapeDrawn = M.CreateShape(M.styleIndex);

            // set x position of the shape equal to the mouse x
            Canvas.SetLeft(M.shapeDrawn, mousePos.X);
            // set y position of the shape equal to the mouse y
            Canvas.SetTop(M.shapeDrawn, mousePos.Y);
            // add it to the canvas.
            M.drawCanvas.Children.Add(M.shapeDrawn);
            // add the new shape to the shape list.
            M.shapes.Add(M.shapeDrawn);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

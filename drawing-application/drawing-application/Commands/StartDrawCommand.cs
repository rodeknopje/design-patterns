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
            m.stylesDisplay.Visibility = Visibility.Collapsed;
            // switch to the Draw state.
            m.SwitchState(States.Draw);
            // set the mouse origin in the main.
            m.mouseOrigin = mousePos;
            // create a new shape based on the selected shape.
            m.shapeDrawn = m.CreateShape(m.styleIndex);

            // set x position of the shape equal to the mouse x
            Canvas.SetLeft(m.shapeDrawn, mousePos.X);
            // set y position of the shape equal to the mouse y
            Canvas.SetTop(m.shapeDrawn, mousePos.Y);
            // add it to the canvas.
            m.drawCanvas.Children.Add(m.shapeDrawn);
            // add the new shape to the shape list.
            m.shapes.Add(m.shapeDrawn);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

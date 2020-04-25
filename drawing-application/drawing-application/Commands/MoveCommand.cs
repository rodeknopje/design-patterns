using System;
using System.Windows;

namespace drawing_application.Commands
{
    public class MoveCommand : Command
    {
        // the current position of the mouse.
        private readonly Point mousePos;

        public MoveCommand(Point mousePos)
        {
            // assign the mouse position.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // the offset from the start pos the the current pos.
            var offset = new Point
            { 
                // calculate the mouse offset
                X = mousePos.X - m.mouseOrigin.X,
                Y = mousePos.Y - m.mouseOrigin.Y,
            };

            // Move the shape based on the offset.
            Selection.GetInstance().Move(offset);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

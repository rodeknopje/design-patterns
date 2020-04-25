using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class DrawCommand : Command
    {
        // the current position of the mouse.
        private readonly Point mousePos;

        public DrawCommand(Point mousePos)
        {
            // assign the mouse position.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // get the offset from the origin point.
            var xOffset = mousePos.X - M.mouseOrigin.X;
            var yOffset = mousePos.Y - M.mouseOrigin.Y;
            // if the x offset is greater than zero.
            if (xOffset > 0)
            {
                // set the width to the offset.
                M.shapeDrawn.Width = xOffset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetLeft(M.shapeDrawn, xOffset + M.mouseOrigin.X);
                // inverse the offset to make it positive.
                M.shapeDrawn.Width = -xOffset;
            }
            if (yOffset > 0)
            {
                // set the width to the offset.
                M.shapeDrawn.Height = yOffset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetTop(M.shapeDrawn, yOffset + M.mouseOrigin.Y);
                // inverse the offset to make it positive.
                M.shapeDrawn.Height = -yOffset;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

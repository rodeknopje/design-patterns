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
            var xOffset = mousePos.X - M.orgin_mouse.X;
            var yOffset = mousePos.Y - M.orgin_mouse.Y;
            // if the x offset is greater than zero.
            if (xOffset > 0)
            {
                // set the width to the offset.
                M.shape_drawn.Width = xOffset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetLeft(M.shape_drawn, xOffset + M.orgin_mouse.X);
                // inverse the offset to make it positive.
                M.shape_drawn.Width = -xOffset;
            }
            if (yOffset > 0)
            {
                // set the width to the offset.
                M.shape_drawn.Height = yOffset;
            }
            else
            {
                // otherwise set the left 
                Canvas.SetTop(M.shape_drawn, yOffset + M.orgin_mouse.Y);
                // inverse the offset to make it positive.
                M.shape_drawn.Height = -yOffset;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

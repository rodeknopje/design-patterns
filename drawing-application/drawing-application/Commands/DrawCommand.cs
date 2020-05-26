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
            var xOffset = mousePos.X - m.mouseOrigin.X;
            var yOffset = mousePos.Y - m.mouseOrigin.Y;
            // if the x offset is greater than zero.
            if (xOffset > 0)
            {
                // set the width to the offset.
                //m.shapeDrawn.Width = xOffset;
                m.shapeDrawn.SetWidth(xOffset);
            }
            else
            {
                // otherwise set the left 
                //Canvas.SetLeft(m.shapeDrawn, xOffset + m.mouseOrigin.X);
                m.shapeDrawn.SetLeft(xOffset + m.mouseOrigin.X);
                // inverse the offset to make it positive.
                // m.shapeDrawn.Width = -xOffset;
                m.shapeDrawn.SetWidth(-xOffset);
            }
            if (yOffset > 0)
            {
                // set the width to the offset.
                //m.shapeDrawn.Height = yOffset;
                m.shapeDrawn.SetHeight(yOffset);
            }
            else
            {
                // otherwise set the left 
                // Canvas.SetTop(m.shapeDrawn, yOffset + m.mouseOrigin.Y);
                m.shapeDrawn.SetTop(yOffset + m.mouseOrigin.Y);
                // inverse the offset to make it positive.
                // m.shapeDrawn.Height = -yOffset;
                m.shapeDrawn.SetHeight(-yOffset);
                
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class ResizeCommand : Command
    {
        private readonly Point mousePos;

        public ResizeCommand(Point mousePos)
        {
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // the transform of the selection.
            var sTransform = Selection.GetInstance().GetTransform();
            // get the offset from the original mouse position.
            var xOffset = mousePos.X - m.mouseOrigin.X;
            var yOffset = mousePos.Y - m.mouseOrigin.Y;
            // calculate offset in percentages.
            var xPercent = xOffset / sTransform.width  + 1;
            var yPercent = yOffset / sTransform.height + 1;

            // update the outline and handle of the selection.
            Selection.GetInstance().ApplyOutlineOffset(new Point(xOffset, yOffset));

            // loop through all the shapes in the selection.
            Selection.GetInstance().GetAllShapes().ForEach(shape =>
            {
                // the new transform that will be assigned to the shape.
                var transform = new Transform(sTransform.x,sTransform.y,0,0);

                if (xPercent > 0)
                {
                    // calculate how much the x position is relative of the width.
                    var xRelativeToWidth = (shape.OriginTransform.x - sTransform.x) / sTransform.width;
                    // multiply the relative width to the width multiply that with the Move percentage and add it to the x position.
                    var newX = sTransform.x + (sTransform.width * xRelativeToWidth) * xPercent;
                    // assign it to the transform.
                    transform.x = newX;

                    // multiply the shape width with the offset percentage.
                    transform.width = shape.OriginTransform.width * xPercent;
                }
                if (yPercent > 0)
                {
                    // calculate how much the x position is relative of the height.
                    var yRelativeToHeight = (shape.OriginTransform.y - sTransform.y) / sTransform.height;
                    // multiply the relative width to the width multiply that with the Move percentage and add it to the x position.
                    var newY = sTransform.y + (sTransform.height * yRelativeToHeight) * yPercent;
                    // assign it to the transform.
                    transform.y = newY;

                    // multiply the shape height with the offset percentage.
                    transform.height = shape.OriginTransform.height * yPercent;
                }
                if(xPercent <= 0)
                {
                    // calculate how much the x position is relative of the width.
                    var xRelativeToWidth = (shape.OriginTransform.x - sTransform.x) / sTransform.width;
                    // multiply the relative width to the width, multiply that with the Move inverse Move percentage and add it to the x position.
                    var newX = sTransform.x + (sTransform.width * xRelativeToWidth) * -xPercent;

                    // assign it to the transform.
                    transform.x = xOffset + sTransform.width + newX;

                    // multiply the original width of the shape with the x percent
                    transform.width = shape.OriginTransform.width * -xPercent;
                }
                if(yPercent <= 0)
                {
                    // calculate how much the y position is relative of the height.
                    var yRelativeToWidth = (shape.OriginTransform.y - sTransform.y) / sTransform.height;
                    // multiply the relative width to the width, multiply that with the Move inverse Move percentage and add it to the x position.
                    var newY = sTransform.y + (sTransform.height * yRelativeToWidth) * -yPercent;
                    // assign it to the transform.
                    transform.y = yOffset + sTransform.height + newY;

                    // multiply the original height of the shape with the x percent
                    transform.height = shape.OriginTransform.height * -yPercent;
                }

                // assign the position plus the original relative position so i cannot reach zero
                Canvas.SetLeft(shape, transform.x);
                Canvas.SetTop (shape, transform.y);
                // assign the scale plus the original relative position so i cannot reach zero
                shape.Width  = transform.width;
                shape.Height = transform.height;

            });


        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

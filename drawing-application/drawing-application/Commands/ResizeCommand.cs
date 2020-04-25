using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class ResizeCommand : Command
    {
        private readonly Point mouse_pos;

        public ResizeCommand(Point mouse_pos)
        {
            this.mouse_pos = mouse_pos;
        }

        public override void Execute()
        {
            var sTransform = m.selection.GetTransform();
            //// get the offset from the orginal mouse position.
            var x_offset = mouse_pos.X - m.orgin_mouse.X;
            var y_offset = mouse_pos.Y - m.orgin_mouse.Y;
            // calculate offset in percentages.
            var x_percent = (x_offset / sTransform.width)  + 1;
            var y_percent = (y_offset / sTransform.heigth) + 1;

            m.selection.ApplyOutlineOffset(new Point(x_offset, y_offset));

            m.selection.GetAllShapes().ForEach(shape =>
            {
                var transform = new Transform(sTransform.x,sTransform.y,0,0);

                if (x_percent > 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var xRelativeToWidth = (shape.OriginTransform.x - sTransform.x) / sTransform.width;
                    // multiply the relative width to the width multiply that with the move percentage and add it to the xposition.
                    var newX = sTransform.x + (sTransform.width * xRelativeToWidth) * x_percent;
                    // assign it to the transform.
                    transform.x = newX;

                    // multiply the shape width with the offset percentage.
                    transform.width = shape.OriginTransform.width * x_percent;
                }
                if (y_percent > 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var yRelativeToHeight = (shape.OriginTransform.y - sTransform.y) / sTransform.heigth;
                    // multiply the relative width to the width multiply that with the move percentage and add it to the xposition.
                    var newY = sTransform.y + (sTransform.heigth * yRelativeToHeight) * y_percent;
                    // assign it to the transform.
                    transform.y = newY;

                    // multiply the shape heigth with the offset percentage.
                    transform.heigth = shape.OriginTransform.heigth * y_percent;
                }
                if(x_percent <= 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var xRelativeToWidth = (shape.OriginTransform.x - sTransform.x) / sTransform.width;
                    // multiply the relative width to the width, multiply that with the move inverse move percentage and add it to the xposition.
                    var newX = sTransform.x + (sTransform.width * xRelativeToWidth) * -x_percent;

                    // assign it to the transform.
                    transform.x = x_offset + sTransform.width + newX;

                    // multiply the orgiginal width of the shape with the x percent
                    transform.width = shape.OriginTransform.width * -x_percent;
                }
                if(y_percent <= 0)
                {
                    // caculate how much the y position is relitive of the heigth.
                    var yRelativeToWidth = (shape.OriginTransform.y - sTransform.y) / sTransform.heigth;
                    // multiply the relative width to the width, multiply that with the move inverse move percentage and add it to the xposition.
                    var newY = sTransform.y + (sTransform.heigth * yRelativeToWidth) * -y_percent;
                    // assign it to the transform.
                    transform.y = y_offset + sTransform.heigth + newY;

                    // multiply the orgiginal heigth of the shape with the x percent
                    transform.heigth = shape.OriginTransform.heigth * -y_percent;
                }
                // small number to calculate an always offset and scale. so shapes stay relative to each othet.
                float smallnumber = 0.000001f;
                // assign the position plus the original relative position so i cannot reach zero
                Canvas.SetLeft(shape, transform.x + smallnumber * shape.OriginTransform.x);
                Canvas.SetTop (shape, transform.y + smallnumber * shape.OriginTransform.y);
                // assign the scale plus the original relative position so i cannot reach zero
                shape.Width  =  transform.width  + smallnumber * shape.OriginTransform.width;
                shape.Height = transform.heigth  + smallnumber * shape.OriginTransform.heigth;

            });


        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

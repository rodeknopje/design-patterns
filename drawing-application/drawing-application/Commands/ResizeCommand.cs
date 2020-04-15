using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class ResizeCommand : Command
    {
        Point mouse_pos;

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

            m.selection.MoveHandle(new Point(x_offset, y_offset));


            m.selection.GetAllShapes().ForEach(shape =>
            {
                var transform = new Transform(sTransform.x,sTransform.y,0,0);

                if (x_percent > 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var xRelativeToWidth = (shape.orginTransform.x - sTransform.x) / sTransform.width;
                    // multiply the relative width to the width multiply that with the move percentage and add it to the xposition.
                    var newX = sTransform.x + (sTransform.width * xRelativeToWidth) * x_percent;
                    // assign it to the transform.
                    transform.x = newX;

                    // multiply the shape width with the offset percentage.
                    transform.width = shape.orginTransform.width * x_percent;
                    
                    m.selection.outline.Width = Canvas.GetLeft(m.selection.handle) - Canvas.GetLeft(m.selection.outline) + (m.selection.handle.Width / 2); 
                }
                if (y_percent > 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var yRelativeToHeight = (shape.orginTransform.y - sTransform.y) / sTransform.heigth;
                    // multiply the relative width to the width multiply that with the move percentage and add it to the xposition.
                    var newY = sTransform.y + (sTransform.heigth * yRelativeToHeight) * y_percent;
                    // assign it to the transform.
                    transform.y = newY;

                    // multiply the shape heigth with the offset percentage.
                    transform.heigth = shape.orginTransform.heigth * y_percent;

                    m.selection.outline.Height = Canvas.GetTop(m.selection.handle) - Canvas.GetTop(m.selection.outline) + (m.selection.handle.Width / 2);
                }
                if(x_percent < 0)
                {
                    Canvas.SetLeft(m.selection.outline, Canvas.GetLeft( m.selection.handle) + m.selection.handle.Width / 2);

                    m.selection.outline.Width =  (-x_offset - m.selection.orginTransform.width);
                }
                if(y_percent < 0)
                {
                    Canvas.SetTop(m.selection.outline, Canvas.GetTop(m.selection.handle) + m.selection.handle.Height / 2);

                    m.selection.outline.Height = (-y_offset - m.selection.orginTransform.heigth);


                }
                // small number to calculate an always offset and scale. so shapes stay relative to each othet.
                float smallnumber = 0.000001f;
                // assign the position plus the original relative position so i cannot reach zero
                Canvas.SetLeft(shape, transform.x + smallnumber * shape.orginTransform.x);
                Canvas.SetTop (shape, transform.y + smallnumber * shape.orginTransform.y);
                // assign the scale plus the original relative position so i cannot reach zero
                shape.Width  =  transform.width  + smallnumber * shape.orginTransform.width;
                shape.Height = transform.heigth  + smallnumber * shape.orginTransform.heigth;

            });



           // m.debug_text.Text = $"{Math.Round(x_percent,2)}, {Math.Round(y_percent,2)}";

            //// get the offset from the orginal mouse position.
            //var x_offset = mouse_pos.X - m.orgin_mouse.X;
            //var y_offset = mouse_pos.Y - m.orgin_mouse.Y;
            //// add the offset to the handle's position.
            //Canvas.SetLeft(m.handle, m.orgin_pos_handle.X + x_offset);
            //Canvas.SetTop (m.handle, m.orgin_pos_handle.Y + y_offset);
            //// calculate the new width and heigth by adding the offset to the orginal scale.
            //var width  = m.shape_selected.orginScale.X + x_offset;
            //var heigth = m.shape_selected.orginScale.Y + y_offset;

            //// if the new width is positive. 
            //if (width >= 0)
            //{
            //    // assign selected shape width with the new width.
            //    m.shape_selected.Width = width;
            //    // assign selection outline width with the new width.
            //    m.selection_outline.Width = width + m.selection_outline.StrokeThickness * 4;

            //}
            //// if the new width is negative.
            //else
            //{
            //    // assign the left from the selected shape with the orignal possition minus the offset.
            //    Canvas.SetLeft(m.shape_selected, m.orgin_pos_handle.X + x_offset);
            //    // assign the width from the selected shape with the inversed width, so it becomes the line on the right side.
            //    m.shape_selected.Width = -width;

            //    // assign the left from the selection outline with the orignal possition minus the offset.
            //    Canvas.SetLeft(m.selection_outline, m.orgin_pos_handle.X + x_offset - m.selection_outline.StrokeThickness * 2);
            //    // assign the width from the selection outline with the inversed width, so it becomes the line on the right side.
            //    m.selection_outline.Width = -width + m.selection_outline.StrokeThickness * 4;
            //}
            //if (heigth >= 0)
            //{
            //    // assign selected shape heigth with the new width.
            //    m.shape_selected.Height = heigth;
            //    // assign selection outline heigth with the new width.
            //    m.selection_outline.Height = heigth + m.selection_outline.StrokeThickness * 4;
            //}
            //else
            //{
            //    // assign the top from the selected shape with the orignal possition minus the offset
            //    Canvas.SetTop(m.shape_selected, m.orgin_pos_handle.Y + y_offset);
            //    // assign the top from the selected shape with the inversed width, so it becomes the line on the right side.
            //    m.shape_selected.Height = -heigth;

            //    // assign the top from the selection outline with the orignal possition minus the offset.
            //    Canvas.SetTop(m.selection_outline, m.orgin_pos_handle.Y + y_offset - m.selection_outline.StrokeThickness * 2);
            //    // assign the top from the selection outline with the inversed width, so it becomes the line on the right side.
            //    m.selection_outline.Height = -heigth + m.selection_outline.StrokeThickness * 4;
            //}
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

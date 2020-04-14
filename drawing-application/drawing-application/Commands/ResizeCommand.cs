﻿using System;
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
          

            m.selection.GetGroup().GetAllShapes().ForEach(shape =>
            {
                var transform = new Transform();

                if (x_percent >= 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var xRelativeToWidth = (shape.orginTransform.x - sTransform.x) / sTransform.width;
                    // multiply the relative width to the width multiply that with the move percentage and add it to the xposition.
                    var newX = sTransform.x + (sTransform.width * xRelativeToWidth) * x_percent;
                    // assign it to the transform.
                    transform.x = newX;

                    // multiply the shape width with the offset percentage.
                    transform.width = shape.orginTransform.x * x_percent;
                }
                if (y_percent >= 0)
                {
                    // caculate how much the x position is relitive of the width.
                    var yRelativeToHeight = (shape.orginTransform.y - sTransform.y) / sTransform.heigth;
                    // multiply the relative width to the width multiply that with the move percentage and add it to the xposition.
                    var newY = sTransform.y + (sTransform.heigth * yRelativeToHeight) * y_percent;
                    // assign it to the transform.
                    transform.y = newY;

                    // multiply the shape heigth with the offset percentage.
                    transform.heigth = shape.orginTransform.y * y_percent;
                }

                Canvas.SetLeft(shape, transform.x);
                Canvas.SetTop (shape, transform.y);

                shape.Width  = transform.width;
                shape.Height = transform.heigth;

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

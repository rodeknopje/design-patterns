using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
            // get the offset from the orginal mouse position.
            var x_offset = mouse_pos.X - m.mouse_orgin.X;
            var y_offset = mouse_pos.Y - m.mouse_orgin.Y;
            // add the offset to the handle's position.
            Canvas.SetLeft(m.handle, m.orgin_position.X + x_offset);
            Canvas.SetTop(m.handle, m.orgin_position.Y + y_offset);
            // calculate the new width and heigth by adding the offset to the orginal scale.
            var width = m.orgin_scale.X + x_offset;
            var heigth = m.orgin_scale.Y + y_offset;

            // if the new width is positive. 
            if (width >= 0)
            {
                // assign selected shape width with the new width.
                m.shape_selected.Width = width;
                // assign selection outline width with the new width.
                m.selection_outline.Width = width + m.selection_outline.StrokeThickness * 4;

            }
            // if the new width is negative.
            else
            {
                // assign the left from the selected shape with the orignal possition minus the offset.
                Canvas.SetLeft(m.shape_selected, m.orgin_position.X + x_offset);
                // assign the width from the selected shape with the inversed width, so it becomes the line on the right side.
                m.shape_selected.Width = -width;

                // assign the left from the selection outline with the orignal possition minus the offset.
                Canvas.SetLeft(m.selection_outline, m.orgin_position.X + x_offset - m.selection_outline.StrokeThickness * 2);
                // assign the width from the selection outline with the inversed width, so it becomes the line on the right side.
                m.selection_outline.Width = -width + m.selection_outline.StrokeThickness * 4;
            }
            if (heigth >= 0)
            {
                // assign selected shape heigth with the new width.
                m.shape_selected.Height = heigth;
                // assign selection outline heigth with the new width.
                m.selection_outline.Height = heigth + m.selection_outline.StrokeThickness * 4;
            }
            else
            {
                // assign the top from the selected shape with the orignal possition minus the offset
                Canvas.SetTop(m.shape_selected, m.orgin_position.Y + y_offset);
                // assign the top from the selected shape with the inversed width, so it becomes the line on the right side.
                m.shape_selected.Height = -heigth;

                // assign the top from the selection outline with the orignal possition minus the offset.
                Canvas.SetTop(m.selection_outline, m.orgin_position.Y + y_offset - m.selection_outline.StrokeThickness * 2);
                // assign the top from the selection outline with the inversed width, so it becomes the line on the right side.
                m.selection_outline.Height = -heigth + m.selection_outline.StrokeThickness * 4;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

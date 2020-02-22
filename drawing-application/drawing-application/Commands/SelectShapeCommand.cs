using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.Commands
{
    class SelectShapeCommand : Command
    {
        Shape shape;

        public SelectShapeCommand(Shape shape)
        {
            this.shape = shape;
        }

        private void InitializeSelectionShapes()
        {
            // assign the selection outline.
            m.selection_outline = new Rectangle
            {
                Fill = Brushes.Transparent,
                Stroke = Brushes.White,
                StrokeThickness = 2f,
                StrokeDashArray = { 5, 5 }
            };

            // create the handle.
            m.handle = new Ellipse
            {
                Width = 20,
                Height = 20,
                StrokeThickness = 2,
                Stroke = Brushes.White,
                Fill = Brushes.Gray,

            };

        }

        public override void Execute()
        {
            m.SwitchState(states.select);


            m.shape_selected = shape;
            // if there is already something selected
            if (m.selection_outline != null)
            {
                // remove the current selection outline.
                m.draw_canvas.Children.Remove(m.selection_outline);
                m.draw_canvas.Children.Remove(m.handle);
            }

            // create the selection outline and the resize handle.
            InitializeSelectionShapes();      
            // set the left and top position te be the same as the selected shape.
            Canvas.SetLeft(m.selection_outline, Canvas.GetLeft(shape) - m.selection_outline.StrokeThickness * 2);
            Canvas.SetTop(m.selection_outline,  Canvas.GetTop(shape)  - m.selection_outline.StrokeThickness * 2);
            // set the width and heigth to be the same as the selected shape.
            m.selection_outline.Width  = shape.Width  + m.selection_outline.StrokeThickness * 4;
            m.selection_outline.Height = shape.Height + m.selection_outline.StrokeThickness * 4;
            // add the selection outline to the draw_canvas.
            m.draw_canvas.Children.Add(m.selection_outline);

            // give the cursor a different icon, when hovering over the outlne.
            m.selection_outline.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.SizeAll;
            m.selection_outline.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;

            // when the selection outline is clicked.
            m.selection_outline.MouseDown += (a, b) => new StartMoveCommand(b.GetPosition(m.draw_canvas)).Execute();
        
            // move it to the bottum right.
            Canvas.SetLeft(m.handle, Canvas.GetLeft(m.selection_outline) + m.selection_outline.Width  - m.handle.Width  / 2);
            Canvas.SetTop(m.handle,  Canvas.GetTop(m.selection_outline)  + m.selection_outline.Height - m.handle.Height / 2);

            // add it to the draw canvas.
            m.draw_canvas.Children.Add(m.handle);

            // when the handle is clicked.
            m.handle.MouseDown += (a, b) => new StartResizeCommand(b.GetPosition(m.draw_canvas)).Execute();

            // give the cursor a different icon, when hovering over the outlne.
            m.handle.MouseEnter += (a, b) => Mouse.OverrideCursor = Cursors.Hand;
            m.handle.MouseLeave += (a, b) => Mouse.OverrideCursor = Cursors.Arrow;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

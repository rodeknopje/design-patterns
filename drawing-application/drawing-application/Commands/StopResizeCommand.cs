using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopResizeCommand : Command
    {
        // list off all shapes which have scaled.
        List<CustomShape> shapes = new List<CustomShape>();
        // their old and new positions
        List<Point> orginPositions = new List<Point>();
        List<Point> newPositions   = new List<Point>();
        // their old and new scales.
        List<Point> orginScales = new List<Point>();
        List<Point> newScales   = new List<Point>();

        public StopResizeCommand()
        {
            // loop through all shapes in the selection.
            m.selection.GetAllShapes().ForEach(shape =>
            {
                // add them to this shape list.
                shapes.Add(shape);
                // add their original scale the the list.
                orginScales.Add(new Point(shape.orginTransform.width, shape.orginTransform.heigth));
                // add their current scale to the list
                newScales.Add(new Point(shape.Width,shape.Height));
                // add their original pos to the list
                orginPositions.Add(new Point(shape.orginTransform.x, shape.orginTransform.y));
                // add their new pos to the list.
                newPositions.Add(new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape)));
            });
        }

        public override void Execute()
        {
            // loop through all the shapes.
            for(int i = 0; i < shapes.Count; i++)
            {
                // set the new width and heigth.
                shapes[i].Width  = newScales[i].X;
                shapes[i].Height = newScales[i].Y;
                // set the new x and y
                Canvas.SetLeft(shapes[i], newPositions[i].X);
                Canvas.SetTop (shapes[i], newPositions[i].Y);

            }
            // toggle the outline off.
            m.selection.ToggleOutline(false);
            // switch to none state.
            m.SwitchState(states.none);
        }

        public override void Undo()
        {
            // loop through all the shapes.
            for (int i = 0; i < shapes.Count; i++)
            {
                // set the new width and heigth.
                shapes[i].Width  = orginScales[i].X;
                shapes[i].Height = orginScales[i].Y;
                // set the new x and y
                Canvas.SetLeft(shapes[i], orginPositions[i].X);
                Canvas.SetTop(shapes[i],  orginPositions[i].Y);

            }
            // toggle the outline off.
            m.selection.ToggleOutline(false);
            // switch to none state.
            m.SwitchState(states.none);
        }
    }
}

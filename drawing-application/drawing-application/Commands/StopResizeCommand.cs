using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class StopResizeCommand : Command
    {
        // list off all shapes which have scaled.
        private readonly List<CustomShape> shapes = new List<CustomShape>();
        // their old and new positions
        private readonly List<Point> originPositions = new List<Point>();
        private readonly List<Point> newPositions    = new List<Point>();
        // their old and new scales.
        private readonly List<Point> originScales = new List<Point>();
        private readonly List<Point> newScales    = new List<Point>();

        public StopResizeCommand()
        {
            // loop through all shapes in the selection.
            Selection.GetInstance().GetAllShapes().ForEach(shape =>
            {
                // add them to this shape list.
                shapes.Add(shape);
                // add their original scale the the list.
                originScales.Add(new Point(shape.OriginTransform.width, shape.OriginTransform.height));
                // add their current scale to the list
                newScales.Add(new Point(shape.Width,shape.Height));
                // add their original pos to the list
                originPositions.Add(new Point(shape.OriginTransform.x, shape.OriginTransform.y));
                // add their new pos to the list.
                newPositions.Add(new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape)));
            });
        }

        public override void Execute()
        {
            // loop through all the shapes.
            for(var i = 0; i < shapes.Count; i++)
            {
                // set the new width and height.
                shapes[i].Width  = newScales[i].X;
                shapes[i].Height = newScales[i].Y;
                // set the new x and y
                Canvas.SetLeft(shapes[i], newPositions[i].X);
                Canvas.SetTop (shapes[i], newPositions[i].Y);

                shapes[i].UpdateOriginTransform();
            }
            // Deselect all the shapes, because we can only Select non selected shapes.
            shapes.ForEach(Selection.GetInstance().RemoveChild);
            // Select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }

        public override void Undo()
        {
            // loop through all the shapes.
            for (var i = 0; i < shapes.Count; i++)
            {
                // set the new width and height.
                shapes[i].Width  = originScales[i].X;
                shapes[i].Height = originScales[i].Y;
                // set the new x and y
                Canvas.SetLeft(shapes[i], originPositions[i].X);
                Canvas.SetTop(shapes[i],  originPositions[i].Y);

            }
            // Deselect all the shapes, because we can only Select non selected shapes.
            shapes.ForEach(Selection.GetInstance().RemoveChild);
            // Select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }
    }
}

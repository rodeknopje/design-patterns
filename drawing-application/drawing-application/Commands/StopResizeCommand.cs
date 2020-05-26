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
                originScales.Add(new Point(shape.GetOriginTransform().width, shape.GetOriginTransform().height));
                // add their current scale to the list
                newScales.Add(new Point(shape.GetWidth(),shape.GetHeight()));
                // add their original pos to the list
                originPositions.Add(new Point(shape.GetOriginTransform().x, shape.GetOriginTransform().y));
                // add their new pos to the list.
                newPositions.Add(new Point(shape.GetLeft(), shape.GetTop()));
            });
        }

        public override void Execute()
        {
            // loop through all the shapes.
            for(var i = 0; i < shapes.Count; i++)
            {
                // set the new width and height.
                shapes[i].SetWidth ( newScales[i].X );
                shapes[i].SetHeight( newScales[i].Y );

                // set the new x and y
                shapes[i].SetLeft(newPositions[i].X);
                shapes[i].SetTop (newPositions[i].Y);

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
                shapes[i].SetWidth (originScales[i].X);
                shapes[i].SetHeight(originScales[i].Y);

                // set the new x and y
                shapes[i].SetLeft(originPositions[i].X);
                shapes[i].SetTop (originPositions[i].Y);

            }
            // Deselect all the shapes, because we can only Select non selected shapes.
            shapes.ForEach(Selection.GetInstance().RemoveChild);
            // Select all these shapes.
            new SelectShapeCommand(shapes).Execute();
        }
    }
}

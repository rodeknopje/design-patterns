using System.Collections.Generic;
using drawing_application.CustomShapes;

namespace drawing_application.Commands
{
    public class MergeCommand : Command
    {
        // the shapes which will be merged.
        private readonly List<CustomShape> shapes = new List<CustomShape>();
        // the result of the merged shapes.
        private readonly Group merged = new Group();

        public MergeCommand()
        {
            // copy the currently selected shapes to the shapes list.
            Selection.GetInstance().GetChildren().ForEach(shapes.Add);
            // add the shapes to the group.
            shapes.ForEach(merged.AddChild);
        }

        public override void Execute()
        {
            // remove the shapes form the hierarchy but not from the canvas.
            shapes.ForEach(x => Hierarchy.GetInstance().RemoveFromHierarchy(x));
            // add the group to the hierarchy.
            Hierarchy.GetInstance().AddToHierarchy(merged);


            Selection.GetInstance().Clear();
        }

        public override void Undo()
        {
            // remove the group from the hierarchy.
            Hierarchy.GetInstance().RemoveFromHierarchy(merged);
            // add the shapes to the hierarchy but not the the canvas since they were never removed from the canvas.
            shapes.ForEach(x => Hierarchy.GetInstance().AddToHierarchy(x));

            Selection.GetInstance().Clear();
        }
    }
}

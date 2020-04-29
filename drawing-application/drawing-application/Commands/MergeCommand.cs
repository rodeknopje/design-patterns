using System.Collections.Generic;
using System.Windows.Controls;
using drawing_application.CustomShapes;

namespace drawing_application.Commands
{
    public class MergeCommand : Command
    {
        private readonly List<CustomShape> shapes = new List<CustomShape>();

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
            shapes.ForEach(Hierarchy.GetInstance().RemoveFromHierarchy);

            m.drawCanvas.Children.Add(merged);

            Hierarchy.GetInstance().AddToHierarchy(merged);
        }

        public override void Undo()
        {
            shapes.ForEach(Hierarchy.GetInstance().AddToHierarchy);

           // m.drawCanvas.Children.
        }
    }
}

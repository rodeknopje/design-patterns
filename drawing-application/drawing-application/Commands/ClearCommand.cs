using drawing_application.CustomShapes;
using System.Collections.Generic;

namespace drawing_application.Commands
{
    public class ClearCommand : Command
    {
        // the buttons that are currently in the scene.
        private readonly List<CustomShape>  shapes;

        public ClearCommand()
        {
            // toggle the outline off.
            Selection.GetInstance().ToggleOutline(false);
            // initialize the shapes list.
            shapes  = new List<CustomShape>();
            // loop through the buttons and shapes.
            for (var i = 0; i < m.drawCanvas.Children.Count; i++)
            {
                // copy all the shapes to this array.
                shapes.Add((CustomShape)m.drawCanvas.Children[i]);
            }
        }
        public override void Execute()
        {
            // remove all shapes.
            shapes.ForEach(x=>Hierarchy.GetInstance().RemoveFromHierarchy(x,true));
            // program state is None.
            m.SwitchState(States.None);
        }

        public override void Undo()
        {
            // add the removed shapes back.
            shapes.ForEach(x => Hierarchy.GetInstance().AddToHierarchy(x, true));
            // program state is None.
            m.SwitchState(States.None);
        }
    }
}

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
                shapes.Add((CustomShape)m.drawCanvas.Children[i]);
            }
        }
        public override void Execute()
        {
            // remove all shapes.
            shapes.ForEach(Hierarchy.GetInstance().RemoveFromHierarchy);
            // program state is None.
            m.SwitchState(States.None);
            // clear the save file.
            m.saveLoad.ClearFile();
        }

        public override void Undo()
        {
            shapes.ForEach(Hierarchy.GetInstance().AddToHierarchy);
        }
    }
}

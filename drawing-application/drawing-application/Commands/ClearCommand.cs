using drawing_application.CustomShapes;
using System.Collections.Generic;

namespace drawing_application.Commands
{
    public class ClearCommand : Command
    {
        // the buttons that are currently in the scene.
        private readonly List<CustomShape>  shapes;
        // the group that we are currently in.
        private readonly Group currentGroup;

        public ClearCommand()
        {
            // toggle the outline off.
            Selection.GetInstance().ToggleOutline(false);
            // initialize the shapes list.
            shapes  = new List<CustomShape>();

            currentGroup = Hierarchy.GetInstance().GetCurrentGroup();
            // loop through the buttons and shapes.
            Hierarchy.GetInstance().GetChildrenInTopGroup().ForEach(shapes.Add);
        }
        public override void Execute()
        {
            // switch to the top level.
            Hierarchy.GetInstance().SwitchToTopLevel();
            // clear the hierarchy.
            Hierarchy.GetInstance().Clear();
            // program state is None.
            m.SwitchState(States.None);
        }

        public override void Undo()
        {
            // recreate the top level from the shapes we saved.
            shapes.ForEach(x=>Hierarchy.GetInstance().AddToHierarchy(x,true));
            // switch to the group we were located in before this command was invoked.
            Hierarchy.GetInstance().SwitchGroup(currentGroup);
            // program state is None.
            m.SwitchState(States.None);
        }
    }
}

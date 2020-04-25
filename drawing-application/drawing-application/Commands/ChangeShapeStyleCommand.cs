using System;

namespace drawing_application.Commands
{
    public class ChangeShapeStyleCommand : Command
    {
        // index of the shape style.
        private readonly int index;

        public ChangeShapeStyleCommand(int index)
        {
            // assign the index.
            this.index = index;
        }

        public override void Execute()
        {
            // set the style index from the main to this index.
            M.styleIndex = index;
            // toggle the outline to false.
            Selection.GetInstance().ToggleOutline(false);
            // switch to None state.
            M.SwitchState(States.None);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

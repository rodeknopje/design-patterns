using System;

namespace drawing_application.Commands
{
    public class ChangeShapeStyleCommand : Command
    {
        private readonly int index;

        public ChangeShapeStyleCommand(int index)
        {
            this.index = index;
        }

        public override void Execute()
        {
            m.style_index = index;

            m.selection.ToggleOutline(false);
  
            m.SwitchState(states.none);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

using drawing_application.CustomShapes;

namespace drawing_application.Commands
{
    public class SwitchGroupCommand : Command
    {
        private readonly Group current;
        private readonly Group next;

        public SwitchGroupCommand(Group next)
        {
            current = Hierarchy.GetInstance().GetCurrentGroup();

            this.next     = next;
        }

        public override void Execute()
        {
            Hierarchy.GetInstance().SwitchGroup(next);

            Selection.GetInstance().Clear();
        }

        public override void Undo()
        {
            Hierarchy.GetInstance().SwitchGroup(current);

            Selection.GetInstance().Clear();
        }
    }
}


namespace drawing_application.Commands
{
    class StopMoveCommand : Command
    {
        public override void Execute()
        {
            m.SwitchState(states.select);
        }

        public override void Undo()
        {

        }
    }
}

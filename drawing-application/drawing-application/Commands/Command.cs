namespace drawing_application.Commands
{
    public abstract class Command
    {
        protected MainWindow M;

        protected Command() => M = MainWindow.ins;

        public abstract void Execute();

        public abstract void Undo();
    }
}

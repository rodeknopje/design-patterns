namespace drawing_application.Commands
{
    public abstract class Command
    {
        protected MainWindow m;

        protected Command() => m = MainWindow.ins;

        public abstract void Execute();

        public abstract void Undo();
    }
}

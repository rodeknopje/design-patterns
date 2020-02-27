namespace drawing_application.Commands
{
    class ChangeShapeStyleCommand : Command
    {
        int index;

        public ChangeShapeStyleCommand(int index)
        {
            this.index = index;
        }

        public override void Execute()
        {
            m.style_index = index;
            m.draw_canvas.Children.Remove(m.selection_outline);
            m.draw_canvas.Children.Remove(m.handle);
            m.SwitchState(states.none);
        }

        public override void Undo()
        { 
            
        }
    }
}

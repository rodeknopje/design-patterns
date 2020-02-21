namespace drawing_application.Commands
{
    class ChangeShapeStyleCommand : Command
    {
        shapes shape;

        public ChangeShapeStyleCommand(shapes shape)
        {
            this.shape = shape;
        }

        public override void Execute()
        {
            m.shape_style = shape;
            m.draw_canvas.Children.Remove(m.selection_outline);
            m.draw_canvas.Children.Remove(m.handle);
            m.SwitchState(states.none);
        }

        public override void Undo()
        { 
            
        }
    }
}

using System.Collections.Generic;
using System.Windows.Controls;
using drawing_application.CustomShapes;

namespace drawing_application.Commands
{
    public class MergeCommand : Command
    {
        private readonly List<CustomShape> shapes = new List<CustomShape>();

        private readonly List<SelectButton> buttons = new List<SelectButton>();

        private readonly Group merged = new Group();

        private readonly SelectButton groupButton;

        public MergeCommand()
        {
            // copy the currently selected shapes to the shapes list.
            Selection.GetInstance().GetChildren().ForEach(shapes.Add);
            // copy the currently active buttons to the buttons list.
            m.GetActiveSelectButtons().ForEach(buttons.Add);

            shapes.ForEach(merged.AddChild);

            groupButton = m.CreateSelectButton(merged);
        }

        public override void Execute()
        {
            shapes.ForEach(m.drawCanvas.Children.Remove);

            buttons.ForEach(m.selectionDisplay.Children.Remove);
        }

        public override void Undo()
        {
            
        }
    }
}

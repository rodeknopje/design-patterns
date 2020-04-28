using drawing_application.CustomShapes;
using System.Collections.Generic;

namespace drawing_application.Commands
{
    public class ClearCommand : Command
    {
        // the buttons that are currently in the scene.
        private readonly List<CustomShape>  shapes;
        // the shapes that are currently in the scene.
        private readonly List<SelectButton> buttons;

        public ClearCommand()
        {
            // toggle the outline off.
            Selection.GetInstance().ToggleOutline(false);
            // initialize the shapes list.
            shapes  = new List<CustomShape>();
            // initialize the buttons list.
            buttons = new List<SelectButton>();
            // loop through the buttons and shapes.
            for (var i = 0; i < m.drawCanvas.Children.Count; i++)
            {
                shapes.Add((CustomShape)m.drawCanvas.Children[i]);
            }
            // add but
            m.GetSelectionButtons().ForEach(buttons.Add);
        }
        public override void Execute()
        {   
            // de select all shapes.
            m.DeselectAllShapes();
            // remove all shapes.
            m.drawCanvas.Children.Clear();
            // remove all buttons in the selection row.
            m.selectionDisplay.Children.Clear();
            // program state is None.
            m.SwitchState(States.None);
            // clear the save file.
            m.saveLoad.ClearFile();
        }

        public override void Undo()
        {
            for (var i = 0; i < shapes.Count; i++)
            {
                m.drawCanvas.Children.Add(shapes[i]);
                m.selectionDisplay.Children.Add(buttons[i]);
            }
        }
    }
}

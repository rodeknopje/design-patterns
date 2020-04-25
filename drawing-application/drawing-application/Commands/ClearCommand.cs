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
            for (var i = 0; i < M.drawCanvas.Children.Count; i++)
            {
                // 
                shapes.Add((CustomShape)M.drawCanvas.Children[i]);

                buttons.Add((SelectButton)M.selectionDisplay.Children[i]);
            }
        }
        public override void Execute()
        {   
            // de select all shapes.
            M.DeselectAllShapes();
            // remove all shapes.
            M.drawCanvas.Children.Clear();
            // remove all buttons in the selection row.
            M.selectionDisplay.Children.Clear();
            // program state is None.
            M.SwitchState(States.None);
            // clear the save file.
            M.saveLoad.ClearFile();
        }

        public override void Undo()
        {
            for (var i = 0; i < shapes.Count; i++)
            {
                M.drawCanvas.Children.Add(shapes[i]);
                M.selectionDisplay.Children.Add(buttons[i]);
            }
        }
    }
}

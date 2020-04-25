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
            M.selection.ToggleOutline(false);
            // initialize the shapes list.
            shapes  = new List<CustomShape>();
            // initialize the buttons list.
            buttons = new List<SelectButton>();

            // loop through the buttons and shapes.
            for (var i = 0; i < M.draw_canvas.Children.Count; i++)
            {
                // 
                shapes.Add((CustomShape)M.draw_canvas.Children[i]);

                buttons.Add((SelectButton)M.selection_row.Children[i]);
            }
        }
        public override void Execute()
        {   
            // remove all shapes.
            M.draw_canvas.Children.Clear();
            // remove all buttons in the selection row.
            M.selection_row.Children.Clear();
            // program state is none.
            M.SwitchState(states.none);
            // clear the save file.
            M.saveload.ClearFile();
        }

        public override void Undo()
        {
            for (var i = 0; i < shapes.Count; i++)
            {
                M.draw_canvas.Children.Add(shapes[i]);
                M.selection_row.Children.Add(buttons[i]);
            }
        }
    }
}

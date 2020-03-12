using drawing_application.CustomShapes;
using System.Collections.Generic;
using System.Windows.Controls;


namespace drawing_application.Commands
{
    class ClearCommand : Command
    {
        List<CustomShape>  shape_list;
        List<Button> button_list;

        public ClearCommand()
        {
            m.selection.ToggleOutline(false);

            shape_list  = new List<CustomShape>();
            button_list = new List<Button>();

            for (int i = 0; i < m.draw_canvas.Children.Count; i++)
            {
                shape_list.Add((CustomShape)m.draw_canvas.Children[i]);

                button_list.Add((Button)m.selection_row.Children[i]);
            }
        }
        public override void Execute()
        {   
            // remove all shapes.
            m.draw_canvas.Children.Clear();
            // remove all buttons in the selection row.
            m.selection_row.Children.Clear();
            // set the id to zero.
            m.ID = 0;
            // program state is none.
            m.SwitchState(states.none);
            // clear the save file.
            m.saveload.ClearFile();
        }

        public override void Undo()
        {
            for (int i = 0; i < shape_list.Count; i++)
            {
                m.draw_canvas.Children.Add(shape_list[i]);
                m.selection_row.Children.Add(button_list[i]);
            }
        }
    }
}

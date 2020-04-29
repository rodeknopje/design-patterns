using drawing_application.CustomShapes;

namespace drawing_application
{
    public class GroupButton : ShapeButton
    {
        public GroupButton(CustomShape shape) : base(shape)
        {
            // invoke the right click method when rmb is clicked.
            MouseRightButtonDown += (a, b) => OnRightClick();

            Text += $" ({((Group)shape).GetChildren().Count})";
        }

        private void OnRightClick()
        {
            // switch the hierarchy layout to this group.
            Hierarchy.GetInstance().SwitchGroup(shape as Group);
        }
    }
}

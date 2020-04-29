using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class Hierarchy
    {
        // the singleton of this class.
        private static Hierarchy instance;
        // the highest group in the hierarchy.
        private readonly Group topGroup;
        // the current selected group in the hierarchy.
        private Group currentGroup;
        // the stack panel on which shapes are displayed.
        private StackPanel stackPanel;

        private Hierarchy()
        {
            // assign the singleton.
            instance = this;
            // create a new group and assign it to the top level.
            topGroup = new Group();
            // the the current level equal to the top level.
            currentGroup = topGroup;
        }

        public static Hierarchy GetInstance()
        {
            return instance ?? new Hierarchy();
        }

        public void SwitchGroup(Group group)
        {
            // assign the new current level.
            currentGroup = group;
        }

        public void AddToHierarchy(CustomShape shape)
        {
            currentGroup.AddChild(shape);

            Refresh();
        }

        public void RemoveFromHierarchy(CustomShape shape)
        {
            currentGroup.RemoveChild(shape);

            Refresh();
        }

        public void ClearStackPanel()
        {
            stackPanel.Children.Clear();
        }

        public void SetStackPanel(StackPanel stackPanel)
        {
            this.stackPanel ??= stackPanel;

            SwitchGroup(topGroup);
        }

        public void DeselectAllButtons()
        {
            // loop through all the buttons.
            foreach (SelectButton btn in stackPanel.Children)
            {
                // deselect them.
                btn.Deselect();
            }
        }

        private void Refresh()
        {
            // clear the stack panel.
            ClearStackPanel();
            // create 
            currentGroup.GetChildren().ForEach(x => stackPanel.Children.Add(new SelectButton(x)));
        }
    }
}


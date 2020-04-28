using System.Windows.Controls;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class Hierarchy
    {
        // the singleton of this class.
        private static Hierarchy instance;
        // the highest group in the hierarchy.
        private readonly Group topLevel;
        // the current selected group in the hierarchy.
        private Group currentLevel;
        // the stack panel on which shapes are displayed.
        private StackPanel stackPanel;

        private Hierarchy()
        {
            // assign the singleton.
            instance = this;
            // create a new group and assign it to the top level.
            topLevel = new Group();
            // the the current level equal to the top level.
            currentLevel = topLevel;
        }

        public Hierarchy GetInstance()
        {
            return instance ?? new Hierarchy();
        }

        public void SwitchLevel(Group group)
        {
            // assign the new current level.
            currentLevel = group;
            // clear the stack panel.
            ClearStackPanel();
            // create a select button for the 
            group.GetChildren().ForEach(x=>stackPanel.Children.Add(new SelectButton(x)));
        }

        public void ClearStackPanel()
        {
            stackPanel.Children.Clear();
        }

        public void SetStackPanel(StackPanel stackPanel)
        {
            this.stackPanel ??= stackPanel;
        }
    }
}


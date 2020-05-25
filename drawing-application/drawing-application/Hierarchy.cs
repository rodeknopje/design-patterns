using System.Collections.Generic;
using System.Windows.Controls;
using drawing_application.Buttons;
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
        // the save load manager which can save and load the program state.
        private readonly SaveLoadManager saveLoadManager;

        private Hierarchy()
        {
            // assign the singleton.
            instance = this;
            // initialize the save load manager.
            saveLoadManager = new SaveLoadManager();
            // load the previous state of the program.
            topGroup =  saveLoadManager.LoadProgramState();
            // the the current level equal to the top level.
            currentGroup = topGroup;

            topGroup.SetActive(true);

            MainWindow.ins.Closed += (a, b) => saveLoadManager.SaveProgramState();

        }

        public static Hierarchy GetInstance()
        {
            return instance ?? new Hierarchy();
        }

        public void SwitchGroup(Group group)
        {
            // assign the new current level.
            currentGroup = group;

            Refresh();
        }

        public void SwitchToTopLevel()
        {
            SwitchGroup(topGroup);
        }

        public void AddToHierarchy(CustomShape shape)
        {
            currentGroup.AddChild(shape);

            shape.SetActive(true);
            
            Refresh();
        }


        public void RemoveFromHierarchy(CustomShape shape)
        {
            currentGroup.RemoveChild(shape);

            shape.SetActive(false);

            Refresh();
        }

        private void ClearStackPanel()
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
            foreach (ShapeButton btn in stackPanel.Children)
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
            currentGroup.GetChildren().ForEach(x => stackPanel.Children.Add(x.GetType() == typeof(Group) ? new GroupButton(x) : new ShapeButton(x)));

            //saveLoadManager.SaveProgramState();
        }

        public void Clear()
        {
            // remove all the shapes from the canvas.
            MainWindow.ins.drawCanvas.Children.Clear();
            // clear the top group.
            topGroup.Clear();
            // refresh the hierarchy.
            Refresh();
        }

        public Group GetCurrentGroup()
        {
            return currentGroup;
        }

        public Group GetTopGroup()
        {
            return topGroup;
        }

        public List<CustomShape> GetChildrenInTopGroup()
        {
            return topGroup.GetChildren();
        }
    }
}


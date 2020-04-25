using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace drawing_application.Commands
{
    class SelectShapeCommand : Command
    {
        // the shapes which will be selected.
        private readonly List<CustomShape> shapes;

        public SelectShapeCommand(List<CustomShape> shapes) 
        {
            // assign the shapes.
            this.shapes = shapes;
        }

        // call the first constructor with a list with the given shape in it.
        public SelectShapeCommand(CustomShape shape) : this(new List<CustomShape>{shape}){}

        public override void Execute()
        {
            // switch to select state.
            m.SwitchState(states.select);
            // assign the selected shapes.
            m.selection.Select(shapes);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

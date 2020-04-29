using System.Windows;
using drawing_application.CustomShapes;
using System.Windows.Controls;
using System.Windows.Media;
using drawing_application.Commands;
using System.Windows.Input;

namespace drawing_application
{
    public class ShapeButton : TextBlock
    {
        // boolean that indicates if the shape is selected.
        private bool selected;
        // the shape which to Select when this button is selected.
        protected readonly CustomShape shape;
        // the color of this item when the shape is not selected.
        private const string DefaultColor = "#FFD3D3D3";
        // the color of this item when the shape is not selected.
        private const string SelectColor = "#FF87CEFA";

        public ShapeButton(CustomShape shape)
        {
            // assign the shape.
            this.shape = shape;
            // assign the correct text
            Text = $"{shape.GetType().Name}";
            // set the margin.
            Margin = new Thickness(0,0,0,1);
            // make the alignment stretch.
            HorizontalAlignment = HorizontalAlignment.Stretch;
            // set the height of this item.
            Height = 30;
            // set the font size.
            FontSize = 20;
            // set the background color.
            Background = new BrushConverter().ConvertFromString(DefaultColor) as SolidColorBrush;
            // set the foreground color
            Foreground = Brushes.Black;
            // invoke the onclick when this item is clicked.
            MouseLeftButtonDown += (a, b) => Onclick();
        }

        public void Onclick()
        {
            // if this control buttons is pressed.
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // if this button already is selected.
                if (selected)
                {
                    // remove the shape from the selection.
                    Selection.GetInstance().RemoveChild(shape);
                }
                else
                {
                    // otherwise add the shape to the selection.
                    new SelectShapeCommand(shape).Execute();
                }
            }   
            // when control is not pressed
            else
            {
                Selection.GetInstance().Clear();
                Hierarchy.GetInstance().DeselectAllButtons();
                // if the shape is not selected
                if (selected == false)
                {
                    // Select it.
                    new SelectShapeCommand(shape).Execute();
                }
            }

            // invert the selected boolean.
            selected = !selected;
            // set the background according to the selected boolean.
            Background = new BrushConverter().ConvertFromString(selected ? SelectColor : DefaultColor) as SolidColorBrush;
        }

        public void Deselect()
        {
            // return if this shape is currently selected.
            if (selected == false)
            {
                return;
            }
            // set the background to its default color.
            Background = new BrushConverter().ConvertFromString(DefaultColor) as SolidColorBrush;
            // reset the selected status.
            selected = false;
        }

        public bool GetSelectionStatus()
        {
            return selected;
        }
    }
}

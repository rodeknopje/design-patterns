using System.Windows;
using drawing_application.CustomShapes;
using System.Windows.Controls;
using System.Windows.Media;
using drawing_application.Commands;
using System.Windows.Input;

namespace drawing_application
{
    public class SelectButton : TextBlock
    {
        // id of this button.
        private static int id;
        // boolean that indicates if the shape is selected.
        private bool selected;
        // the shape which to select when this button is selected.
        private readonly CustomShape shape;
        // the color of this item when the shape is not selected.
        private const string DefaultColor = "#FFD3D3D3";
        // the color of this item when the shape is not selected.
        private const string SelectColor = "#FF87CEFA";

        public SelectButton(CustomShape shape)
        {
            // assign the shape.
            this.shape = shape;
            // assign the correct text
            Text = $"{shape.GetType().Name} ({id++})";
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
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (selected)
                {
                    //hier moet hij uitgezet worden
                    MainWindow.ins.selection.RemoveChild(shape);
                }
                else
                {
                    new SelectShapeCommand(shape).Execute();
                }
            }   
            else
            {
                MainWindow.ins.selection.Clear();

                if (selected == false)
                {
                    new SelectShapeCommand(shape).Execute();
                }
               
            }

            selected = !selected;

            Background = new BrushConverter().ConvertFromString(selected ? SelectColor : DefaultColor) as SolidColorBrush;

        }
    }
}

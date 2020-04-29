using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using drawing_application.Commands;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class GroupButton : ShapeButton
    {
        public GroupButton(CustomShape shape) : base(shape)
        {
            // add the amount of children this group has to the text.
            textBlock.Text += $" ({((Group)shape).GetChildren().Count})";

            // create 3 columns for this grid make the second one have the same width as the height of the grid
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1)});
            ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(Height)});
            // initialize the text block.
            var arrowText = new TextBlock
            {
                // make the text an arrow.
                Text = ">",
                // assign the text colors.
                Background = Brushes.LightGreen,
                Foreground = Brushes.Black,
                // assign the alignment.
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment       = TextAlignment.Center,
                // set the font size equal to the font size of the text block.
                FontSize = textBlock.FontSize
            };
            arrowText.MouseLeftButtonDown += (a, b) => MainWindow.ins.commandManager.InvokeCommand(new SwitchGroupCommand(shape as Group));
            // set the text block to the third column
            SetColumn(arrowText, 2);       
            // add the text block to the grid.
            Children.Add(arrowText);
        }
    }
}

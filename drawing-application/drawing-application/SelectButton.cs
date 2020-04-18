using System;
using System.Collections.Generic;
using System.Text;
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
        private static int ID;
        private bool selected;
        private CustomShape shape;
        public SelectButton(CustomShape shape)
        {
            this.shape = shape;
            // assign the correct text
            Text = $"{shape.GetType().Name} ({ID++})";
            //Margin = new Thickness(3);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            

            Height = 30;

            FontSize = 20;

            Background = Brushes.LightGray;

            Foreground = Brushes.Black;

            // if the text box is clicked then select the current shape
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

            Background = selected ? Brushes.LightSkyBlue : Brushes.LightGray;
        }
        



    }
}

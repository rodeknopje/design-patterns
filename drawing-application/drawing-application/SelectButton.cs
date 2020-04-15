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
    class SelectButton : Button

    {
        private static int ID;
        private bool selected;
        private CustomShape shape;
        public SelectButton(CustomShape shape)
        {
            this.shape = shape;
            // assign the correct text
            Content = $"{shape.GetType().Name} ({ID++})";
            Margin = new Thickness(1);
            HorizontalContentAlignment = HorizontalAlignment.Left;
            FontSize = 20;
            Foreground = Brushes.DarkRed;
            selected = false;

            // if the textbox is clicked then select the curren shape

            Click += (a, b) => Onclick(); 


        }


        public void Onclick()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (selected == true)
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
            Background = selected == true ? Brushes.LightSkyBlue : Brushes.Gray;
        }
        



    }
}

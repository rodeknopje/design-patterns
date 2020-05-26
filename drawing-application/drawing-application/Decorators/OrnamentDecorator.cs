using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using drawing_application.CustomShapes;

namespace drawing_application.Decorators
{
    public class OrnamentDecorator : ShapeDecorator
    {

        private TextBox[] textBlocks;


        public OrnamentDecorator(CustomShape shape, string left, string right, string top, string bottom) : base(shape)
        {
            textBlocks = new TextBox[4];

            textBlocks[0] = new TextBox{Text = left};
            textBlocks[1] = new TextBox{Text = right};
            textBlocks[2] = new TextBox{Text = top};
            textBlocks[3] = new TextBox{Text = bottom};
        }

        public void DisplayOrnaments(bool status)
        {
            Canvas.SetLeft(textBlocks[0], shape.GetLeft());
            Canvas.SetTop (textBlocks[0], shape.GetTop());


            if (status)
            {
                MainWindow.ins.drawCanvas.Children.Add(textBlocks[0]);
            }
        }
    }
}

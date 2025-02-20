﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using drawing_application.CustomShapes;
using System.Linq;
using drawing_application.Visitors;

namespace drawing_application.Decorators
{
    public class OrnamentDecorator : ShapeDecorator
    {

        private readonly TextBox[] ornaments;


        public OrnamentDecorator(CustomShape shape, string left, string right, string top, string bottom) : base(shape)
        {
            ornaments = new TextBox[4];

            ornaments[0] = new TextBox { Foreground = Brushes.Black, FontSize = 20, BorderThickness = new Thickness(0), Background = Brushes.LightGray, Text = left };
            ornaments[1] = new TextBox { Foreground = Brushes.Black, FontSize = 20, BorderThickness = new Thickness(0), Background = Brushes.LightGray, Text = right };
            ornaments[2] = new TextBox { Foreground = Brushes.Black, FontSize = 20, BorderThickness = new Thickness(0), Background = Brushes.LightGray, Text = top };
            ornaments[3] = new TextBox { Foreground = Brushes.Black, FontSize = 20, BorderThickness = new Thickness(0), Background = Brushes.LightGray, Text = bottom };
        }

        public override void Move(Point offset)
        {
            base.Move(offset);

            DisplayOrnaments(true);
        }

        public override void SetWidth(double width)
        {
            base.SetWidth(width);

            DisplayOrnaments(true);
        }

        public override void SetHeight(double height)
        {
            base.SetHeight(height);

            DisplayOrnaments(true);

        }

        public string[] GetOrnaments()
        {
            return ornaments.Select(x => x.Text).ToArray();
        }

        public override string Accept(IVisitor iVisitor)
        {
            return iVisitor.Visit(this);
        }

        public void DisplayOrnaments(bool status)
        {
            Canvas.SetLeft(ornaments[0], shape.GetLeft()-25);
            Canvas.SetTop (ornaments[0], shape.GetTop() + GetHeight() / 2);

            Canvas.SetLeft(ornaments[1], shape.GetLeft() + GetWidth());
            Canvas.SetTop (ornaments[1], shape.GetTop() + GetHeight() / 2);

            Canvas.SetLeft(ornaments[2], shape.GetLeft() + GetWidth() / 2);
            Canvas.SetTop (ornaments[2], shape.GetTop()-25);

            Canvas.SetLeft(ornaments[3], shape.GetLeft() + GetWidth() / 2);
            Canvas.SetTop (ornaments[3], shape.GetTop() + GetHeight());


            if (status && MainWindow.ins.shapeDrawn != this)
            {
                foreach (var text in ornaments)
                {
                    if (MainWindow.ins.drawCanvas.Children.Contains(text) == false )
                    {
                        MainWindow.ins.drawCanvas.Children.Add(text);
                    }
                }
            }
            else
            {
                foreach (var text in ornaments)
                {
                    MainWindow.ins.drawCanvas.Children.Remove(text);
                }
            }
        }




    }
}

﻿using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace drawing_application.Strategies
{
    public class Hexagon : IStrategyShape
    {
        public List<Point> Draw(CustomShape shape)
        {
           return new List<Point>
           {
                    new Point(0.25,1),
                    new Point(0.75,1),
                    new Point(1,.5),
                    new Point(.75,0),
                    new Point(.25,0),
                    new Point(0,.5),
           }
           .Select(i => new Point(i.X * shape.Width - shape.StrokeThickness, i.Y * shape.Height - shape.StrokeThickness)).ToList();
        }
    }
}

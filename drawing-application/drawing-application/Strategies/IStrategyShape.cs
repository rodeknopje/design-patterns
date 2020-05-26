using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using drawing_application.CustomShapes;
namespace drawing_application.Strategies
{
    public interface IStrategyShape
    {
        List<Point> Draw(CustomShape shape); 
        string ToString();
    }
}

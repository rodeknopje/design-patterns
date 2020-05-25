using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;

namespace drawing_application.Visitors
{
    public class WriteVisitor : IVisitor
    {
        private readonly int index;

        public WriteVisitor(int index)
        {
            this.index = index;
        }

        public string Visit(CustomShape shape)
        {
            return $"{new string(' ', index * 2)}{shape.GetType().Name} {(int)Canvas.GetLeft(shape)} {(int)Canvas.GetTop(shape)} {(int)shape.Width} {(int)shape.Height}";
        }

        public string Visit(Group shape)
        {
            var lines = new List<string>
            {
                $"{new string(' ', 2*index)}{shape.GetType().Name} {shape.GetChildren().Count}"
            };

            shape.GetChildren().ForEach(x => lines.Add(x.Accept(new WriteVisitor(index+1))));

            return string.Join('\n', lines);
        }
    }
}

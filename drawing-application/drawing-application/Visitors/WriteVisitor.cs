using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using drawing_application.Decorators;

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
            string[] positions = {"left", "right", "top", "bottom"};

            var indent = new string(' ',index*2);

            var ornaments = ((OrnamentDecorator)shape).GetOrnaments();

            var lines = new List<string>();

            for (var i = 0; i < positions.Length; i++)
            {
                if (ornaments[i] != string.Empty)
                {
                    lines.Add($"{indent}{positions[i]} \"{ornaments[i]}\"");
                }
            }

            lines.Add($"{new string(' ', index * 2)}{shape} {(int)shape.GetLeft()} {(int)shape.GetTop()} {(int)shape.GetWidth()} {(int)shape.GetHeight()}");

            return string.Join('\n',lines);
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

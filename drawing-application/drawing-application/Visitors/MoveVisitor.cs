using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Visitors
{
    public class MoveVisitor : IVisitor
    {
        private readonly Point offset;

        public MoveVisitor(Point offset)
        {
            this.offset = offset;
        }

        public string Visit(CustomShape shape)
        {

            // Move the shape based on their origin transform plus an offset.
            shape.Move(offset);
            // update the origin transform
            shape.UpdateOriginTransform();

            return "";
        }

        public string Visit(Group shape)
        {
            // Move all the children based on a offset.
            shape.GetChildren().ForEach(x => x.Move(offset));

            shape.UpdateOriginTransform();

            return "";
        }
    }
}

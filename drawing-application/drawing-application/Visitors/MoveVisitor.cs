using drawing_application.CustomShapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace drawing_application.Visitors
{
    public class MoveVisitor : IVisitor
    {
        public string Visit(CustomShape shape)
        {
            return "";
        }

        public string Visit(Group shape)
        {
            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace drawing_application.Visitors
{
    public interface IVisitor
    {
        public string Visit(CustomShapes.CustomShape shape);
        public string Visit(CustomShapes.Group shape);
    }
}

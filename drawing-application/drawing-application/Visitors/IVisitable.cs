using System;
using System.Collections.Generic;
using System.Text;

namespace drawing_application.Visitors
{
    public interface IVisitable
    {
        string Accept(IVisitor iVisitor);
    } 

}

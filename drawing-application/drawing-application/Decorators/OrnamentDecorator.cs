using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using drawing_application.CustomShapes;

namespace drawing_application.Decorators
{
    public class OrnamentDecorator : ShapeDecorator
    {
        public OrnamentDecorator(CustomShape shape, string left, string right, string top, string bottom) : base(shape)
        {
                
        }

        
    }
}

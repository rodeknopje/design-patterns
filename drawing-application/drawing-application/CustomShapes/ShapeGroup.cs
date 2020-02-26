using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.CustomShapes
{
    public abstract class ShapeGroup : Shape
    {
        protected override Geometry DefiningGeometry => InitializeGeometry();
        protected List<(float x, float y)> coords => InitializeCoords();

        private List<ShapeGroup> childeren = new List<ShapeGroup>();
        
        protected abstract Geometry InitializeGeometry( );
        protected abstract List<(float x, float y)> InitializeCoords();

        public void AddToGroup(ShapeGroup shape) => childeren.Add(shape);

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

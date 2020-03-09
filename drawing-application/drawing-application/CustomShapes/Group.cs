using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace drawing_application.CustomShapes
{
    public class Group : CustomShape
    {
        private List<CustomShape> childeren = new List<CustomShape>();

        protected override void DrawShape(out List<Point> coords) => coords = null;

        public void AddChild(CustomShape shape)
        {
            childeren.Add(shape);
        }

        public override void Move(Point offset)
        {
            foreach (var child in childeren)
            {
                child.Move(offset);
            }
        }

        public override void UpdateOrginPos()
        {
            foreach (var child in childeren)
            {
                child.UpdateOrginPos();
            }
        }

        public override void UpdateOrginScale()
        {
            foreach (var child in childeren)
            {
                child.UpdateOrginScale();
            }
        }
    }
}

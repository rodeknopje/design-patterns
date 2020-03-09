using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;

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

        public List<CustomShape> GetAllShapes()
        {
            var shapes = childeren.Where(T => T.GetType() != typeof(Group)).ToList();
            var groups = childeren.Where(T => T.GetType() == typeof(Group)).ToList();

            foreach (var group in groups)
            {
                shapes.AddRange(((Group)group).GetAllShapes());
            }

            return shapes;
        }

        public void RemoveChild(CustomShape shape)
        {
            childeren.Remove(shape);
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

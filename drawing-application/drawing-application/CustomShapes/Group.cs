using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using drawing_application.Visitors;

namespace drawing_application.CustomShapes
{
    public class Group : CustomShape
    {
        // all the custom shapes shapes which are children from this group
        private readonly List<CustomShape> children = new List<CustomShape>();

        // return null when asked for coords, since this object doesn't have visuals from itself..
        protected override void DrawShape(out List<Point> coords) => coords = null;



        public Group() : base(null)
        {
            Canvas.SetLeft(this, 0);
            Canvas.SetTop (this, 0);

            Width  = 0;
            Height = 0;
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public void AddChild(CustomShape shape)
        {
            // add a custom shape to the group.
            children.Add(shape);
        }

        public virtual void Clear()
        {
            // clear the children.
            children.Clear();
        }

        public virtual void RemoveChild(CustomShape shape)
        {
            // remove a child.
            children.Remove(shape);
        }

        public List<CustomShape> GetAllShapes()
        {
            // get all the custom shapes which are not a group.
            var shapes = children.Where(T => T.GetType() != typeof(Group)).ToList();
            // get all the groups.
            var groups = children.Where(T => T.GetType() == typeof(Group)).ToList();
            
            // loop trough all the groups.
            foreach (var group in groups)
            {
                // recursively get all the custom shapes and add it to the shapes list.
                shapes.AddRange(((Group)group).GetAllShapes());
            }
            // return all the shapes.
            return shapes;
        }

        public override void Move(Point offset)
        {
            // Move all the children based on a offset.
            children.ForEach(x=>x.Move(offset));
        }

        public override void UpdateOriginTransform()
        {
            // update the origin transform of all the children.
            children.ForEach(x=>x.UpdateOriginTransform());
        }

        public override void SetActive(bool state)
        {
            base.SetActive(state);

            children.ForEach(x=>x.SetActive(state));
        }

        public List<CustomShape> GetChildren() => children;

        public override string ToString(int level)
        {
            var lines = new List<string>
            {
                $"{new string(' ', 2*level++)}{GetType().Name} {GetChildren().Count}"
            };

            GetChildren().ForEach(x=>lines.Add(x.ToString(level)));

            return string.Join('\n', lines);
        }

        public override string Accept(IVisitor iVisitor)
        {
            return iVisitor.Visit(this);
        }
    }
}

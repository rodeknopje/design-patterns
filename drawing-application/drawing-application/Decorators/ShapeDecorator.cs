using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using drawing_application.CustomShapes;
using drawing_application.Visitors;

namespace drawing_application.Decorators
{
    public abstract class ShapeDecorator : CustomShape
    {
        protected readonly CustomShape shape;

        protected ShapeDecorator(CustomShape shape) : base(null)
        {
            this.shape = shape;
        }

        public override void UpdateOriginTransform()
        {
            shape.UpdateOriginTransform();
        }

        public override void SetLeft(double left)
        {
            shape.SetLeft(left);
        }

        public override void SetTop(double top)
        {
            shape.SetTop(top);
        }

        public override void SetWidth(double width)
        {
            shape.SetWidth(width);
        }

        public override void SetHeight(double height)
        {
            shape.SetHeight(height);
        }

        public override void Move(Point offset)
        {
            shape.Move(offset);
        }


        public override void SetActive(bool state)
        {
            shape.SetActive(state);
        }

        public override string Accept(IVisitor iVisitor)
        {
            return shape.Accept(iVisitor);
        }

        public override string ToString()
        {
            return shape.ToString();
        }

        public override double GetTop()
        {
            return shape.GetTop();
        }

        public override double GetLeft()
        {
            return shape.GetLeft();
        }

        public override double GetWidth()
        {
            return shape.GetWidth();
        }

        public override double GetHeight()
        {
            return shape.GetHeight();
        }

        public override Transform GetOriginTransform()
        {
            return shape.GetOriginTransform();
        }
    }
}

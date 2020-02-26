﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace drawing_application.CustomShapes
{
    class Star : ShapeGroup
    {
        protected override List<(float x, float y)> InitializeCoords()
        { 
            return  new List<(float x, float y)>
            {
                (.5f,0),
                (.625f,.4f),
                (1,.4f),
                (.69f,.625f),
                (.8f,1),
                (.5f,.775f),
                (.2f,1),
                (.31f,.625f),
                (0,.4f),
                (.375f,.4f),
            };
        }

        protected override Geometry InitializeGeometry()
        {
            StreamGeometry geom = new StreamGeometry();

            using (var gc = geom.Open())
            {
                gc.BeginFigure(new Point(Width * coords[0].x, Height * coords[0].y), true, true);

                for (int i = 1; i < coords.Count; i++)
                {
                    gc.LineTo(new Point(Width * coords[i].x, Height * coords[i].y), true, false);
                };
            }
            return geom;

        }
    }
}
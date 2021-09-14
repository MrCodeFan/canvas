using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace FoAP_Lab1_2.Figures
{
    class Triangle : Polygon
    {
        public Triangle(ArrayList points) : base(points)
        {

        }
        public Triangle(Point[] points) : base(points)
        {

        }
        public Triangle(Point point1, Point point2, Point point3) 
            : base(new Point[] { point1, point2, point3 } )
        {

        }
    }
}

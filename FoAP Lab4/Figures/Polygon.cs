using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace FoAP_Lab1_2.Figures
{
    class Polygon : Figure
    {
        public Polygon(Point[] points)
        {
            this.points.Clear();
            foreach(Point point in points)
            {
                this.points.Add(point);
            }
            changeRange();
        }
        public Polygon(ArrayList points)
        {
            this.points = points;
            changeRange();
        }

        public override void MoveTo(int x, int y)
        {
            int moveX = x - this.x,
                moveY = y - this.y;
            for(int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(((Point)points[i]).X + moveX, ((Point)points[i]).Y + moveY);
            }
            changeRange();
        }
    }
}

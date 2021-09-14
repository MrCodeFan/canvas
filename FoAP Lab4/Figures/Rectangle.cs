using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FoAP_Lab1_2.Figures
{
    class Rectangle:Figure
    {
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            changeRange();

            points.Add(new Point(x, y));
            points.Add(new Point(x, y + height));
            points.Add(new Point(x + width, y + height));
            points.Add(new Point(x + width, y));

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FoAP_Lab1_2.Figures
{
    class Ellipse : Figure
    {
        public Ellipse(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            changeRange();
        }
        public Ellipse()
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;

            changeRange();
        }
        public override void Draw(Bitmap bitmap)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawEllipse(pen, this.x, this.y, this.width, this.height);
        }
        public Point getCenterPoint()
        {
            return new Point(
                x + width/2, y + height/2
            );
        }
    }
}

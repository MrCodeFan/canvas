using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FoAP_Lab1_2.Figures
{
    
    class StrCentred: Figure
    {
        string str;
        public Font drawFont;
        public SolidBrush drawBrush;
        public StrCentred(int x, int y, string str)
        {
            this.x = x;
            this.y = y;
            this.str = str;

            drawFont = new Font("Arial", 10);
            drawBrush = new SolidBrush(Color.Black);
        }
        public override void Draw(Bitmap bitmap)
        {
            Graphics g = Graphics.FromImage(bitmap);

            g.DrawString(str, drawFont, drawBrush, new Point(x, y));
        }

        public override void changePen(Pen pen)
        {
            drawFont = new Font("Arial", pen.Width);
            drawBrush = new SolidBrush(pen.Color);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoAP_Lab1_2.Figures
{

    public class Range
    {
        private int startX, startY;
        private int endX, endY;
        public Range(int x, int y, int w, int h)
        {
            startX = x;
            startY = y;
            endX = x + w;
            endY = y + h;
        }

        public bool In(int x, int y)
        {
            if (
                x >= startX && y >= startY &&
                x <= endX && y <= endY
                )
            {
                return true;
            }

            return false;
        }
    }
}

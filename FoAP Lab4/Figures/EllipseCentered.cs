using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoAP_Lab1_2.Figures
{
    class EllipseCentered : Ellipse
    {
        public EllipseCentered(int x, int y, int w, int h) 
            : base(x - w / 2, y - h / 2, w, h)
        {

        }
    }
}

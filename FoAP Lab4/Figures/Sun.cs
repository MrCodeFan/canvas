using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace FoAP_Lab1_2.Figures
{
    class Sun : ComplexFigure
    {
        public Sun(int x, int y, int width, int height) : base()
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            gen();
            
            //base.setParams();
        }

        public void gen(int num = 8)
        {
            figures.Clear();

            double angel = 360 / (double) num;
            double onePieceW = width / (double)34;
            double onePieceH = height / (double)34;
            figures.Add(
                new Ellipse(
                    Convert.ToInt32(x + 7 * onePieceW),
                    Convert.ToInt32(y + 7 * onePieceH),
                    Convert.ToInt32(20 * onePieceW),
                    Convert.ToInt32(20 * onePieceH)
                )
            );
            //figures.Add(
            //    new Ellipse( 
            //        Convert.ToInt32( 7 * onePieceW ),
            //        Convert.ToInt32( 7 * onePieceH ),
            //        Convert.ToInt32( 20 * onePieceW ),
            //        Convert.ToInt32( 20 * onePieceH )
            //    )
            //);
            int centerX = x + width / 2;
            int centerY = y + height / 2;

            Point[] points = new Point[] { 
                new Point( x + width / 2, y ),
                new Point( Convert.ToInt32( x + width / 2 - 3 * onePieceW ), y + Convert.ToInt32( 6 * onePieceH ) ),
                new Point( Convert.ToInt32( x + width / 2 + 3 * onePieceW ), y + Convert.ToInt32( 6 * onePieceH ))
            };

            figures.Add(
                new Triangle( points )
            );

            for (int i = 1; i < num; i++)
            {
                Point[] pointsFor = new Point[3];
                double rotAngel = angel * i * Math.PI / 180;
                for (int ii = 0; ii < 3; ii++)
                {
                    pointsFor[ii] = new Point(
                        Convert.ToInt32(centerX + (points[ii].X - centerX) * Math.Cos(rotAngel) - (points[ii].Y - centerY) * Math.Sin(rotAngel)),
                        Convert.ToInt32(centerY + (points[ii].X - centerX) * Math.Sin(rotAngel) + (points[ii].Y - centerY) * Math.Cos(rotAngel))
                    ); 
                }
                
                figures.Add(
                    new Triangle(pointsFor)
                );
            }

            changeRange();

        }

        public override void MoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
            gen();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FoAP_Lab1_2.Figures
{
    class Clock : ComplexFigure
    {
        bool dinamic = true;
        int[] time = new int[3];
        int drawTime = 0;
        
        public Clock(int x, int y, int width) : base()
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = ( width * 46 ) / 31;
            setTime(DateTime.Now.Second, DateTime.Now.Minute + 1, DateTime.Now.Hour);
            dinamic = true;
            range = new Range(x, y, width, height);
        }
        public void gen(int sec = 0, int min = 0, int hour = 0)
        {
            figures.Clear();

            Figure tempFigure;

            double onePiece = width / 31;
            tempFigure = new Circle(
                    Convert.ToInt32(x + width / 2 - onePiece),
                    Convert.ToInt32(y),
                    Convert.ToInt32(2 * onePiece)
                );
            tempFigure.changePen(pen);
            figures.Add( tempFigure );
            Circle mainCircle = new Circle(
                x, Convert.ToInt32(y + 13 * onePiece), 
                Convert.ToInt32(32 * onePiece)
            );
            Point centerCircle = mainCircle.getCenterPoint();
            
            Point mainC = new Point(centerCircle.X, Convert.ToInt32(centerCircle.Y + 12*onePiece));

            mainCircle.changePen(pen);
            figures.Add(mainCircle);
            //figures.Add(new EllipseCentered(mainC.X, mainC.Y, Convert.ToInt32(5 * onePiece), Convert.ToInt32(5 * onePiece)));

            tempFigure = new EllipseCentered(centerCircle.X, centerCircle.Y, Convert.ToInt32(2 * onePiece), Convert.ToInt32(2 * onePiece));
            tempFigure.changePen( pen );
            figures.Add(tempFigure);

            tempFigure = new EllipseCentered(mainC.X, mainC.Y, Convert.ToInt32(5 * onePiece), Convert.ToInt32(5 * onePiece));
            // tempFigure.changePen(pen);
            figures.Add(tempFigure);
            tempFigure = new StrCentred(mainC.X - Convert.ToInt32(onePiece), mainC.Y - Convert.ToInt32(onePiece), "6");
            //tempFigure.changePen(new Pen(pen.Color, width/20));
            figures.Add(tempFigure);

            for (int i = 1; i < 12; i++)
            {
                double rotAngel = (360 * i * Math.PI) / (12 * 180);
                string strNum = (((i + 6) % 12>0)? (i + 6) % 12: 12).ToString();
                Point pointFor = new Point(
                    Convert.ToInt32(centerCircle.X + (mainC.X - centerCircle.X) * Math.Cos(rotAngel) - (mainC.Y - centerCircle.Y) * Math.Sin(rotAngel)),
                    Convert.ToInt32(centerCircle.Y + (mainC.X - centerCircle.X) * Math.Sin(rotAngel) + (mainC.Y - centerCircle.Y) * Math.Cos(rotAngel))
                );
                tempFigure = new EllipseCentered(pointFor.X, pointFor.Y, Convert.ToInt32(5 * onePiece), Convert.ToInt32(5 * onePiece));
                // tempFigure.changePen(pen);
                figures.Add(tempFigure);


                tempFigure = new StrCentred(pointFor.X - Convert.ToInt32(onePiece), pointFor.Y - Convert.ToInt32(onePiece), strNum);
                tempFigure.changePen(new Pen(tempFigure.pen.Color, width / 20));

                figures.Add(tempFigure);

            }

            mainC = new Point(mainC.X, Convert.ToInt32(mainC.Y + 6 * onePiece));
            double rotateAngel = (330 * Math.PI) / ( 180);
            Point pointP = new Point(
                Convert.ToInt32(centerCircle.X + (mainC.X - centerCircle.X) * Math.Cos(rotateAngel) - (mainC.Y - centerCircle.Y) * Math.Sin(rotateAngel)),
                Convert.ToInt32(centerCircle.Y + (mainC.X - centerCircle.X) * Math.Sin(rotateAngel) + (mainC.Y - centerCircle.Y) * Math.Cos(rotateAngel))
            );
            tempFigure = new EllipseCentered(pointP.X, pointP.Y, Convert.ToInt32(5 * onePiece), Convert.ToInt32(5 * onePiece));
            tempFigure.changePen(pen);
            figures.Add(tempFigure);

            rotateAngel = (30 * Math.PI) / (180);
            pointP = new Point(
                Convert.ToInt32(centerCircle.X + (mainC.X - centerCircle.X) * Math.Cos(rotateAngel) - (mainC.Y - centerCircle.Y) * Math.Sin(rotateAngel)),
                Convert.ToInt32(centerCircle.Y + (mainC.X - centerCircle.X) * Math.Sin(rotateAngel) + (mainC.Y - centerCircle.Y) * Math.Cos(rotateAngel))
            );

            tempFigure = new EllipseCentered(pointP.X, pointP.Y, Convert.ToInt32(5 * onePiece), Convert.ToInt32(5 * onePiece));
            tempFigure.changePen(pen);
            figures.Add(tempFigure);


            Point[][] pointLine = new Point[][] { new Point[4], new Point[4], new Point[4]};
            pointLine[0][0] = centerCircle;
            pointLine[0][1] = new Point(Convert.ToInt32(centerCircle.X - 0.75 * onePiece), Convert.ToInt32(centerCircle.Y - 2 * onePiece));
            pointLine[0][3] = new Point(Convert.ToInt32(centerCircle.X + 0.75 * onePiece), Convert.ToInt32(centerCircle.Y - 2 * onePiece));
            pointLine[0][2] = new Point(Convert.ToInt32(centerCircle.X), Convert.ToInt32(centerCircle.Y - 14 * onePiece)); 
            pointLine[1][0] = centerCircle;
            pointLine[1][1] = new Point(Convert.ToInt32(centerCircle.X - 2 * onePiece), Convert.ToInt32(centerCircle.Y - 4 * onePiece));
            pointLine[1][3] = new Point(Convert.ToInt32(centerCircle.X + 2 * onePiece), Convert.ToInt32(centerCircle.Y - 4 * onePiece));
            pointLine[1][2] = new Point(Convert.ToInt32(centerCircle.X), Convert.ToInt32(centerCircle.Y - 12 * onePiece));
            pointLine[2][0] = centerCircle;
            pointLine[2][1] = new Point(Convert.ToInt32(centerCircle.X - 1.5 * onePiece), Convert.ToInt32(centerCircle.Y - 3 * onePiece));
            pointLine[2][3] = new Point(Convert.ToInt32(centerCircle.X + 1.5 * onePiece), Convert.ToInt32(centerCircle.Y - 3 * onePiece));
            pointLine[2][2] = new Point(Convert.ToInt32(centerCircle.X), Convert.ToInt32(centerCircle.Y - 8 * onePiece));
            //figures.Add(new Polygon(pointLine));
            int[] arrayMM = new int[] { sec, min, hour%12 };
            int[] arrayDMM = new int[] { 60, 60, 12 };

            Color[] colorLines = new Color[] { Color.Red, pen.Color, pen.Color };

            for(int i = 0; i < 3; i++)
            {
                double rotA = ((arrayMM[i]*360)/arrayDMM[i] * Math.PI) / (180);
                for (int pointI = 0; pointI < 4; pointI++)
                {
                    pointLine[i][pointI] = new Point(
                        Convert.ToInt32(centerCircle.X + (pointLine[i][pointI].X - centerCircle.X) * Math.Cos(rotA) - (pointLine[i][pointI].Y - centerCircle.Y) * Math.Sin(rotA)),
                        Convert.ToInt32(centerCircle.Y + (pointLine[i][pointI].X - centerCircle.X) * Math.Sin(rotA) + (pointLine[i][pointI].Y - centerCircle.Y) * Math.Cos(rotA))
                    );
                }
                Figure figure = new Polygon(pointLine[i]);
                figure.changePen(new Pen(colorLines[i], 4));
                figures.Add(figure);
            }


        }
        public void setTime(int sec, int min, int hour)
        {
            time[0] = sec % 60;
            time[1] = min % 60;
            time[2] = hour % 12;
            dinamic = false;
            gen(time[0], time[1], time[2]);
        }
        public override void Draw(Bitmap bitmap)
        {
            if (dinamic) { gen(DateTime.Now.Second, DateTime.Now.Minute, DateTime.Now.Hour); }
            base.Draw(bitmap);
            float onePiece = width / 31;
            Graphics g = Graphics.FromImage(bitmap);
            if (dinamic && time[1] == DateTime.Now.Minute && (drawTime % 4 == 0 || drawTime % 5 == 0))
            {
                if (time[1] == DateTime.Now.Minute)
                {
                    if (drawTime % 4 == 0)
                    {
                        g.DrawArc(pen, x + 8 * onePiece, y + 2 * onePiece, 18 * onePiece, 18 * onePiece, 160, 180);
                        g.DrawLine(pen, Convert.ToInt32(x + 8 * onePiece), Convert.ToInt32(y + 13 * onePiece), Convert.ToInt32(x + 26 * onePiece), Convert.ToInt32(y + 8 * onePiece));
                    }
                    else
                    {
                        g.DrawArc(pen, x + 8 * onePiece, y + 2 * onePiece, 18 * onePiece, 18 * onePiece, 200, 175);
                        g.DrawLine(pen, Convert.ToInt32(x + 8 * onePiece), Convert.ToInt32(y + 8 * onePiece), Convert.ToInt32(x + 26 * onePiece), Convert.ToInt32(y + 14 * onePiece));
                    }

                    g.DrawLine(pen, Convert.ToInt32(x + 15 * onePiece), Convert.ToInt32(y + 11 * onePiece), Convert.ToInt32(x + 15 * onePiece), Convert.ToInt32(y + 13 * onePiece));
                    g.DrawLine(pen, Convert.ToInt32(x + 18 * onePiece), Convert.ToInt32(y + 11 * onePiece), Convert.ToInt32(x + 18 * onePiece), Convert.ToInt32(y + 13 * onePiece));

                    g.DrawLine(pen, Convert.ToInt32(x + 6 * onePiece), Convert.ToInt32(y + 5 * onePiece), Convert.ToInt32(x + 2 * onePiece), Convert.ToInt32(y + 2 * onePiece));
                    g.DrawLine(pen, Convert.ToInt32(x + 28 * onePiece), Convert.ToInt32(y + 5 * onePiece), Convert.ToInt32(x + 32 * onePiece), Convert.ToInt32(y + 2 * onePiece));
                    g.DrawLine(pen, Convert.ToInt32(x + 5 * onePiece), Convert.ToInt32(y + 9 * onePiece), Convert.ToInt32(x + 1 * onePiece), Convert.ToInt32(y + 7 * onePiece));
                    g.DrawLine(pen, Convert.ToInt32(x + 27 * onePiece), Convert.ToInt32(y + 9 * onePiece), Convert.ToInt32(x + 31 * onePiece), Convert.ToInt32(y + 7 * onePiece));

                }
            } else
            {
                g.DrawArc(pen, x + 8 * onePiece, y + 2 * onePiece, 18 * onePiece, 18 * onePiece, 180, 180);
                g.DrawLine(pen, Convert.ToInt32(x + 8 * onePiece), Convert.ToInt32(y + 11 * onePiece), Convert.ToInt32(x + 26 * onePiece), Convert.ToInt32(y + 11 * onePiece));

                g.DrawLine(pen, Convert.ToInt32(x + 15 * onePiece), Convert.ToInt32(y + 11 * onePiece), Convert.ToInt32(x + 15 * onePiece), Convert.ToInt32(y + 13 * onePiece));
                g.DrawLine(pen, Convert.ToInt32(x + 18 * onePiece), Convert.ToInt32(y + 11 * onePiece), Convert.ToInt32(x + 18 * onePiece), Convert.ToInt32(y + 13 * onePiece));
            }
            drawTime += 1;
            
        }

        public override void MoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;

            gen(DateTime.Now.Second, DateTime.Now.Minute + 1, DateTime.Now.Hour);
            changeRange();
        }
    }

    
}

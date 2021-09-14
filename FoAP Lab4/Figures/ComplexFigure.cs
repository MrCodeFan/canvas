using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FoAP_Lab1_2.Figures
{
    class ComplexFigure: Figure
    {
        public ArrayList figures;

        public ComplexFigure(Figure[] figures)
        {
            this.figures = new ArrayList();
            foreach (Figure figure in figures)
            {
                this.figures.Add(figure);
            }
            setParams();
        }
        public ComplexFigure(ArrayList figures)
        {
            this.figures = figures;
            //setParams();
            changeRange();
        }
        public ComplexFigure()
        {
            this.figures = new ArrayList();
        }
        public void setParams()
        {
            int minX = ((Figure)figures[0]).x, minY = ((Figure)figures[0]).y,
                maxX = ((Figure)figures[0]).x, maxY = ((Figure)figures[0]).y;
            //Console.WriteLine("minX = {0}\nminY = {1}\nmaxX = {2}\nmaxY = {3}\n----------------", minX, minY, maxX, maxY);
            foreach (Figure figure in figures)
            {
                if (figure.points.Count == 0)
                {
                    if (minX > figure.x) { minX = figure.x; }
                    if (minY > figure.y) { minY = figure.y; }
                    if (maxX < figure.x + figure.width) { maxX = figure.x + figure.width; }
                    if (maxY < figure.y + figure.height) { maxY = figure.y + figure.height; }
                }
                else
                {
                    foreach (Point point in figure.points)
                    {
                        if (minX > point.X) { minX = point.X; }
                        if (minY > point.Y) { minY = point.Y; }
                        if (maxX < point.X) { maxX = point.X; }
                        if (maxY < point.Y) { maxY = point.Y; }
                    }
                }
            }
            Console.WriteLine("minX = {0}\nminY = {1}\nmaxX = {2}\nmaxY = {3}\n", minX, minY, maxX, maxY);
            x = minX;
            y = minY;
            width = maxX - minX;
            height = maxY - minY;
        }
        public override void changeRange()
        {
            setParams();
            range = new Range(x, y, width, height);
        }
        public override void Draw(Bitmap bitmap)
        {
            foreach(Figure figure in figures)
            {
                figure.Draw(bitmap);
            }
        }
        public override void MoveTo(int x, int y)
        {
            int moveX = x - this.x,
                moveY = y - this.y;

            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(
                    ((Point)points[i]).X + moveX,
                    ((Point)points[i]).Y + moveY
                );
            }

            for (int i = 0; i < figures.Count; i++)
            {
                ((Figure)figures[i]).MoveTo(
                    ((Figure)figures[i]).x + moveX,
                    ((Figure)figures[i]).y + moveY
                );
            }
            changeRange();
        }

        public override void changePen(Pen pen)
        {
            this.pen = pen;
            foreach(Figure figure in figures)
            {
                figure.changePen(pen);
            }
        }
    }
}

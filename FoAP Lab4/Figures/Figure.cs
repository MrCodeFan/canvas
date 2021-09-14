using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoAP_Lab1_2.Figures
{
    abstract public class Figure
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public string Name { get; set; }
        
        public Range range;

        public Pen pen = new Pen(Color.Black, 5);

        public ArrayList points = new ArrayList();
        public bool close = true;

        virtual public void Draw(Bitmap bitmap)
        {
            Graphics g = Graphics.FromImage(bitmap);
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                {
                    g.DrawLine(pen, (Point)points[i], (Point)points[i + 1]);
                }
            }
            if (close)
            {
                g.DrawLine(pen, (Point)points[points.Count - 1], (Point)points[0]);
            }
        }
        virtual public void MoveTo(int x, int y)
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
            changeRange();
        }

        public virtual void changeRange()
        {
            if (points.Count == 0)
            {
                range = new Range(x, y, width, height);
            } else
            {
                int minX = ((Point)points[0]).X, minY = ((Point)points[0]).Y,
                    maxX = ((Point)points[0]).X, maxY = ((Point)points[0]).Y;
                foreach(Point point in points)
                {
                    if (minX > point.X) { minX = point.X; }
                    if (minY > point.Y) { minY = point.Y; }
                    if (maxX < point.X) { maxX = point.X; }
                    if (maxY < point.Y) { maxY = point.Y; }
                }
                x = minX;
                y = minY;
                width = maxX - minX;
                height = maxY - minY;
                range = new Range(minX, minY, maxX - minX, maxY - minY);
            }
        }

        public virtual void changePen(Pen pen)
        {
            this.pen = pen;
        }

        public void Select(Bitmap bitmap)
        {
            Graphics g = Graphics.FromImage(bitmap);
            //g.DrawLine(
            //    Settings.selectPen,
            //    new Point(Convert.ToInt32(x - pen.Width / 2), Convert.ToInt32(y - pen.Width / 2)),
            //    new Point(Convert.ToInt32(x + width + pen.Width / 2), Convert.ToInt32(y - pen.Width / 2))
            //);
            //g.DrawLine(
            //    Settings.selectPen,
            //    new Point(Convert.ToInt32(x + width + pen.Width / 2), Convert.ToInt32(y - pen.Width / 2)),
            //    new Point(Convert.ToInt32(x + width + pen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2))
            //);
            //g.DrawLine(
            //    Settings.selectPen,
            //    new Point(Convert.ToInt32(x + width + pen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2)),
            //    new Point(Convert.ToInt32(x - pen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2))
            //);
            //g.DrawLine(
            //    Settings.selectPen,
            //    new Point(Convert.ToInt32(x - pen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2)),
            //    new Point(Convert.ToInt32(x - pen.Width / 2), Convert.ToInt32(y - pen.Width / 2))
            //);

            //Point[] points = new Point[]{
            //    new Point(Convert.ToInt32(x - pen.Width / 2 - Settings.selectPen.Width), Convert.ToInt32(y - pen.Width / 2 - Settings.selectPen.Width / 2)),
            //    new Point(Convert.ToInt32(x + width + pen.Width / 2 + Settings.selectPen.Width), Convert.ToInt32(y - pen.Width / 2 - Settings.selectPen.Width / 2)),
            //    new Point(Convert.ToInt32(x + width + pen.Width / 2 + Settings.selectPen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2)),
            //    new Point(Convert.ToInt32(x - pen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2 + Settings.selectPen.Width / 2))
            //};
            //for (int i = 0; i < points.Length; i++)
            //{
            //    g.DrawLine(
            //        Settings.selectPen,
            //        points[i],
            //        points[ (i + 1 < points.Length)? i + 1 : 0 ]
            //    );
            //}
            g.DrawLine(
                Settings.selectPen,
                new Point(Convert.ToInt32(x - pen.Width / 2 - Settings.selectPen.Width), Convert.ToInt32(y - pen.Width / 2 - Settings.selectPen.Width / 2)),
                new Point(Convert.ToInt32(x + width + pen.Width / 2 + Settings.selectPen.Width), Convert.ToInt32(y - pen.Width / 2 - Settings.selectPen.Width / 2))
            );
            g.DrawLine(
                Settings.selectPen,
                new Point(Convert.ToInt32(x + width + pen.Width / 2 + Settings.selectPen.Width / 2), Convert.ToInt32(y - pen.Width / 2 - Settings.selectPen.Width)),
                new Point(Convert.ToInt32(x + width + pen.Width / 2 + Settings.selectPen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2 + Settings.selectPen.Width))
            );
            g.DrawLine(
                Settings.selectPen,
                new Point(Convert.ToInt32(x + width + pen.Width / 2 + Settings.selectPen.Width), Convert.ToInt32(y + height + pen.Width / 2 + Settings.selectPen.Width / 2)),
                new Point(Convert.ToInt32(x - pen.Width / 2 - Settings.selectPen.Width), Convert.ToInt32(y + height + pen.Width / 2 + Settings.selectPen.Width / 2))
            );
            g.DrawLine(
                Settings.selectPen,
                new Point(Convert.ToInt32(x - pen.Width / 2 - Settings.selectPen.Width / 2), Convert.ToInt32(y + height + pen.Width / 2 + Settings.selectPen.Width)),
                new Point(Convert.ToInt32(x - pen.Width / 2 - Settings.selectPen.Width / 2), Convert.ToInt32(y - pen.Width / 2 - Settings.selectPen.Width))
            );
        }
    }


}

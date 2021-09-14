using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;

namespace FoAP_Lab1_2.Figures
{
    static class ShapeContainer
    {
        static public ArrayList container = new ArrayList();
        static public ArrayList visibleNums = new ArrayList();

        static public void start()
        {
            Figure[] figures = new Figure[] {
                //new Sun(0, 0, 300, 300),
                new Clock(400, 50, 350),
                new Clock(120, 350, 150),
                //new Circle(300, 300, 20),
                new Sun( 10, 10, 200, 200),
                new Polygon(
                    new Point[] {
                        new Point(250, 100),
                        new Point(250, 200),
                        new Point(300, 250),
                        new Point(350, 300),
                        new Point(350, 100)
                    }
                )
            };

            for (int i = 0; i < figures.Length; i++)
            {
                figures[i].Name = String.Format("{0}. {1}", i, figures[i].GetType().Name);
                figures[i].changeRange();
            }

            addFigure( figures );

        }
        static public void load()
        {
            
        }

        static public void Draw(Bitmap bitmap)
        {
            foreach (int num in visibleNums)
            {
                Figure figure = (Figure)container[num];
                figure.Draw(bitmap);
            }
        }
        static public void DrawAll(Bitmap bitmap)
        {
            foreach (Figure figure in container)
            {
                figure.Draw(bitmap);
            }
        }

        static public void addVisible(int num) {
            if (num >= 0 && num < container.Count)
            {
                if (!visibleNums.Contains(num))
                {
                    visibleNums.Add(num);
                }
            }
        }
        static public void delVisible(int num)
        {
            if (visibleNums.Contains(num))
            {
                ArrayList newVisible = new ArrayList();
                foreach (int v in visibleNums)
                {
                    if (v != num)
                    {
                        newVisible.Add(v);
                    }
                }
                visibleNums = newVisible;
            }
        }
        static public void delFigure(int num)
        {
            delVisible(num);
            if (container.Count > num)
            {
                container.RemoveAt(num);
            }
            for( int i = 0; i < visibleNums.Count; i++ )
            {
                int n = (int)visibleNums[i];
                if (n >= num)
                {
                    n = n - 1;
                }
                visibleNums[i] = n;
            }
        }
        static public void delFigure(Figure figure)
        {
            if (container.Contains(figure))
            {
                int index = container.IndexOf(figure);
                delFigure(index);
            }
        }

        static public void addFigure(Figure figure)
        {
            //if (!container.Contains(figure))
            //{
            container.Add(figure);
            addVisible(container.Count - 1);
            Console.WriteLine("{0}", visibleNums[0]);
        }
        static public void addFigure(Figure[] figures)
        {
            foreach(Figure figure in figures)
            {
                addFigure(figure);
            }
        }

        static public bool nameTaken(string name)
        {
            foreach(Figure figure in container)
            {
                if (figure.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        static public ArrayList searchByCoords(int x, int y)
        {
            ArrayList figures = new ArrayList();

            foreach(int num in visibleNums)
            {
                if (((Figure)ShapeContainer.container[num]).range.In(x, y))
                {
                    figures.Add((Figure)ShapeContainer.container[num]);
                }
            }

            return figures;
        }
        static public ArrayList searchByCoordsAll(int x, int y)
        {
            ArrayList figures = new ArrayList();

            foreach (Figure figure in ShapeContainer.container)
            {
                if (figure.range.In(x, y))
                {
                    figures.Add(figure);
                }
            }

            return figures;
        }
        static public void selectByCoords(int x, int y, Bitmap bitmap, bool all = false)
        {
            ArrayList figures;
            if (all) { figures = searchByCoordsAll(x, y); }
            else { figures = searchByCoords(x, y); }

            foreach(Figure figure in figures)
            {
                figure.Select(bitmap);
            }
        }
        static public int indexByName(string name)
        {
            int index = -1;
            for(int i = 0; i < container.Count; i++ )
            {
                Figure figure = (Figure)ShapeContainer.container[i];
                if (figure.Name.Equals(name)) { index = i;  }
            }

            return index;
        }

        static public void changePen(Pen pen)
        {
            foreach(Figure figure in container)
            {
                figure.changePen(pen);
            }

        }
    }


}

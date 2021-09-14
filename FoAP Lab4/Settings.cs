using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FoAP_Lab1_2
{
    public static class Settings
    {
        public static Color paramsSelectedColor = Color.FromArgb(255, 168, 228, 160);
        public static Color paramsStandartColor = SystemColors.ControlDark;

        public static int[] createBox = new int[] { 390, 230 };
        public static int[] createBoxLocation = new int[] { 120, 120 + (createBox[0] - createBox[1])/2 };

        public static int[] collectionBox = new int[] { 370, 200 };
        public static int[] collectionBoxLocation = new int[] { 110, 110 + (collectionBox[0] - collectionBox[1]) / 2 };

        public static Color selectColor = Color.FromArgb(255, 34, 245, 164);
        public static Pen selectPen = new Pen(selectColor, 2);
        public static Pen movePen = new Pen(Color.Bisque, 1);

        public static bool realTimeSelect = true;
        public static bool realTimeAngelLines = true;
        public static bool realTimeSideLines = true;

        public static Color formLabelUnselectColor = Color.FromArgb(255, 150, 150, 150);
        public static Color formLabelSelectColor = Color.FromArgb(255, 250, 176, 122);

        public static Color[] editSelectedButtonColor = new Color[] {
            Color.FromArgb(255, 250, 176, 122),
            Color.FromArgb(255, 250, 176, 122),
            Color.FromArgb(255, 250, 176, 122)
        };

    }
}

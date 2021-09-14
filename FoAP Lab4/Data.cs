using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Collections;

namespace FoAP_Lab1_2
{
    public class Option {
        public int id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public double dependenceW { get; set; }
        public double dependenceH { get; set; }
        public ArrayList include = new ArrayList();

        //public Option(string name, string parent, double dependenceW, double dependenceH)
        //{
        //    this.name = name;
        //    this.parent = parent;
        //    this.dependenceW = dependenceW;
        //    this.dependenceH = dependenceH;
        //}
    }
    public static class Data
    {
        public static int i;
        public static string strObjects;
        public static Option[] Options = new Option[0];
        public static void input()
        {
            try
            {
                string fileName = "shapes.json";
                string fileData = "";
                foreach (string str in System.IO.File.ReadAllLines(fileName))
                {
                    fileData += str + "";
                }
                Console.Write(fileData);
            }
            catch { }



            Option option = new Option
            {
                id = 1,
                name = "Name",
                parent = "Parent",
                dependenceW = 1,
                dependenceH = 1
            };

            string json = JsonSerializer.Serialize<Option>(option);
            Console.WriteLine("Test JSON:\n{0}\n----------------\n", json);
        }
        public static void output()
        {
            string res = "[ ";
            foreach (Option option in Options)
            {
                res += JsonSerializer.Serialize<Option>(option) + ", ";
            }
            res += " ]";
        }
    }
}

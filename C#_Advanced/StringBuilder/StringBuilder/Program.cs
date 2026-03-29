using System;
using System.Text;
using System.Diagnostics;

namespace StringBuildert
{
    internal class Program
    {
        public static void concatToString(int iterations)
        {
            string s = "";
            for (int i = 0; i < iterations; i++)
            {
                s += "a";
            }
        }

        public static void concatToStringBuilder(int iterations)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iterations; i++)
            {
                sb.Append("a");
            }
            string result = sb.ToString();
        }

        static void Main(string[] args)
        {
            int iterations = 400000;

            Stopwatch sw = new Stopwatch();

            // Measure String
            sw.Start();
            concatToString(iterations);
            sw.Stop();
            Console.WriteLine("String time: " + sw.ElapsedMilliseconds + " ms");

            // Reset stopwatch
            sw.Reset();

            // Measure StringBuilder
            sw.Start();
            concatToStringBuilder(iterations);
            sw.Stop();
            Console.WriteLine("StringBuilder time: " + sw.ElapsedMilliseconds + " ms");
        }
    }
}
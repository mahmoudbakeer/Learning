using System.Reflection;

namespace OperatorOverLoading
{
    internal class Program
    {
        public class Point {

            public int x { get; }
            public int y { get; }

            public Point(int p1 , int p2)
            {
                x = p1;
                y = p2;
            }
            

            public void Print()
            {
                Console.WriteLine($" x = {this.x} , y = {this.y}");
            }

            // operator overloading for specific objects
            public static Point operator +(Point p1 , Point p2)
            {
                return new Point(p1.x + p2.x, p1.y + p2.y);
            }
            public static Point operator -(Point p1, Point p2)
            {
                return new Point(p1.x - p2.x, p1.y - p2.y);
            }

            //when ever you make overload for == operator you should make one for != or it will throw an error
            public static bool operator ==(Point p1, Point p2)
            {
                return (p1.x == p2.x &&  p1.y == p2.y);
            }
            public static bool operator !=(Point p1, Point p2)
            {
                return (p1.x != p2.x || p1.y != p2.y);
            }

        }

        static void Main(string[] args)
        {


            Point p1 = new Point(1,2);
            Point p2 = new Point(3,5);
            p1.Print();
            p2.Print();

            Point res = p1 + p2;
            Console.WriteLine(res);
            res.Print();


            res = p1 - p2;
            Console.WriteLine(res);
            res.Print();

            if (p1 == p2) Console.WriteLine("They are equal ");
            else if (p1 != p2) Console.WriteLine("They are not equal");
        }
    }
}

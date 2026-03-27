namespace Nullabledatatype
{
    internal class Program
    {
        public static void Procedures1(string name, int? age)
        {
            Console.WriteLine($"the name is : {name}");
            Console.WriteLine($"the age is : {age}");
        }
        public static void Procedures1(string? name, int age)
        {
            Console.WriteLine($"the name is : {name}");
            Console.WriteLine($"the age is : {age}");
        }
 
        static void Main(string[] args)
        {
            Nullable<int> Nage = null;
            // the Nullable<ValueType> only works with value datatype not reference and string is refrence 
            string? Nname = null;


            int age = 21;
            string name = "mahmoud";
            Console.WriteLine("age is nullable : ");
            Procedures1(name, Nage);
            Console.WriteLine("Name is nullable : ");
            Procedures1(Nname, age);
        }
    }
}

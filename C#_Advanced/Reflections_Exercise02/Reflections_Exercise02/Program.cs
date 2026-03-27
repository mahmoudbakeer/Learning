using System.Reflection;
using System.Threading.Channels;

namespace Reflections_Exercise02
{
    public class MyClass
    {
        public int MyProperty { get; set; }
        public string MyName { get; set; }
        public static void method1()
        {
            Console.WriteLine("This is method 1..............");
        }
        public  void method2(int a, string b)
        {
            Console.WriteLine($"This is the method two with parameters a = {a} and b = {b}......");
        }
    }
    internal class Program
    {
        internal static string GetParameterList(ParameterInfo[] parameters)
        {
            return string.Join(", " , parameters.Select(p => $"Parameter  = {p.ParameterType} and {p.Name}"));
        }

        static void Main(string[] args)
        {
            Type myclass = typeof(MyClass);
            Console.WriteLine();
            Console.WriteLine("the class basic informations : ");
            Console.WriteLine($"Type name  = {myclass.Name}");
            Console.WriteLine($"Type Full name  = {myclass.FullName}");


            Console.WriteLine($"The Properties : ");
            // Inspecting the MyClass properties using the reflector myclass
            foreach (var item in myclass.GetProperties())
            {
                Console.WriteLine($"property name is {item.Name} and property type is  {item.PropertyType}");
            }

            // Inspecting the MyClass methods using the reflector myclass
            Console.WriteLine($"The Methods : ");
            foreach (var item in myclass.GetMethods())
            {
                Console.WriteLine($"method name is {item.Name} and method type is  {item.MemberType}");
            }


            // activator to create an instance of MyClass class
            object myclassobject = Activator.CreateInstance(myclass);

            // access to the property and setting a value of MyProperty in Myclass through Type.GetProperty()
            myclass.GetProperty("MyProperty").SetValue(myclassobject, 4);
            Console.WriteLine($"The new instantiated object of MyClass is 4 now");


            // the Getting of the property value through a Type.GetProperty
            Console.WriteLine($"Getting the instance the setted value : ");
            int value = (int)myclass.GetProperty("MyProperty").GetValue(myclassobject);
            Console.WriteLine($"The value is {value}");



            // Invoking the method2 
            object[] parameters = { 4 , "See me I'm right here"};
            myclass.GetMethod("method2").Invoke(myclassobject , parameters);
            Console.WriteLine();
            Console.WriteLine();

            
        }
    }
}

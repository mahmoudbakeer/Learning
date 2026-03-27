using System;
using System.Reflection;

namespace CustomAttributeWithReflections
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    class MyCustomAttribute : Attribute
    {
        public string Description { get; }

        public MyCustomAttribute(string description)
        {
            Description = description;
        }
    }

    [MyCustom("This is the class description")]
    internal class Program
    {
        [MyCustom("This is the method1 description")]
        public void Method1()
        {
            Console.WriteLine("The method1 executed");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Type type = typeof(Program);

            // perform reading on the class attributes
            var classAttributes = type.GetCustomAttributes(typeof(MyCustomAttribute), false);
            foreach (MyCustomAttribute attr in classAttributes)
            {
                Console.WriteLine($"Class Attribute Description: {attr.Description}");
            }

            // perform reading on the class methods
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                var methodAttributes = method.GetCustomAttributes(typeof(MyCustomAttribute), false);
                foreach (MyCustomAttribute attr in methodAttributes)
                {
                    Console.WriteLine($"Method {method.Name} Attribute Description: {attr.Description}");
                }
            }

            // create a instance of this class
            Program obj = new Program();
            obj.Method1();
        }
    }
}
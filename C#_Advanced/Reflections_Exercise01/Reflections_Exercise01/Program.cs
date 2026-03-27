using System;
using System.Linq;
using System.Reflection;

// Reflection is a powerful feature in C# that allows you to inspect and interact with types at '''runtime'''.
// In this exercise, we will use reflection to explore the System.String class
// and list all of its public methods along with their return types and parameters.
class Program
{
    static void Main()
    {
        // Get the assembly containing the System.String type
        Assembly mscorlib = typeof(string).Assembly;


        // Get the System.String type
        Type stringType = mscorlib.GetType("System.String");


        if (stringType != null)
        {
            Console.WriteLine($"Methods of the System.String class:\n");


            // Get all public methods of the System.String class
            var stringMethods = stringType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy((method) => method.Name);


            foreach (var method in stringMethods)
            {
                Console.WriteLine($"\t{method.ReturnType} {method.Name}({GetParameterList(method.GetParameters())})");
            }
        }
        else
        {
            Console.WriteLine("System.String type not found.");
        }


        Console.ReadKey();


    }

    // this method to navigate throw all the parameters of specific class method 
    static string GetParameterList(ParameterInfo[] parameters)
    {
        return string.Join(", ", parameters.Select(parameter => $"{parameter.ParameterType} {parameter.Name}"));
    }
}
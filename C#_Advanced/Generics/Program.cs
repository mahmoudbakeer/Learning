namespace Generics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("With strings : ");
            Person<string> person1 = new Person<string>("First Value " , "John");
            person1.Display();
            Console.WriteLine("With integers : ");
            Person<int> person2 = new Person<int>(25 ,"Hani");
            person2.Display();


            IGenericInterface<string> nonGenericClass = new GenericClass<string>();
            nonGenericClass.Display("Hello from non generic class that implements generic interface");
            IGenericInterface<string> genericClass = new NonGenericClass();
            genericClass.Display("Hello from generic class that implements non generic interface");

            Engineer engineer = new Engineer();
            // now about constraints in clss , IEmployee interface meanse that the T must be class and also one of IEmployee Implementaion
            CompanyEntrance<Engineer> t = new CompanyEntrance<Engineer>();
            CompanyEntrance<Engineer>.Display(engineer); // now that works 
        }
    }
    class Person<T>
    {
        public T Value { get;  }
        public string Name{ get; } // generic class can have non generic members as well
        public Person(T value, string Name)
        {
            Value = value;
            this.Name = Name;   
        }

        public void Display()
        {
            Console.WriteLine($"Value is : {Value}");
            Console.WriteLine($"Name is : {Name}");
        }
    }
    interface IGenericInterface<T>
    {
        void Display(string value);
    }
    // we can have a generic interface and a non generic class that implements it but the implementation of the method should be non generic as well
    class NonGenericClass : IGenericInterface<string> // pass the data type that we want to use with the generic interface
    {
        public void Display(string value)
        {
            Console.WriteLine($"Value is : {value}");
        }
    }
    public class GenericClass<T> : IGenericInterface<string> // the implementaion of the class should be generic as well
    {
        //public void Display(T value)
        //{
        //    Console.WriteLine($"Value is : {value}");
        //}

        public void Display(string value)
        {
            Console.WriteLine($"Value is : {value}");
        }
    }

    // we can also have constraints on the generic type parameter to restrict the types that can be used with the generic class or interface
    class ConstrainedGenericClass<T> where T : IComparable<T> // this means that the type parameter T must implement the IComparable<T> interface
    {

        // what are the constraints and how we can use them in our code
        /*
         * the first constraint is the where T : IComparable<T> which means that the type parameter T must implement the IComparable<T> interface. This allows us to use the CompareTo method of the IComparable<T> interface in our code to compare two values of type T.
         * the second constraint is the where T : class which means that the type parameter T must be a reference type. This allows us to use the null value with the type parameter T in our code.
         * the third constraint is the where T : struct which means that the type parameter T must be a value type. This allows us to use the default value of the type parameter T in our code.
         * the fourth constraint is the where T : new() which means that the type parameter T must have a parameterless constructor. This allows us to create an instance of the type parameter T in our code using the new keyword.
         * the fifth constraint is the where T : U which means that the type parameter T must be a derived class of the type parameter U. This allows us to use the members of the type parameter U in our code when we are working with the type parameter T.
         * the sixth constraint is the where T : unmanaged which means that the type parameter T must be an unmanaged type. This allows us to use the type parameter T in unsafe code and to use pointers with the type parameter T in our code.
         * the seventh constraint is the where T : notnull which means that the type parameter T must be a non-nullable type. This allows us to use the type parameter T in our code without worrying about null reference exceptions.
         * the eighth constraint is the where T : default which means that the type parameter T must have a default value. This allows us to use the default value of the type parameter T in our code without worrying about uninitialized variables.
         * and the rest are the combinations of the above constraints which can be used to further restrict the types that can be used with the generic class or interface.
         * the most important ones are the where T : IComparable<T> and the where T : class and the where T : struct and the where T : new() constraints as they are commonly used in real world scenarios to ensure that the types used with the generic class or interface have certain capabilities or characteristics that are required for the functionality of the class or interface.
         */
        public T Value { get; }
        public ConstrainedGenericClass(T value)
        {
            Value = value;
        }
        public void Display()
        {
            Console.WriteLine($"Value is : {Value}");
        }
    }

    public interface IEmployee
    {
        void Display(string value);
    }

    public class Engineer : IEmployee
    {
        public void Display(string value)
        {
            Console.WriteLine($"Engineer is : {value}");
        }
    }


    public class CompanyEntrance<T> where T : class , IEmployee
    {
        
        public static void Display(T employees)
        {
            Console.WriteLine("Welcome to the company");
        }
    }

}

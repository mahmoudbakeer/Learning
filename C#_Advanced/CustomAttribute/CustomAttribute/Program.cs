namespace CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property , AllowMultiple = true)]
    public class RangeAttribute : Attribute
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string ErrorMessage { get; set; }
        public RangeAttribute(int min ,int max)
        {
            Min = min; Max = max;
        }
    }
    public class Person 
    {
        [Range(18 , 19 , ErrorMessage = "Cannot exceed or be less than the located limits...")]
        public int Age { get; set; }

        [Range(3, 100, ErrorMessage = "Cannot exceed or be less than the located limits...")]
        public int Experience { get; set; }

        public Person(int age , int experience)
        {
            Age = age;
            Experience = experience;
        }
    }

    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Person person = new Person(23, 2);
            ValidatePerson(person);

        }
        public static bool ValidatePerson(Person person)
        {
            Type type = typeof(Person);

            foreach (var prop in type.GetProperties())
            {
                var attributes = prop.GetCustomAttributes(typeof(RangeAttribute), true);

                foreach (RangeAttribute range in attributes)
                {
                    int value = (int)prop.GetValue(person);

                    if (value < range.Min || value > range.Max)
                    {
                        Console.WriteLine(range.ErrorMessage);
                        return false;
                    }
                }
            }
            return true;
        }

    }
}

namespace UnionAndUnioinBy
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string color { get; set; }
        public Car(int id , string Name, string color)
        {
           this.Id = id;
           this.Name = Name;
           this.color = color;

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            int[] ints1 = { 5, 3, 9, 7, 5, 9, 3, 7 };
            int[] ints2 = { 8, 3, 6, 4, 4, 9, 1, 0 };

            IEnumerable<int> union = ints1.Union(ints2);

            foreach (int num in union)
            {
                Console.Write("{0} ", num);
            }
            Console.WriteLine();
            Console.WriteLine();
            /*
             This code produces the following output:

             5 3 9 7 8 6 4 1 0
            */


            // union by uses a key selector means based on which attributes you want to compare between elements
            List<Car> list1 = [new Car(1,"BMW","Black"),new Car(2,"Poarch","Violet"),new Car(3,"VWX","Black"),new Car(4,"Toyota","Red")];
            List<Car> list2 = [new Car(9,"BMW","white"),new Car(8,"Poarch","Blue"),new Car(5,"VWX","Yellow"),new Car(4,"Toyota","Brown")];


            var res =  list1.UnionBy(list2, c => $"{c.Name} - {c.color}" , StringComparer.OrdinalIgnoreCase).OrderBy(c => c.Name); 

            foreach (Car car in res)
            {
                Console.WriteLine($"Car name : {car.Name} \n\t Car Color : {car.color}");
            }
            Console.WriteLine($"Cars count : {res.Count()}");

        }
    }
}

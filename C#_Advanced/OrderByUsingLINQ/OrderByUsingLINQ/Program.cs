namespace LINQQueries
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // OrderBy : Sorts the elements of a sequence in ascending order according to a key
            // OrderByDescending : Sorts the elements of a sequence in ascending order by using a specified comparer.

            List<Car> cars = Car.GetCars();
            // var res = cars.OrderBy(c => c.Year);
            //var res = cars.OrderByDescending(c => c.Price);

            //Car.Print();
        }
    }
}

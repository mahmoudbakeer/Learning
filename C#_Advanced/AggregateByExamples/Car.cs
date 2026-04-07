public class Car
{
    // Properties
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }

    // Static Print Method
    public static void Print(IEnumerable<Car> cars)
    {
        var list = cars.ToList();

        if (!list.Any())
        {
            Console.WriteLine("No records found.");
            return;
        }

        // Calculate column widths
        var widths = new
        {
            Id = Math.Max(2, list.Max(c => c.Id.ToString().Length)),
            Brand = Math.Max(5, list.Max(c => c.Brand.Length)),
            Model = Math.Max(5, list.Max(c => c.Model.Length)),
            Year = 4,
            Price = Math.Max(5, list.Max(c => c.Price.ToString("N2").Length)),
            Color = Math.Max(5, list.Max(c => c.Color.Length))
        };

        // Helper to create separators
        string Separator(char corner = '+', char line = '-') =>
            corner + string.Join(corner, new[]
            {
            new string(line, widths.Id + 2),
            new string(line, widths.Brand + 2),
            new string(line, widths.Model + 2),
            new string(line, widths.Year + 2),
            new string(line, widths.Price + 2),
            new string(line, widths.Color + 2)
            }) + corner;

        // Print table
        Console.WriteLine(Separator());
        Console.WriteLine(
            $"| {"ID".PadRight(widths.Id)} " +
            $"| {"Brand".PadRight(widths.Brand)} " +
            $"| {"Model".PadRight(widths.Model)} " +
            $"| {"Year".PadRight(widths.Year)} " +
            $"| {"Price".PadLeft(widths.Price)} " +
            $"| {"Color".PadRight(widths.Color)} |"
        );
        Console.WriteLine(Separator());

        // Print rows with separators between each record
        for (int i = 0; i < list.Count; i++)
        {
            var c = list[i];
            Console.WriteLine(
                $"| {c.Id.ToString().PadRight(widths.Id)} " +
                $"| {c.Brand.PadRight(widths.Brand)} " +
                $"| {c.Model.PadRight(widths.Model)} " +
                $"| {c.Year.ToString().PadRight(widths.Year)} " +
                $"| {c.Price.ToString("N2").PadLeft(widths.Price)} " +
                $"| {c.Color.PadRight(widths.Color)} |"
            );

            // Add separator between records (except after the last one)
            if (i < list.Count - 1)
            {
                Console.WriteLine(Separator('+', '-'));
            }
        }

        Console.WriteLine(Separator());
        Console.WriteLine($"({list.Count} rows)");
    }
    public static List<Car> GetCars()
    {
        List<Car> cars = new List<Car>(1000);


        var realCars = new List<(string Brand, string Model, decimal BasePrice)>
                {
                    ("Toyota", "Corolla", 20000),
                    ("Toyota", "Camry", 25000),
                    ("Honda", "Civic", 22000),
                    ("Honda", "Accord", 27000),
                    ("BMW", "3 Series", 42000),
                    ("BMW", "X5", 60000),
                    ("Mercedes", "C-Class", 45000),
                    ("Mercedes", "E-Class", 55000),
                    ("Tesla", "Model 3", 40000),
                    ("Tesla", "Model S", 80000),
                    ("Ford", "Mustang", 55000),
                    ("Ford", "F-150", 50000),
                    ("Audi", "A4", 43000),
                    ("Audi", "Q7", 65000),
                    ("Hyundai", "Elantra", 21000),
                    ("Hyundai", "Tucson", 28000),
                    ("Kia", "Sportage", 26000),
                    ("Kia", "Sorento", 30000),
                    ("Nissan", "Altima", 24000),
                    ("Nissan", "GT-R", 100000)
                };

        string[] colors = { "White", "Black", "Silver", "Blue", "Red", "Gray" };

        int id = 1;

        // Generate exactly 1000 records (no randomness)
        while (cars.Count < 1000)
        {
            foreach (var car in realCars)
            {
                if (cars.Count >= 1000)
                    break;

                cars.Add(new Car
                {
                    Id = id++,
                    Brand = car.Brand,
                    Model = car.Model,
                    Year = 2010 + (id % 15),                 // cycles between 2010–2024
                    Price = car.BasePrice + (id % 5000),     // slight realistic variation
                    Color = colors[id % colors.Length]
                });
            }
        }
        return cars;
    }
}
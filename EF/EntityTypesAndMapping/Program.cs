

// this will make the compiler recognize this is the main
using EntityTypesAndMapping.Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("The Products : ");
foreach (var item in new AppDbContext().OrderGivenBill.FromSqlInterpolated($"SELECT * FROM GetOrderBill(1)").ToList())
{
    Console.WriteLine(item);
}

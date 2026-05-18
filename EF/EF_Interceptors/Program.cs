using EF_Interceptors.Data;
using EF_Interceptors.DataBaseHelper;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("To Show the Implement of SoftDeleting Interceptor :");
DatabaseHelper.ReCreateDataBase();
DatabaseHelper.PopulateDataBase();
Console.WriteLine("Before Deleting : ");
Console.WriteLine();
DatabaseHelper.PrintBooks();

// now lets delete one book to see if it worked
using (var context = new AppDbContext())
{
    var book = context.Books.First();
    context.Books.Remove(book);
    context.SaveChanges();
}
Console.WriteLine("After Deleting : ");
Console.WriteLine();
DatabaseHelper.PrintBooks();

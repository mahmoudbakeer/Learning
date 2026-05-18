// this project is intended to show the real tracking in the EF Core
using EF_ChangeTracker.Data;
using EF_ChangeTracker.Entities;

internal class Program
{
    private static void Main(string[] args)
    {
        UpdateTracking();
    }

    // methods
    public static void BasicTracking()
    {
        using (var context = new AppDbContext())
        {
            Console.WriteLine("Here the basic tracking : ");
            Console.WriteLine();
            var result = context.Courses.First(); // the linq query, on IQueryable
            context.ChangeTracker.DetectChanges(); // it will tell him to see the what has been changed with the stored snapshot by EF core
            result.Price = 3000;
            Console.WriteLine("Before the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            Console.WriteLine();

            // now we will save changes and print the after it
            context.SaveChanges();
            Console.WriteLine("After the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
        }
    }

    public static void InsertionTracking()
    {
        using (var context = new AppDbContext())
        {
            Console.WriteLine("Here the insertion tracking : ");
            Console.WriteLine();
            context.Authors.Add(new Author { Id = 3, AuthorName = "Khalid" });
            Console.WriteLine("Before the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            Console.WriteLine();

            // now we will save changes and print the after it
            context.SaveChanges();
            Console.WriteLine("After the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            var book = context.Authors.Where(auth => auth.Id == 3).FirstOrDefault();
            context.Authors.Remove(book);
            context.SaveChanges();
        }
    }

    public static void AttachTracking()
    {
        using (var context = new AppDbContext())
        {
            Console.WriteLine("Here the Attach tracking : ");
            Console.WriteLine();
            // lets says we got a record but not through this context, like we might go it from external resource
            // and its exist in the database, at the same time i want to tell the ef core to track all the changes will happend on this record
            // and update the database record accordingly
            // make sure the same entity info before attack it
            var author = new Author { Id = 1, AuthorName = "Hadi" };
            context.Attach(author);
            author.AuthorName = "Hado";
            Console.WriteLine("Before the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            Console.WriteLine();

            // now we will save changes and print the after it
            context.SaveChanges();
            Console.WriteLine("After the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            author.AuthorName = "Hadi";
            context.SaveChanges();
        }
    }

    public static void UpdateTracking()
    {
        using (var context = new AppDbContext())
        {
            Console.WriteLine("Here the Attach tracking : ");
            Console.WriteLine();
            // this will make attack then
            // update the database record accordingly
            // make sure the same entity info before attack it
            var author = new Author { Id = 1, AuthorName = "Hado" };
            context.Update(author);
            Console.WriteLine("Before the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            Console.WriteLine();

            // now we will save changes and print the after it
            context.SaveChanges();
            Console.WriteLine("After the saving : ");
            Console.WriteLine(context.ChangeTracker.DebugView.LongView);
            context.Attach(author);
            author.AuthorName = "Hadi";
            context.SaveChanges();
        }
    }
}

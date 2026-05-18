using EF_RelationShipsDeleting.Data;
using EF_RelationShipsDeleting.DataBaseHelper;
using Microsoft.EntityFrameworkCore;

// in this program i will study the relationships and the deleting behaviors between them
namespace EF_RelationShipsDeleting
{
    internal class Program
    {
        public static void Main()
        {
            TypicalUpdateInEFCore();
        }

        public static void CasCadeDelete()
        {
            using (var context = new AppDbContext())
            {
                DatabaseHelper.RecreateCleanDatabase();
                DatabaseHelper.PopulateDatabase();
                Console.WriteLine("The Cascade Delete : ");
                // deleting the main author will produce deleting in the all dependent entities if the relationship is required
                var author = context.Authors.Where(author => author.Id == 1).Single();
                context.Authors.Remove(author);
                Console.WriteLine("Before Saving changes : ");
                Console.WriteLine();
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                Console.WriteLine();

                context.SaveChanges();

                Console.WriteLine("After Saving Changes :");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                Console.WriteLine();
            }
        }

        // here we have changed the OnDelete(DeleteBehaviour.Restirct)
        public static void RestrictedDelete()
        {
            using (var context = new AppDbContext())
            {
                DatabaseHelper.RecreateCleanDatabase();
                DatabaseHelper.PopulateDatabase();
                Console.WriteLine("The Restirict Delete : ");
                // deleting the main author will produce deleting in the all dependent entities if the relationship is required
                var author = context.Authors.Where(author => author.Id == 1).Single();
                context.Authors.Remove(author);
                Console.WriteLine("Before Saving changes : ");
                Console.WriteLine();
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                Console.WriteLine();

                context.SaveChanges();

                Console.WriteLine("After Saving Changes :");
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                Console.WriteLine();
            }
        }

        public static void SeverRelationShip()
        {
            using (var context = new AppDbContext())
            {
                DatabaseHelper.RecreateCleanDatabase();
                DatabaseHelper.PopulateDatabase();
                Console.WriteLine("The Sever RelationShip : ");
                // now lets cut all the books relationship with the author
                // you must include the books to sever thier relationship
                // and make sure the relationship is not required
                var author = context
                    .AuthorV2s.Where(author => author.Id == 1)
                    .Include(author => author.BookV2s)
                    .Single();

                //set all the books to null to sever thier relationship
                author.BookV2s.Clear(); // here means remove them, the ef core will understand this and sever the relationship
                context.SaveChanges();
            }
        }

        public static void ServeFromTheChildSide()
        {
            using (var context = new AppDbContext())
            {
                DatabaseHelper.RecreateCleanDatabase();
                DatabaseHelper.PopulateDatabase();
                Console.WriteLine("The Sever From Child Side RelationShip : ");
                var author = context
                    .AuthorV2s.Where(author => author.Id == 1)
                    .Include(author => author.BookV2s)
                    .Single();
                foreach (var item in author.BookV2s)
                {
                    item.AuthorV2 = null; // the ef core will automatically understand this and cut the sever without deleting the childs
                }
                context.SaveChanges();
            }
        }

        // this work but its very inefficient, so if you have 1000 book then there is 1000 query going back and forth to update each book separatly
        public static void TypicalUpdateInEFCore()
        {
            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();

            using (var context = new AppDbContext())
            {
                var author = context
                    .Authors.Where(author => author.Id == 1)
                    .Include(author => author.Books)
                    .Single();

                // now manually we will update all the book prices and increase them by 10 percent
                foreach (var item in author.Books)
                {
                    item.Price *= 1.1m;
                }
                // now save the changes
                context.SaveChanges();
            }
        }

        public static void EfficientUpdate()
        {
            DatabaseHelper.RecreateCleanDatabase();
            DatabaseHelper.PopulateDatabase();
            using (var context = new AppDbContext())
            {
                // no need for the SaveChanges() here it excute it directly
                context
                    .Books.Where(book => book.AuthorId == 1)
                    .ExecuteUpdate(b => b.SetProperty(b => b.Price, b => b.Price * 1.1m));
            }
        }
        // or you can directly update it using the RawSql() method
    }
}

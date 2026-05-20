using EF_Transactions.Data;
using EF_Transactions.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_Transactions.DataBaseHelper
{
    public class DatabaseHelper
    {
        public static void ReCreateDataBase()
        {
            using (var context = new AppDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public static void PopulateDatabase()
        {
            using (var context = new AppDbContext())
            {
                context.Accounts.Add(
                    new Account()
                    {
                        Id = 1,
                        Balance = 1000m,
                        ClientName = "Mahmoud",
                    }
                );
                context.Accounts.Add(
                    new Account()
                    {
                        Id = 2,
                        Balance = 1000m,
                        ClientName = "Hamed",
                    }
                );
            }
        }
    }
}

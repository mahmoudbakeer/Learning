using EF_Transactions.Data;

internal class Program
{
    public static Random rand = new();

    private static void Main(string[] args)
    {
        Console.WriteLine();
    }

    public static void InitialTransactionSteps()
    {
        using (var context = new AppDbContext())
        {
            var account1 = context.Accounts.First(account => account.Id == 1);
            var account2 = context.Accounts.First(account => account.Id == 2);

            // first debit then deposit
            account1.WithDraw(100);
            context.SaveChanges();
            if (rand.Next(0, 2) == 1)
            {
                throw new NotImplementedException();
            }
            account2.Deposit(100);
            context.SaveChanges();

            // now here you will see, there is a 50% chance that this method will throw an error before depositing the money into the other account : account2
            // this is very bad thing and bad desing
        }
    }

    public static void SingleSaveTransactionSteps()
    {
        // this is a solution for the previous problem to use single SaveChanges at the end of the transaction means if it faild for some reason
        // it wont save any changes
        using (var context = new AppDbContext())
        {
            var account1 = context.Accounts.First(account => account.Id == 1);
            var account2 = context.Accounts.First(account => account.Id == 2);

            // first debit then deposit
            account1.WithDraw(100);
            if (rand.Next(0, 2) == 1)
            {
                throw new NotImplementedException();
            }
            account2.Deposit(100);
            context.SaveChanges();
        }
    }

    public static void EFCoreTransactionSteps()
    {
        using var context = new AppDbContext();

        // Start explicit transaction
        using var transaction = context.Database.BeginTransaction();

        try
        {
            var account1 = context.Accounts.First(account => account.Id == 1);
            var account2 = context.Accounts.First(account => account.Id == 2);

            account1.WithDraw(100);
            context.SaveChanges(); // Save 1: If we stop here, DB has it, but it's not committed

            if (rand.Next(0, 2) == 1)
            {
                throw new Exception("Simulated crash!");
            }

            account2.Deposit(100);
            context.SaveChanges(); // Save 2

            // If we reach this line, BOTH saves become permanent
            transaction.Commit();
        }
        catch (Exception)
        {
            // Explicit rollback is good practice, though disposing
            // an uncommitted transaction rolls it back automatically.
            transaction.Rollback();
            throw;
        }
    }
}

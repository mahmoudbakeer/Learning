namespace EF_Transactions.Entities
{
    public class GLTransaction
    {
        // Added 'private set' so EF Core can populate these from the database,
        // while keeping them read-only to the rest of your application.
        public int Id { get; private set; }
        public string Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal Amount { get; private set; }

        public Account Account { get; private set; }

        // Fixed the typo and changed the type to 'int'
        public int AccountId { get; private set; }

        // 1. Changed 'createdat' to 'createdAt' to match the property
        public GLTransaction(string notes, decimal amount, DateTime createdAt)
        {
            Notes = notes;
            Amount = amount;
            CreatedAt = createdAt;
        }

        // 2. Added a protected parameterless constructor.
        // You won't use this in your code, but EF Core will use it behind the scenes.
        protected GLTransaction() { }
    }
}

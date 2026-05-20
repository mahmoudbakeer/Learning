namespace EF_Transactions.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public decimal Balance { get; set; }
        public List<GLTransaction> Transactions { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Transactions.Add(new GLTransaction("Deposit", amount, DateTime.Now));
                Balance += amount;
            }
        }

        public void WithDraw(decimal amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                Transactions.Add(new GLTransaction("WithDraw", amount * -1, DateTime.Now));
            }
        }
    }
}

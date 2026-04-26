// we want to use dapper to get the data
class Wallet
{
    public int Id { get; set; }
    public string Holder { get; set; }
    public decimal Balance { get; set; }


    public override string ToString()
    {
        return $"ID : {Id} - Holder : {Holder} - Balance  : {Balance:C}";
    }
}

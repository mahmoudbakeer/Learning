namespace EF_ChangeTracker.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string BookName { get; set; }
    }
}

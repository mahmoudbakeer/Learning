using EF_Interceptors.Entities;

namespace EF_Interceptors
{
    public class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = null!;
        public List<Book> Books { get; set; } = null!;
    }
}

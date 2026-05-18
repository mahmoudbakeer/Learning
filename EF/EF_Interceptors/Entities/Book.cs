using EF_Interceptors.Entities.Contracts;

namespace EF_Interceptors.Entities
{
    public class Book : ISoftDelete
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            TimeDeleted = DateTime.Now;
        }

        public void UndoDelete()
        {
            IsDeleted = false;
            TimeDeleted = null;
        }
    }
}

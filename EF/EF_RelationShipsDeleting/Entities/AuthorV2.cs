namespace EF_RelationShipsDeleting.Entities
{
    public class AuthorV2
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public List<BookV2> BookV2s { get; set; }
    }
}

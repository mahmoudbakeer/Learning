namespace EF_RelationShipsDeleting.Entities
{
    public class BookV2
    {
        public int Id { get; set; }
        public int? AuthorV2Id { get; set; }
        public AuthorV2? AuthorV2 { get; set; }
        public string BookName { get; set; }
    }
}

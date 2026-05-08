namespace EF_RawSql.Entities
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public Student Student { get; set; } = null!;
        public Section Section { get; set; } = null!;
    }
}

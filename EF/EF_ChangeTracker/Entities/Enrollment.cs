namespace EF_ChangeTracker.Entities
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public Student Student { get; set; } = null!;
        public Section Section { get; set; } = null!;
    }
}

namespace EF_QueryData01.Entities
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public Student Student { get; set; }
        public Section Section { get; set; }
    }
}

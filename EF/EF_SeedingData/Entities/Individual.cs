namespace EF_SeedingData.Entities
{
    public class Individual : Student
    {
        public string University { get; set; }
        public int YearOfGraduation { get; set; }
        public bool IsIntern {  get; set; }
    }
}

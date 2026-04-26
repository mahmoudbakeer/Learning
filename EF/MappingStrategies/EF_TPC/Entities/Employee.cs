namespace Migrations_001.Entities
{
    public class Employee : Student
    {
        public string Company { get; set; }
        public int YearsOfExperience { get; set; }
        public string Title { get; set; }
    }
}

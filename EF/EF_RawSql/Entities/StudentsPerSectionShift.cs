public class StudentsPerSectionShift
{
    public string SectionName { get; set; }
    public int StudentsCount { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public override string ToString()
    {
        return $"SectionName : {SectionName}, NumberOfStudentsAttending : {StudentsCount}, TimePeriod : from {StartTime} to {EndTime}";
    }
}

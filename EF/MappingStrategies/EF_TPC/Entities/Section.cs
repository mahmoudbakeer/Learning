using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrations_001.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int CourseId { get; set; } // means required relationship
        public Course Course { get; set; } // means required relationship
        public int? InstructorId { get; set; } // means optional relationship
        public Instructor? Instructor { get; set; } // means optional relationship
        public TimeSlot TimeSlot { get; set; } // owned property
        public Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }
        //public ICollection<SectionSchedule> SectionSchedules { get; set; } = new List<SectionSchedule>();
        public ICollection<Enrollment> Enrollments { get; set; }
    }
    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}

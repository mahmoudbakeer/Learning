using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SeedingData.Entities
{
    // Principle (Parent)
    public class Office
    {
        public int Id { get; set; }
        public string OfficeName { get; set; }
        public string OfficeLocation { get; set; }
        public Instructor? Instructor { get; set; }
    }
}

using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SeedingData.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}

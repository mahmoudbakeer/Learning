using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrations_001.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
}

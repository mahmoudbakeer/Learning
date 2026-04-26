using Migrations_001.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_TPC.Entities
{
    public abstract class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; } // this means no quiz without Course, required foreign key
    }
}

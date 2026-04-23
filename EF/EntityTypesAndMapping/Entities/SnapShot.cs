using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTypesAndMapping.Entities
{
    // exclude using dataAnnotation
    //[NotMapped]
    public class SnapShot
    {
        public DateTime Time => DateTime.Now;
        public string version => Guid.NewGuid().ToString().Substring(0,9);
    }
}

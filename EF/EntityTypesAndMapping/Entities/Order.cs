using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTypesAndMapping.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OderDate { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; } = Enumerable.Empty<OrderDetail>();
    }
}


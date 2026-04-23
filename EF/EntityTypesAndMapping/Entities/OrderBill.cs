using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityTypesAndMapping.Entities
{
    public class OrderBill
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }

        public override string ToString()
        {
            return $"{ProductName} - {Quantity} - {UnitPrice} - {SubTotal}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_CA.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public List<Product> SaleProducts { get; set; } = new List<Product>();
        public Sale(){}

    }
}

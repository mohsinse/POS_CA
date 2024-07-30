using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POS.Model
{

    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public List<Product> SaleProducts { get; set; } = new List<Product>();

        public Sale() { }
    }

}

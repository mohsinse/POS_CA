using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DTO
{
    public class SaleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "SaleProducts is required.")]
        public List<ProductDTO> SaleProducts { get; set; } = new List<ProductDTO>();

        // Parameterless constructor for model binding
        public SaleDTO() { }

        public SaleDTO(int id, List<ProductDTO> saleProducts)
        {
            Id = id;
            SaleProducts = saleProducts;
        }
    }
}

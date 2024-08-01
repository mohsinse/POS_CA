using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DTO
{
    public class CategoryDTO
    {
        public int? Id { get; set; } // Nullable for creation scenarios

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Parameterless constructor for model binding
        public CategoryDTO() { }

        // Constructor for convenience
        public CategoryDTO(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

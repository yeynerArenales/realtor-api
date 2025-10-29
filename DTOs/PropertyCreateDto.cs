using System.ComponentModel.DataAnnotations;

namespace realtorAPI.DTOs
{
    public class PropertyCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100)]
        public string CodeInternal { get; set; } = string.Empty;

        [Range(1800, 3000)]
        public int Year { get; set; }

        [Required]
        public string IdOwner { get; set; } = string.Empty;
    }
}


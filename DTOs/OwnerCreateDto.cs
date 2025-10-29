using System.ComponentModel.DataAnnotations;

namespace realtorAPI.DTOs
{
    public class OwnerCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Address { get; set; } = string.Empty;

        [Url]
        public string? Photo { get; set; }

        public DateTime? Birthday { get; set; }
    }
}


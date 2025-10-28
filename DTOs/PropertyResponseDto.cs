namespace realtorAPI.DTOs
{
    public class PropertyResponseDto
    {
        public string? Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}


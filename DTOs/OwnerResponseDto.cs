namespace realtorAPI.DTOs
{
    public class OwnerResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public DateTime? Birthday { get; set; }
    }
}


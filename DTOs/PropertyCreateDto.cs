namespace realtorAPI.DTOs
{
    public class PropertyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = string.Empty;
        public int Year { get; set; }
        public string IdOwner { get; set; } = string.Empty;
    }
}

